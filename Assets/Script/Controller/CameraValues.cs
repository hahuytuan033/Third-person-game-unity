using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(fileName = "CameraValues", menuName = "Tundayne/CameraValues", order = 1)]
    public class CameraValues : ScriptableObject
    {
        [Header("Speed and Smooth Camera")]
        public float turnSmooth = 0.1f;
        public float moveSpeed = 9f;
        public float adaptSpeed = 9f;

        [Header("Rotation Camera")]
        public float yRotationSpeed = 8f;
        public float xRotationSpeed = 8f;
        public float minAngle = -35f;
        public float maxAngle = 35f;

        [Header("Camera Postion")]
        public float normalZ = -3f;
        public float normalX;
        public float normalY;
        public float crounchY;

        [Header("Aiming Camera Postion")]
        public float aimSpeed = 25f;
        public float aimZ = -0.5f;
        public float aimX = 0;

    }
}

