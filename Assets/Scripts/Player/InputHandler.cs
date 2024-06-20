using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLMJ
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool rollFlag;
        public bool sprintFlag;
        public float rollInputTimer;
        public bool comboFlag;

        public bool b_Input;
        public bool a_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool jump_Input;
        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;
        public bool lockOnFlag;
        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }
        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleJumpInput();
            HandleQuickSlotsInput();
            HandleLockOnInput();
            HandleInteractingButtonInput();
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (b_Input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if(rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }
    
        private void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;

            if (rb_Input)
            {
                playerAttacker.HandleRBAction();
            }

            if(rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleJumpInput()
        {
          inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
        }

        private void HandleQuickSlotsInput()
        {
            inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;

            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if(d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }
        }

        private void HandleInteractingButtonInput()
        {
            inputActions.PlayerActions.A.performed += i => a_Input = true;
        }
    }
}