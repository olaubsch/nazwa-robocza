using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace OLMJ
{
    public class InputHandlerTests
    {
        private GameObject playerGameObject;
        private InputHandler inputHandler;
        private PlayerControlsMock playerControlsMock;
        private PlayerAttackerMock playerAttackerMock;
        private PlayerInventoryMock playerInventoryMock;
        private PlayerManagerMock playerManagerMock;
        private CameraHandlerMock cameraHandlerMock;

        [SetUp]
        public void Setup()
        {
            // Inicjalizacja przed każdym testem
            playerGameObject = new GameObject();
            inputHandler = playerGameObject.AddComponent<InputHandler>();

            // Mocks
            playerControlsMock = new PlayerControlsMock();
            playerAttackerMock = new PlayerAttackerMock();
            playerInventoryMock = new PlayerInventoryMock();
            playerManagerMock = new PlayerManagerMock();
            cameraHandlerMock = new CameraHandlerMock();

            inputHandler.inputActions = playerControlsMock;
            inputHandler.playerAttacker = playerAttackerMock;
            inputHandler.playerInventory = playerInventoryMock;
            inputHandler.playerManager = playerManagerMock;
            inputHandler.cameraHandler = cameraHandlerMock;

            inputHandler.OnEnable(); 
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerGameObject);
        }

        [UnityTest]
        public IEnumerator MoveInput_CorrectValues()
        {

            playerControlsMock.InvokeMovement(new Vector2(0.3f, 0.6f));
            playerControlsMock.InvokeCamera(new Vector2(0.1f, 0.2f));

            yield return null; 
            Assert.AreEqual(0.3f, inputHandler.horizontal);
            Assert.AreEqual(0.6f, inputHandler.vertical);
            Assert.AreEqual(0.9f, inputHandler.moveAmount); 
            Assert.AreEqual(0.1f, inputHandler.mouseX);
            Assert.AreEqual(0.2f, inputHandler.mouseY);
        }

        [UnityTest]
        public IEnumerator HandleRollInput_RollFlagSet()
        {

            playerControlsMock.InvokeRoll();

            yield return null; 
            Assert.IsTrue(inputHandler.rollFlag);
        }

        [UnityTest]
        public IEnumerator HandleAttackInput_RBInputHandled()
        {
            playerControlsMock.InvokeRB();

            yield return null; 
            Assert.IsTrue(playerAttackerMock.rbActionHandled);
        }

        [UnityTest]
        public IEnumerator HandleJumpInput_JumpInputDetected()
        {

            playerControlsMock.InvokeJump();

            yield return null; 
            Assert.IsTrue(inputHandler.jump_Input);
        }

        [UnityTest]
        public IEnumerator HandleQuickSlotsInput_DPadRightChangeRightWeapon()
        {

            playerControlsMock.InvokeDPadRight();

            yield return null;
            Assert.IsTrue(playerInventoryMock.rightWeaponChanged);
        }

        [UnityTest]
        public IEnumerator HandleLockOnInput_LockOnInputHandled()
        {
            playerControlsMock.InvokeLockOn();

            yield return null;
            Assert.IsTrue(cameraHandlerMock.lockOnHandled);
        }

        [UnityTest]
        public IEnumerator HandleInteractingButtonInput_AInputDetected()
        {

            playerControlsMock.InvokeA();

            yield return null; 
            Assert.IsTrue(inputHandler.a_Input);
        }
    }

    public class PlayerControlsMock : PlayerControls
    {
        private System.Action<Vector2> onMovement;
        private System.Action<Vector2> onCamera;
        private System.Action onRB;
        private System.Action onJump;
        private System.Action onDPadRight;
        private System.Action onLockOn;
        private bool enabled = true;

        public void InvokeMovement(Vector2 input)
        {
            if (onMovement != null)
                onMovement.Invoke(input);
        }

        public void InvokeCamera(Vector2 input)
        {
            if (onCamera != null)
                onCamera.Invoke(input);
        }

        public void InvokeRB()
        {
            if (onRB != null)
                onRB.Invoke();
        }

        public void InvokeJump()
        {
            if (onJump != null)
                onJump.Invoke();
        }

        public void InvokeDPadRight()
        {
            if (onDPadRight != null)
                onDPadRight.Invoke();
        }

        public void InvokeLockOn()
        {
            if (onLockOn != null)
                onLockOn.Invoke();
        }

        public override void Enable()
        {
            enabled = true;
        }

        public override void Disable()
        {
            enabled = false;
        }

        public override bool enabled => enabled;

        public override PlayerMovementActions PlayerMovement => new PlayerMovementActions { Movement = new InputAction.CallbackContext { action = new InputAction() } };
        public override PlayerMovementActions PlayerActions => new PlayerMovementActions { RB = new InputAction.CallbackContext { action = new InputAction() }, Jump = new InputAction.CallbackContext { action = new InputAction() } };
        public override PlayerMovementActions PlayerQuickSlots => new PlayerMovementActions { DPadRight = new InputAction.CallbackContext { action = new InputAction() } };
        public override PlayerActions PlayerActions => new PlayerActions { LockOn = new InputAction.CallbackContext { action = new InputAction() } };
    }

    public class PlayerAttackerMock : PlayerAttacker
    {
        public bool rbActionHandled;

        public override void HandleRBAction()
        {
            rbActionHandled = true;
        }

    }
    public class PlayerInventoryMock : PlayerInventory
    {
        public bool rightWeaponChanged;

        public override void ChangeRightWeapon()
        {
            rightWeaponChanged = true;
        }
    }

    public class PlayerManagerMock : PlayerManager
    {
        public override bool isJumping { get; set; }
    }

    public class CameraHandlerMock : CameraHandler
    {
        public bool lockOnHandled;

        public override void HandleLockOn()
        {
            lockOnHandled = true;
        }
    }
}
