using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tundayne
{
    public class AnimatorHook : MonoBehaviour
    {
        Animator anim;
        StatesManager states;

        float m_h_weight;
        float o_h_weight;
        float l_weight;
        float b_weight;

        Transform rh_target;
        public Transform lh_target;
        Transform shoulder;
        Transform aimPivot;
        Vector3 lookDir;

        public bool disable_o_h;
        public bool disable_m_h = true; // Variable has been set to true.

        public void Init(StatesManager st)
        {
            states = st;
            anim = states.anim;

            shoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform;
            aimPivot = new GameObject().transform;
            aimPivot.name = "aim pivot";
            aimPivot.transform.parent = states.transform;
            rh_target = new GameObject().transform;
            rh_target.name = "right hand target";
            rh_target.parent = aimPivot;
            states.input.aimPosition = states.transform.position + transform.forward * 15;
            states.input.aimPosition.y += 1.4f;
        }

        void OnAnimatorMove()
        {
            lookDir = states.input.aimPosition - aimPivot.position;
            HandleShoulder();
        }

        void HandleShoulder()
        {
            HandleShoulderPosition();
        }

        void HandleShoulderPosition()
        {
            aimPivot.position = shoulder.position;
        }

        void HandleShoulderRotation()
        {
            Vector3 targetDir = lookDir;
            if (targetDir == Vector3.zero)
            {
                targetDir = aimPivot.forward;
            }

            Quaternion tr = Quaternion.LookRotation(targetDir);
            aimPivot.rotation = Quaternion.Slerp(aimPivot.rotation, tr, states.delta * 15);

        }

        void HandleWeights()
        {
            if (states.statesManager.isInteracting)
            {
                m_h_weight = 0;
                o_h_weight = 0;
                l_weight = 0;
                return;
            }

            float t_l_weight = 0;
            float t_m_weight = 0;

            if (states.statesManager.isAiming)
            {
                t_m_weight = 1;
                b_weight = 0.4f;
            }
            else
            {
                b_weight = 0.3f;
            }

            if (disable_m_h)
            {
                t_m_weight = 0;
            }

            if (lh_target != null)
            {
                o_h_weight = 1;
            }
            else
            {
                o_h_weight = 0;
            }

            if (disable_o_h)
            {
                o_h_weight = 0;
            }

            Vector3 ld = states.input.aimPosition - states.mTransform.position;
            float angle = Vector3.Angle(states.mTransform.forward, ld);
            if (angle < 76)
            {
                t_l_weight = 1;
            }
            else
            {
                t_l_weight = 0;
            }

            if (angle > 45)
            {
                t_m_weight = 0;
            }

            l_weight = Mathf.Lerp(l_weight, t_l_weight, states.delta * 3);
            m_h_weight = Mathf.Lerp(m_h_weight, t_m_weight, states.delta * 9);
        }

        void OnAnimatorIK()
        {
            HandleWeights();

            anim.SetLookAtWeight(l_weight, b_weight, 1, 1, 1);
            anim.SetLookAtPosition(states.input.aimPosition);

            if (lh_target != null)
            {
                UpdateIK(AvatarIKGoal.LeftHand, lh_target, o_h_weight);
            }

            UpdateIK(AvatarIKGoal.RightHand, rh_target, m_h_weight);
        }

        void UpdateIK(AvatarIKGoal goal, Transform t, float w)
        {
            anim.SetIKPositionWeight(goal, w);
            anim.SetIKRotationWeight(goal, w);
            anim.SetIKPosition(goal, t.position);
            anim.SetIKRotation(goal, t.rotation);
        }

        public void Tick()
        {
            RecoilActual();
        }

        #region Recoil
        float recoilT;
        Vector3 offsetPosition;
        Vector3 offsetRotation;
        Vector3 basePosition;
        Vector3 baseRotation;
        bool recoilIsInit;

        public void RecoilAnim()
        {
            if (!recoilIsInit)
            {
                recoilIsInit = true;
                recoilT = 0;
                offsetPosition = Vector3.zero;

            }
        }

        public void RecoilActual()
        {
            if (recoilIsInit)
            {
                recoilT += states.delta * 3;
                if (recoilT > 1)
                {
                    recoilT = 1;
                    recoilIsInit = false;
                }

                offsetPosition = Vector3.forward;
                offsetRotation = Vector3.right * 90;

                rh_target.localPosition = basePosition + offsetPosition;
                rh_target.localEulerAngles = baseRotation + offsetRotation;
            }
        }
        #endregion
    }
}