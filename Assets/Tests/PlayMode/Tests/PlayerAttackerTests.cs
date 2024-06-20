using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace OLMJ
{
    public class PlayerAttackerTests
    {
        private GameObject playerGameObject;
        private PlayerAttacker playerAttacker;
        private AnimatorHandlerMock animatorHandlerMock;
        private InputHandlerMock inputHandlerMock;
        private PlayerManagerMock playerManagerMock;
        private PlayerInventoryMock playerInventoryMock;
        private WeaponSlotManagerMock weaponSlotManagerMock;

        [SetUp]
        public void Setup()
        {
            playerGameObject = new GameObject();
            playerAttacker = playerGameObject.AddComponent<PlayerAttacker>();

            animatorHandlerMock = new AnimatorHandlerMock();
            inputHandlerMock = new InputHandlerMock();
            playerManagerMock = new PlayerManagerMock();
            playerInventoryMock = new PlayerInventoryMock();
            weaponSlotManagerMock = new WeaponSlotManagerMock();

            playerAttacker.animatorHandler = animatorHandlerMock;
            playerAttacker.inputHandler = inputHandlerMock;
            playerAttacker.playerManager = playerManagerMock;
            playerAttacker.playerInventory = playerInventoryMock;
            playerAttacker.weaponSlotManager = weaponSlotManagerMock;
            
            playerAttacker.Awake();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerGameObject);
        }

        [Test]
        public void HandleWeaponCombo_ComboFlagSet_PlayAttack2Animation()
        {
            inputHandlerMock.comboFlag = true;
            playerInventoryMock.rightWeapon = new WeaponItem("TestWeapon", "Attack1", "Attack2", true, false, false);

            playerAttacker.HandleWeaponCombo(playerInventoryMock.rightWeapon);

            Assert.IsFalse(inputHandlerMock.comboFlag);
            Assert.AreEqual("Attack2", animatorHandlerMock.playedAnimation);
        }

        [Test]
        public void HandleLightAttack_PlayAttack1Animation_LastAttackSet()
        {
            playerInventoryMock.rightWeapon = new WeaponItem("TestWeapon", "Attack1", "Attack2", true, false, false);

            playerAttacker.HandleLightAttack(playerInventoryMock.rightWeapon);

            Assert.AreEqual("Attack1", animatorHandlerMock.playedAnimation);
            Assert.AreEqual("Attack1", playerAttacker.lastAttack);
        }

        [Test]
        public void HandleHeavyAttack_PlayAttack2Animation_LastAttackSet()
        {
            playerInventoryMock.rightWeapon = new WeaponItem("TestWeapon", "Attack1", "Attack2", true, false, false);

            playerAttacker.HandleHeavyAttack(playerInventoryMock.rightWeapon);

            Assert.AreEqual("Attack2", animatorHandlerMock.playedAnimation);
            Assert.AreEqual("Attack2", playerAttacker.lastAttack);
        }

        public class AnimatorHandlerMock : AnimatorHandler
        {
            public string playedAnimation;

            public override void PlayTargetAnimation(string targetAnim, bool isInteracting)
            {
                playedAnimation = targetAnim;
            }
        }

        public class InputHandlerMock : InputHandler
        {
            public bool comboFlag;
        }


        public class PlayerInventoryMock : PlayerInventory
        {
            public WeaponItem rightWeapon;
        }

    }
}
