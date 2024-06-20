using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLMJ
{
    public class PlayerLocomotion : MonoBehaviour
    {
        PlayerManager playerManager;
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDirection;
        PlayerStats playerStats;

        public LayerMask obstacleLayer; // Warstwa przeszkód
        public float obstacleCheckDistance = 1.5f; // Dystans sprawdzania przeszkód
        public float jumpForce = 7f;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        private Coroutine recharge;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 45;

        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            playerStats = GetComponent<PlayerStats>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = myTransform.forward;

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
                return;

            if (playerManager.isInteracting)
                return;

            HandleFalling(delta, moveDirection);

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5 && playerStats.GetCurrentStamina() > 0)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerStats.SubtractStamina(1); // Odejmujemy staminę podczas sprintu
            }
            else
            {
                if (playerManager.isSprinting)
                {
                    EndActionWithDelay(1f); // Uruchomienie EndAction z opóźnieniem 2 sekund
                }

                if (inputHandler.moveAmount < 0.5)
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animatorHandler.anim.GetBool("isInteracting"))
                return;

            if (inputHandler.rollFlag && playerStats.GetCurrentStamina() >= playerStats.sprintStaminaCost)
            {
                playerStats.SubtractStamina(playerStats.sprintStaminaCost);
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                    EndActionWithDelay(1f); // Uruchomienie EndAction z opóźnieniem 1 sekundy po rollu
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Backstep", true);
                    EndActionWithDelay(1f); // Uruchomienie EndAction z opóźnieniem 1 sekundy po backstepie
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for " + inAirTimer);
                        animatorHandler.PlayTargetAnimation("Locomotion", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Locomotion", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if (playerManager.isGrounded)
            {
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }
        }

        public void HandleJumping()
    {
        if (playerManager.isInteracting || playerManager.isJumping)
        {
            Debug.Log("Cannot jump: either interacting or already jumping.");
            return;
        }

        if (inputHandler.jump_Input && playerStats.GetCurrentStamina() >= playerStats.jumpStaminaCost)
        {
            playerStats.SubtractStamina(playerStats.jumpStaminaCost);
            Vector3 moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.y = 0;

            Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
            myTransform.rotation = jumpRotation;

            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animatorHandler.PlayTargetAnimation("Jump", true);
            playerManager.isJumping = true; // Ustawienie flagi isJumping
            EndActionWithDelay(1f); // Uruchomienie EndAction z opóźnieniem 1 sekundy po skoku

            Debug.Log("Jumping.");

            if (CheckForObstacles())
            {
                Debug.Log("Obstacle detected during jump.");
                // Logika do przeskakiwania przeszkody, jeśli jest to potrzebne
            }
        }
        else
        {
            Debug.Log("Jump input not detected or not enough stamina.");
        }
    }

        void OnCollisionStay(Collision collision)
{
    if (collision.collider.CompareTag("Stairs"))
    {
        rigidbody.AddForce(Vector3.up * movementSpeed, ForceMode.Impulse);
    }
}


        public void EndAction()
        {
            if (recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(playerStats.RegenerateStamina());
        }

        private void EndActionWithDelay(float delay)
        {
            if (recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(DelayedEndAction(delay));

            playerManager.isJumping = false;
        }

        private IEnumerator DelayedEndAction(float delay)
        {
            yield return new WaitForSeconds(delay);
            EndAction();
        }

        private bool CheckForObstacles()
{
    RaycastHit hit;
    Vector3 forward = myTransform.TransformDirection(Vector3.forward);

    if (Physics.Raycast(myTransform.position, forward, out hit, obstacleCheckDistance, obstacleLayer))
    {
        Debug.DrawRay(myTransform.position, forward * obstacleCheckDistance, Color.red); 
        Debug.Log("Obstacle detected: " + hit.collider.name);
        return true;
    }
    Debug.DrawRay(myTransform.position, forward * obstacleCheckDistance, Color.green); 
    return false;
}

        #endregion
    }
}
