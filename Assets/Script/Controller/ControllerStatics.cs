using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Tundayne/Controller Statics", fileName = "ControllerStatics")]
    public class ControllerStatics : ScriptableObject
    {
        [Header("Character Statistics")]
        public float moveSpeed = 4f;
        public float sprintSpeed = 6f;
        public float crouchSpeed = 2f;
        public float aimSpeed = 2f;
        public float rotationSpeed = 8f;
    }
}

