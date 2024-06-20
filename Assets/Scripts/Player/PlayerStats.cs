using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OLMJ
{
    public class PlayerStats : CharacterStats
    {
        public HealthBar healthbar;
        public StaminaBar staminaBar;

        AnimatorHandler animatorHandler;
        PlayerLocomotion playerLocomotion; // Dodane pole

        public float regenRate = 2f; // Adjust as needed
        private float regenTimer;
        public int sprintStaminaCost = 10; // Adjust as needed
        public int jumpStaminaCost = 5; // Adjust as needed

        private bool isSprinting = false;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>(); // Inicjalizacja playerLocomotion
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);

            StartCoroutine(RegenerateStamina());
        }

        public IEnumerator RegenerateStamina()
{
    while (currentStamina < maxStamina)
    {
        currentStamina += (int)(regenRate / 5f);
        if(currentStamina > maxStamina) currentStamina = maxStamina;
        staminaBar.SetCurrentStamina((int)currentStamina);
        yield return new WaitForSeconds(0.5f); // Zmiana tempo odnawiania staminy na co 0.2 sekundy
    }
}


        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0);
            healthbar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("TakeDamage01", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death01_A", true);
                //HANDLE PLAYER DEATH
                SceneManager.LoadScene("lobby");
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
            }
        }

        public int GetCurrentStamina()
        {
            return (int)currentStamina;
        }

        public void SubtractStamina(int amount)
        {
            currentStamina -= amount;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            staminaBar.SetCurrentStamina((int)currentStamina);
        }
    }
}
