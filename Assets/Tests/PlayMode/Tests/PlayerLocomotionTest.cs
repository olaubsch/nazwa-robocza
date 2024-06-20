using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace OLMJ
{
    public class PlayerLocomotionTests
    {
        private GameObject playerGameObject;
        private PlayerLocomotion playerLocomotion;

        [SetUp]
        public void Setup()
        {
            playerGameObject = new GameObject();
            playerLocomotion = playerGameObject.AddComponent<PlayerLocomotion>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerGameObject);
        }

        [UnityTest]
        public IEnumerator PlayerMovesForward()
        {
            playerLocomotion.HandleMovement(1.0f); 
            Vector3 originalPosition = playerLocomotion.transform.position;
            yield return new WaitForSeconds(1.0f); 

            Assert.Greater(playerLocomotion.transform.position.z, originalPosition.z);
        }

        [UnityTest]
        public IEnumerator PlayerMovesRight()
        {
            playerLocomotion.HandleMovement(1.0f);
            Vector3 originalPosition = playerLocomotion.transform.position;
            yield return new WaitForSeconds(1.0f);

            Assert.Greater(playerLocomotion.transform.position.x, originalPosition.x);
        }

        [UnityTest]
        public IEnumerator PlayerJumps()
        {
            // Test skoku
            playerLocomotion.HandleJumping();
            yield return new WaitForSeconds(1.0f); 

            Assert.IsTrue(playerLocomotion.playerManager.isJumping);
        }

        [UnityTest]
        public IEnumerator PlayerCannotJumpWithoutEnoughStamina()
        {
            playerLocomotion.playerStats.SetCurrentStamina(0); 

            // Spróbuj wykonać skok
            playerLocomotion.HandleJumping();
            yield return new WaitForSeconds(1.0f); 

            Assert.IsFalse(playerLocomotion.playerManager.isJumping);
        }

        [UnityTest]
        public IEnumerator PlayerDetectsObstacleDuringJump()
        {
            GameObject obstacle = new GameObject();
            obstacle.layer = LayerMask.NameToLayer("Obstacle");
            obstacle.transform.position = playerLocomotion.transform.position + playerLocomotion.transform.forward * 2.0f; 
            playerLocomotion.HandleJumping();
            yield return new WaitForSeconds(1.0f); 

            Assert.IsTrue(playerLocomotion.CheckForObstacles());

            Object.Destroy(obstacle);
        }

        [UnityTest]
        public IEnumerator PlayerStaminaRegeneratesOverTime()
        {
            playerLocomotion.playerStats.SetCurrentStamina(10); 

            playerLocomotion.EndAction();

            yield return new WaitForSeconds(5.0f);

            Assert.Greater(playerLocomotion.playerStats.GetCurrentStamina(), 10); 

            Assert.LessOrEqual(playerLocomotion.playerStats.GetCurrentStamina(), playerLocomotion.playerStats.maxStamina); 
        }
    }
}
