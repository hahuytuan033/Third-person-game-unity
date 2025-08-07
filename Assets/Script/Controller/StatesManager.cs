using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class StatesManager : MonoBehaviour
    {
        [System.Serializable]
        public class InputVariables
        {
            public float horizontal;
            public float vertical;
            public float moveAmount;
            public Vector3 moveDirection;
            public Vector3 aimPosition;
            public Vector3 rotateDirection;
        }

        [System.Serializable]
        public class ControllerStates
        {
            [Header("Character States")]
            public bool onGround;
            public bool isAiming;
            public bool isCrouching;
            public bool isRunning;
            public bool isInteracting;
        }

        public ControllerStates statesManager;
        public ControllerStatics controllerStatics;
        public InputVariables input;
        public WeaponManager weaponManager;
        public ResourcesManager resourcesManager;

        #region References
        public Animator anim;
        public GameObject activeModel;
        [HideInInspector] public AnimatorHook animatorHook;
        [HideInInspector] public Rigidbody rigid;
        [HideInInspector] public Collider controllerCollider;

        [HideInInspector] public LayerMask ignoreLayer;
        [HideInInspector] public LayerMask ignoreForGround;

        //[HideInInspector] public Transform referencesParent ;

        [HideInInspector] public Transform mTransform;
        public CharState currentState;
        public float delta;

        List<Collider> ragdollColliders = new List<Collider>();
        List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
        #endregion

        #region Init
        public void Init()
        {
            resourcesManager.Init();
            mTransform = this.transform;
            SetUpAnimator();
            rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.drag = 4;
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            controllerCollider = GetComponent<Collider>();

            SetupRagdoll();

            ignoreLayer = ~(1 << 9);
            ignoreForGround = ~(1 << 9 | 1 << 10);

            animatorHook = activeModel.AddComponent<AnimatorHook>();
            animatorHook.Init(this);

            Init_WeaponManger();
        }

        void SetUpAnimator()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                activeModel = anim.gameObject;
            }

            if (anim == null)
            {
                anim = activeModel.GetComponent<Animator>();
            }

            anim.applyRootMotion = false;
        }

        void SetupRagdoll()
        {
            Rigidbody[] rigids = activeModel.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigids)
            {
                if (rb == rigid)
                {
                    continue;
                }

                Collider c = rb.gameObject.GetComponent<Collider>();
                c.isTrigger = true;

                ragdollRigidbodies.Add(rb);
                ragdollColliders.Add(c);
                rb.isKinematic = true;
                rb.gameObject.layer = 10;

                ignoreLayer = ~(1 << 9);
                ignoreForGround = ~(1 << 9 | 1 << 10);
            }
        }
        #endregion

        #region FixedUpdate
        public void FixedTick(float d)
        {
            delta = d;
            mTransform = activeModel.transform;

            switch (currentState)
            {
                case CharState.normal:
                    statesManager.onGround = OnGround();

                    if (statesManager.isAiming)
                    {
                        // Khi nhắm
                        MovemtentAiming();
                    }
                    else
                    {
                        // Khi không nhắm
                        MovementNormal();
                    }

                    RotationNormal();
                    break;
                case CharState.onAir:
                    statesManager.onGround = OnGround();
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
            }
        }

        void MovementNormal()
        {
            if (input.moveAmount > 0.05f)
            {
                rigid.drag = 0;
            }
            else
            {
                rigid.drag = 4;
            }

            float speed = controllerStatics.moveSpeed;
            if (statesManager.isRunning)
            {
                speed = controllerStatics.sprintSpeed;
            }
            if (statesManager.isCrouching)
            {
                speed = controllerStatics.crouchSpeed;
            }

            Vector3 dir = Vector3.zero;
            dir = mTransform.forward * (speed * input.moveAmount);
            rigid.velocity = dir;
        }

        void RotationNormal()
        {
            Vector3 targetDir;

            if (statesManager.isAiming)
            {
                targetDir = input.rotateDirection;
            }
            else
            {
                targetDir = input.moveDirection;
            }

            targetDir.y = 0;
            if (targetDir == Vector3.zero)
            {
                targetDir = mTransform.forward;
            }

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(mTransform.rotation, lookDir, controllerStatics.rotationSpeed * delta);
            mTransform.rotation = targetRotation;
        }

        public void MovemtentAiming()
        {
            float speed = controllerStatics.aimSpeed;
            Vector3 v = input.moveDirection * speed;
            rigid.velocity = v;
        }
        #endregion


        #region Update
        public void Tick(float d)
        {
            delta = d;

            switch (currentState)
            {
                case CharState.normal:
                    statesManager.onGround = OnGround();
                    HandleAnimationAll();
                    break;
                case CharState.onAir:
                    rigid.drag = 0;
                    statesManager.onGround = OnGround();
                    break;
                case CharState.cover:
                    break;
                case CharState.vaulting:
                    break;
            }
        }
        void HandleAnimationAll()
        {
            anim.SetBool(StaticStrings.sprint, statesManager.isRunning);
            anim.SetBool(StaticStrings.aiming, statesManager.isAiming);
            anim.SetBool(StaticStrings.crouch, statesManager.isCrouching);

            if (statesManager.isAiming)
            {
                HandleAnimationsAiming();
            }
            else
            {
                HandleAnimationsNormal();
            }
        }

        void HandleAnimationsNormal()
        {
            if (input.moveAmount > 0.05f)
            {
                rigid.drag = 0;
            }
            else
            {
                rigid.drag = 4;
            }

            float animVertical = input.moveAmount;
            anim.SetFloat(StaticStrings.Vertical, animVertical, 0.15f, delta);
        }

        void HandleAnimationsAiming()
        {
            float vertical = input.vertical;
            float horizontal = input.horizontal;

            anim.SetFloat(StaticStrings.Horizontal, horizontal, 0.2f, delta);
            anim.SetFloat(StaticStrings.Vertical, vertical, 0.2f, delta);
        }
        #endregion

        #region Manager Functions
        public void Init_WeaponManger()
        {
            CreateRunTimeWeapon(weaponManager.mainWeapon_id, ref weaponManager.m_weapon);
            //EquiqRuntimeWeapon(weaponManager.m_weapon);
        }

        public void CreateRunTimeWeapon(string id, ref RuntimeWeapon r_w_m)
        {
            Weapon w = resourcesManager.GetWeapon(id);
            RuntimeWeapon rm = resourcesManager.runtime.WeaponToRuntimeWeapon(w);

            GameObject gameObject = Instantiate(w.modelPrefab);
            rm.m_instance = gameObject;
            rm.weaponActual = w;
            rm.weaponHook = gameObject.GetComponent<WeaponHook>();
            gameObject.SetActive(false);

            Transform p = anim.GetBoneTransform(HumanBodyBones.RightHand);
            gameObject.transform.parent = p;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;

            r_w_m = rm;
        }

        public void EquiqRuntimeWeapon(RuntimeWeapon rw)
        {
            rw.m_instance.SetActive(true);
            animatorHook.EquiqWeapon(rw);
         }

        #endregion
        bool OnGround()
        {
            Vector3 origin = mTransform.position;
            origin.y += 0.6f;
            Vector3 direction = -Vector3.up;
            float distance = 0.7f;
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, distance, ignoreForGround))
            {
                Vector3 transformPosition = hit.point;
                mTransform.position = transformPosition;
                return true;
            }
            return false;
        }
    }

    public enum CharState
    {
        normal,
        onAir,
        cover,
        vaulting
    }
}