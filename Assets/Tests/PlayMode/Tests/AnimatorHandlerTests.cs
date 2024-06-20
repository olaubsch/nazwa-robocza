using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace OLMJ
{
    public class AnimatorHandlerTests
    {
        private GameObject playerGameObject;
        private AnimatorHandler animatorHandler;
        private Animator animator;

        [SetUp]
        public void Setup()
        {
            playerGameObject = new GameObject();
            animatorHandler = playerGameObject.AddComponent<AnimatorHandler>();
            animator = playerGameObject.AddComponent<Animator>();

            animatorHandler.Initialize(); 
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerGameObject);
        }

        [Test]
        public void UpdateAnimatorValues_CorrectVerticalAndHorizontal()
        {

            float verticalMovement = 0.75f;
            float horizontalMovement = -0.25f;
            bool isSprinting = false;

            animatorHandler.UpdateAnimatorValues(verticalMovement, horizontalMovement, isSprinting);

            Assert.AreEqual(1f, animator.GetFloat("Vertical"));
            Assert.AreEqual(-0.5f, animator.GetFloat("Horizontal"));
        }

        [UnityTest]
        public IEnumerator PlayTargetAnimation_CorrectAnimation()
        {

            string targetAnim = "Jump";
            bool isInteracting = true;

            animatorHandler.PlayTargetAnimation(targetAnim, isInteracting);

            yield return new WaitForSeconds(0.5f);

            Assert.AreEqual(targetAnim, animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }

        [Test]
        public void CanRotate_SetToTrue()
        {

            animatorHandler.CanRotate();

            Assert.IsTrue(animatorHandler.canRotate);
        }

        [Test]
        public void StopRotation_SetToFalse()
        {

            animatorHandler.StopRotation();

            Assert.IsFalse(animatorHandler.canRotate);
        }

        [UnityTest]
        public IEnumerator OnJumpAnimationEnd_SetJumpingToFalse()
        {

            animatorHandler.OnJumpAnimationEnd();

            yield return null;

            PlayerManager playerManager = playerGameObject.GetComponentInParent<PlayerManager>();
            Assert.IsFalse(playerManager.isJumping);
        }

        [Test]
        public void EnableCombo_SetCanDoComboToTrue()
        {

            animatorHandler.EnableCombo();

            Assert.IsTrue(animator.GetBool("canDoCombo"));
        }

        [Test]
        public void DisableCombo_SetCanDoComboToFalse()
        {

            animatorHandler.DisableCombo();

            Assert.IsFalse(animator.GetBool("canDoCombo"));
        }
    }
}
