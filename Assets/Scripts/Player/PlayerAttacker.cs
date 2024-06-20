using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLMJ {
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        PlayerManager playerManager;
        PlayerInventory playerInventory;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;


        public void Awake()
        {
            animatorHandler = GetComponent<AnimatorHandler>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();

        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                if (lastAttack == weapon.Attack1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.Attack2, true);
                }
            }

        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.Attack1, true);
            lastAttack = weapon.Attack1;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.Attack2, true);
            lastAttack = weapon.Attack2;
        }

        #region Input Actions
        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
        }
        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;

                if (playerManager.canDoCombo)
                    return;

                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (weapon.isFaithCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    //CHECK FOR FP
                    //ATTEMPT TO CAST SPELL
                }
            }
        }
        #endregion
    }
}
