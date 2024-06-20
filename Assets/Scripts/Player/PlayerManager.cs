using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLMJ
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;

        public bool isInteracting;
        private bool isPaused = false;
        
    

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool isJumping;
        public bool canDoCombo;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("isInteracting");
            isInAir = !isGrounded;
            anim.SetBool("isInAir", isInAir);

            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
            canDoCombo = anim.GetBool("canDoCombo");

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

        public void SetPauseState(bool paused)
    {
        isPaused = paused;

        // Zablokuj lub odblokuj ruch kamery
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactbleText;
                        //SET THE UI TEXT TO THE INTERACTABLE OBJECT'S TEXT
                        //SET THE TEXT POP UP TO TRUE

                        if(inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
        }


    }
}