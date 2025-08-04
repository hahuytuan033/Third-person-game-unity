using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class InputHandle : MonoBehaviour
    {
        float horizontal;
        float vertical;

        bool aimInput;
        bool sprintInput;
        bool shootInput;
        bool crouchInput;
        bool reloadInput;
        bool switchInput;
        bool pivotInput;

        bool isInit;
        float delta;

        public StatesManager states;
        public Transform cameraHolder;

        void Start()
        {
            InitInGame();
        }

        public void InitInGame()
        {
            states.Init();
            isInit = true;
        }

        void FixedUpdate()
        {
            if (!isInit)
            {
                return;
            }
            delta = Time.fixedDeltaTime;
            GetInputFixedUpdate();
            InGame_UpdateStates_FixedUpdate();
            states.FixedTick(delta);
        }

        void GetInputFixedUpdate()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }

        void InGame_UpdateStates_FixedUpdate()
        {
            states.input.horizontal = horizontal;
            states.input.vertical = vertical;

            states.input.moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            Vector3 moveDirection = cameraHolder.forward * vertical;
            moveDirection += cameraHolder.right * horizontal;
            moveDirection.Normalize();
            states.input.moveDirection = moveDirection;


        }

        void Update()
        {
            if (!isInit)
            {
                return;
            }
            delta = Time.deltaTime;

            states.Tick(delta);
        }
    }

    public enum GamePhase
    {
        inGame,
        inMenu
    }
}

