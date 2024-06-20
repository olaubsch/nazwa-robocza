using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Tests
{
    public class PlayerInventoryTests
    {
        private GameObject playerGameObject;
        private PlayerInventory playerInventory;
        private MockWeaponSlotManager mockWeaponSlotManager;

        [SetUp]
        public void Setup()
        {
            playerGameObject = new GameObject();
            playerInventory = playerGameObject.AddComponent<PlayerInventory>();
            mockWeaponSlotManager = playerGameObject.AddComponent<MockWeaponSlotManager>();

            playerInventory.weaponSlotManager = mockWeaponSlotManager;

            playerInventory.weaponsInRightHandSlots = new WeaponItem[2];
            playerInventory.weaponsInRightHandSlots[0] = CreateWeaponItem("Sword", true);
            playerInventory.weaponsInRightHandSlots[1] = CreateWeaponItem("Axe", true);

            playerInventory.weaponsInLeftHandSlots = new WeaponItem[2];
            playerInventory.weaponsInLeftHandSlots[0] = CreateWeaponItem("Shield", false);
            playerInventory.weaponsInLeftHandSlots[1] = null;

            playerInventory.unarmedWeapon = CreateWeaponItem("Fist", true);

            playerInventory.Awake();
            playerInventory.Start();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerGameObject);
        }

        [Test]
        public void ChangeRightWeapon_SwitchesToNextRightWeapon()
        {
            playerInventory.ChangeRightWeapon();
            Assert.AreEqual("Axe", playerInventory.rightWeapon.itemName);
        }

        [Test]
        public void ChangeRightWeapon_SwitchesToUnarmedWhenNoMoreWeapons()
        {
            playerInventory.ChangeRightWeapon();
            playerInventory.ChangeRightWeapon();
            playerInventory.ChangeRightWeapon();
            Assert.AreEqual("Fist", playerInventory.rightWeapon.itemName);
        }

        [Test]
        public void ChangeLeftWeapon_SwitchesToNextLeftWeapon()
        {
            playerInventory.ChangeLeftWeapon();
            Assert.AreEqual("Shield", playerInventory.leftWeapon.itemName);
        }

        [Test]
        public void ChangeLeftWeapon_SwitchesToUnarmedWhenNoMoreWeapons()
        {
            playerInventory.ChangeLeftWeapon();
            playerInventory.ChangeLeftWeapon();
            playerInventory.ChangeLeftWeapon();
            Assert.AreEqual("Fist", playerInventory.leftWeapon.itemName);
        }

        private WeaponItem CreateWeaponItem(string name, bool isMelee)
        {
            WeaponItem weapon = new WeaponItem();
            weapon.itemName = name;
            weapon.isMeleeWeapon = isMelee;
            return weapon;
        }

    }
}
