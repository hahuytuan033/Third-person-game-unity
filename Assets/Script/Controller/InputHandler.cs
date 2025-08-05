using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class InputHandler : MonoBehaviour
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
        public CameraHandler cameraHandler;

        void Start()
        {
            InitInGame();
        }

        public void InitInGame()
        {
            states.Init();
            cameraHandler.Init(this);
            isInit = true;
        }

        #region FixedUpdate
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

            cameraHandler.FixedTick(delta);


        }

        void GetInputFixedUpdate()
        {
            vertical = Input.GetAxis(StaticStrings.Vertical);
            horizontal = Input.GetAxis(StaticStrings.Horizontal);
        }

        void InGame_UpdateStates_FixedUpdate()
        {
            states.input.horizontal = horizontal;
            states.input.vertical = vertical;

            states.input.moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            Vector3 moveDirection = cameraHandler.mTransform.forward * vertical;
            moveDirection += cameraHandler.mTransform.right * horizontal;
            moveDirection.Normalize();
            states.input.moveDirection = moveDirection;

            states.input.rotateDirection = cameraHandler.mTransform.forward;
        }
        #endregion


        #region Update
        void Update()
        {
            if (!isInit)
            {
                return;
            }
            delta = Time.deltaTime;
            GetInput_Update();

            InGame_UpdateStates_Update();
            states.Tick(delta);
        }

        void InGame_UpdateStates_Update()
        {
            states.statesManager.isAiming = aimInput;
        }

        void GetInput_Update()
        {
            aimInput = Input.GetMouseButton(1);
            Debug.Log(aimInput);

        }
        #endregion
    }

    public enum GamePhase
    {
        inGame,
        inMenu
    }
}

