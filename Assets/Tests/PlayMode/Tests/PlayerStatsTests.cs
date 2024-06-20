using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class PlayerStatsTests
    {
        private GameObject playerGameObject;
        private PlayerStats playerStats;
        private AnimatorHandler animatorHandler;
        private HealthBar healthBar;
        private StaminaBar staminaBar;
        private PlayerLocomotion playerLocomotion;

        [SetUp]
        public void Setup()
        {
            playerGameObject = new GameObject();
            playerStats = playerGameObject.AddComponent<PlayerStats>();

            animatorHandler = playerGameObject.AddComponent<AnimatorHandler>();
            healthBar = playerGameObject.AddComponent<HealthBar>();
            staminaBar = playerGameObject.AddComponent<StaminaBar>();
            playerLocomotion = playerGameObject.AddComponent<PlayerLocomotion>();

            playerStats.animatorHandler = animatorHandler;
            playerStats.healthbar = healthBar;
            playerStats.staminaBar = staminaBar;
            playerStats.playerLocomotion = playerLocomotion;

            playerStats.Awake();
            playerStats.Start();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerGameObject);
        }

        [UnityTest]
        public IEnumerator RegenerateStamina_WhileCurrentStaminaBelowMax_RegeneratesStamina()
        {
            playerStats.currentStamina = 0;
            playerStats.maxStamina = 100;

            yield return null; 

            yield return new WaitForSeconds(1f);

            Assert.Greater(playerStats.currentStamina, 0);
            Assert.LessOrEqual(playerStats.currentStamina, playerStats.maxStamina);
        }

        [Test]
        public void SetMaxHealthFromHealthLevel_HealthLevelSet_MaxHealthSet()
        {
            playerStats.healthLevel = 5;

            int maxHealth = playerStats.SetMaxHealthFromHealthLevel();

            Assert.AreEqual(50, maxHealth);
            Assert.AreEqual(50, playerStats.maxHealth);
        }

        [Test]
        public void SetMaxStaminaFromStaminaLevel_StaminaLevelSet_MaxStaminaSet()
        {
            playerStats.staminaLevel = 3;

            int maxStamina = playerStats.SetMaxStaminaFromStaminaLevel();

            Assert.AreEqual(30, maxStamina);
            Assert.AreEqual(30, playerStats.maxStamina);
        }

        [Test]
        public void TakeDamage_SubtractsHealthAndSetsCurrentHealth()
        {
            playerStats.currentHealth = 100;

            playerStats.TakeDamage(20);

            Assert.AreEqual(80, playerStats.currentHealth);
            Assert.AreEqual(80, healthBar.currentHealth);
        }

        [Test]
        public void TakeDamage_HealthBelowZero_PlaysDeathAnimationAndLoadsScene()
        {
            playerStats.currentHealth = 10;

            playerStats.TakeDamage(20);

            Assert.AreEqual(0, playerStats.currentHealth);
            Assert.AreEqual("Death01_A", animatorHandler.playedAnimation);
            Assert.IsTrue(TestUtils.IsSceneLoaded("lobby"));
        }

        [Test]
        public void GetCurrentStamina_ReturnsCurrentStamina()
        {
            playerStats.currentStamina = 50;

            int currentStamina = playerStats.GetCurrentStamina();

            Assert.AreEqual(50, currentStamina);
        }

        [Test]
        public void SubtractStamina_SubtractsStaminaAndSetsCurrentStamina()
        {
            playerStats.currentStamina = 50;

            playerStats.SubtractStamina(10);

            Assert.AreEqual(40, playerStats.currentStamina);
            Assert.AreEqual(40, staminaBar.currentStamina);
        }

        public class AnimatorHandler : MonoBehaviour
        {
            public string playedAnimation;

            public void PlayTargetAnimation(string targetAnim, bool isInteracting)
            {
                playedAnimation = targetAnim;
            }
        }
        public class HealthBar : MonoBehaviour
        {
            public int currentHealth;


            public void SetCurrentHealth(int currentHealth)
            {
                this.currentHealth = currentHealth;
            }
        }

        public class StaminaBar : MonoBehaviour
        {
            public int currentStamina;

            public void SetCurrentStamina(int currentStamina)
            {
                this.currentStamina = currentStamina;
            }
        }

        public static class TestUtils
        {
            public static bool IsSceneLoaded(string sceneName)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                return currentScene.name.Equals(sceneName);
            }
        }
    }
}
