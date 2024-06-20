using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace OLMJ
{
    public class PlayerManagerTests
    {
        private GameObject playerGameObject;
        private PlayerManager playerManager;
        private InputHandler inputHandler;

        [SetUp]
        public void Setup()
        {
            playerGameObject = new GameObject();
            playerManager = playerGameObject.AddComponent<PlayerManager>();
            inputHandler = playerGameObject.AddComponent<InputHandler>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerGameObject);
        }

        [UnityTest]
        public IEnumerator PlayerStartsNotInteracting()
        {
            yield return null; 

            Assert.IsFalse(playerManager.isInteracting);
        }

        [UnityTest]
        public IEnumerator PlayerMovesForwardOnInput()
        {
            inputHandler.vertical = 1.0f;
            playerManager.Update(); 
            yield return new WaitForSeconds(1.0f); 
            Assert.IsFalse(playerManager.isGrounded);
        }

        [UnityTest]
        public IEnumerator PlayerJumpsOnInput()
        {
            inputHandler.jump_Input = true; 
            playerManager.Update(); 

            yield return new WaitForSeconds(1.0f); 

            Assert.IsTrue(playerManager.isJumping);
        }

        [UnityTest]
        public IEnumerator PlayerPausesGameCorrectly()
        {
            playerManager.SetPauseState(true); 

            yield return null; 

            Assert.IsTrue(Cursor.visible);
            Assert.AreEqual(CursorLockMode.None, Cursor.lockState);
        }

        [UnityTest]
        public IEnumerator PlayerInteractsWithObject()
        {
            GameObject interactableObject = new GameObject();
            interactableObject.tag = "Interactable";
            interactableObject.AddComponent<BoxCollider>(); 

            inputHandler.a_Input = true; 
            playerManager.CheckForInteractableObject(); 

            yield return null;

            Assert.IsTrue(interactableObject.GetComponent<Interactable>().WasInteracted);

            Object.Destroy(interactableObject);
        }
    }
}
