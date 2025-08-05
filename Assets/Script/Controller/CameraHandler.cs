using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tundayne
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform camTrans;
        public Transform target;
        public Transform pivot;
        public Transform mTransform;
        public bool leftPivot;
        float delta;

        float mouseX;
        float mouseY;
        float smoothX;
        float smoothY;
        float smoothXVelocity;
        float smoothYvelocity;
        float lookAngle;
        float tiltAngle;

        public CameraValues cameraValues;
        StatesManager states;

        public void Init(InputHandler inputHandler)
        {
            mTransform = this.transform;
            states = inputHandler.states;
            
            target = states.mTransform;
        }

        public void FixedTick(float d)
        {
            delta = d;
            if (target == null)
            {
                return;
            }
            HandlePosition();
            HandleRotation();

            float speed = cameraValues.moveSpeed;
            if (states.statesManager.isAiming)
            {
                speed = cameraValues.aimSpeed;
            }
            Vector3 targetPosition = Vector3.Lerp(mTransform.position, target.position, delta * speed);
            mTransform.position = targetPosition;
        }

        public void HandlePosition()
        {
            float targetX = cameraValues.normalX;
            float targetZ = cameraValues.normalZ;
            float targetY = cameraValues.normalY;

            if (states.statesManager.isCrouching)
            {
                targetY = cameraValues.crounchY;
            }

            if (states.statesManager.isAiming)
            {
                targetX = cameraValues.aimX;
                targetZ = cameraValues.aimZ;
            }

            if (leftPivot)
            {
                targetX = -targetX;
            }

            Vector3 newPivotPosition = pivot.localPosition;
            newPivotPosition.x = targetX;
            newPivotPosition.y = targetY;

            Vector3 newCamPosition = camTrans.localPosition;
            newCamPosition.z = targetZ;

            float t = delta * cameraValues.adaptSpeed;
            pivot.localPosition = Vector3.Lerp(pivot.localPosition, newPivotPosition, t);
            camTrans.localPosition = Vector3.Lerp(camTrans.localPosition, newCamPosition, t);

        }

        public void HandleRotation()
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            if (cameraValues.turnSmooth > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXVelocity, cameraValues.turnSmooth);
                smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothYvelocity, cameraValues.turnSmooth);
            }
            else
            {
                smoothX = mouseX;
                smoothY = mouseY;
            }
            lookAngle += smoothX * cameraValues.yRotationSpeed;
            Quaternion targetRotation = Quaternion.Euler(0, lookAngle, 0);
            transform.rotation = targetRotation;

            tiltAngle -= smoothY * cameraValues.xRotationSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, cameraValues.minAngle, cameraValues.maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);


        }
    }

}
