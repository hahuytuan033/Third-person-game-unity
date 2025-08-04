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

        public Animator anim;
        public GameObject activeModel;
        [HideInInspector] public Rigidbody rigid;
        [HideInInspector] public Collider controllerCollider;
        
        [HideInInspector]public LayerMask ignoreLayer;
        [HideInInspector]public LayerMask ignoreForGround;

        public Transform mTransform;
        public CharState currentState;
        public float delta;

        List<Collider> ragdollColliders = new List<Collider>();
        List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();

        public void Init()
        {
            SetUpAnimator();
            rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.drag = 4;
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            controllerCollider = GetComponent<Collider>();

            SetupRagdoll();
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

                    }
                    else
                    {
                        MovementNormal();
                        RotationNormal();
                    }
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
            Vector3 targetDir = input.moveDirection;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
            {
                targetDir = mTransform.forward;
            }

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(mTransform.rotation, lookDir, controllerStatics.rotationSpeed * delta);
            mTransform.rotation = targetRotation;
        }

        void HandleAnimationsNormal()
        {
            float animVertical = input.moveAmount;
            anim.SetFloat("Vertical", animVertical, 0.15f, delta);
        }

        public void MovemtentAiming()
        {

        }

        public void Tick(float d)
        {
            delta = d;

            switch (currentState)
            {
                case CharState.normal:
                    statesManager.onGround = OnGround();
                    HandleAnimationsNormal();
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
