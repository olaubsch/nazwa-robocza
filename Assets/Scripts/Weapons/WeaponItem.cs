using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLMJ
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Attack Animations")]
        public string Attack1;
        public string Attack2;

        [Header("Weapon Type")]
        public bool isSpellCaster;
        public bool isFaithCaster;
        public bool isPyroCaster;
        public bool isMeleeWeapon;

    }
}