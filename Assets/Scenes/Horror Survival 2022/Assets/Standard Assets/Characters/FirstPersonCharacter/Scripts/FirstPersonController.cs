using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonController : MonoBehaviour
{
    public static bool InventorySwitchOn=false;
    private bool isCrouching;
    private bool duringCrouchAnim;
    public bool canmove { get; private set; } = true;

    private bool isSprinting => canSprint && Input.GetKey(sprintKey);
    private bool shouldJump => Input.GetKey(jumpKey) && characterController.isGrounded;
    private bool shouldCrouch => Input.GetKeyDown(crouchkey) && !duringCrouchAnim && characterController.isGrounded;

    [Header("Crouch Control")]
    [SerializeField] private float crouchHeight = 0.7f;
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchCenter = new Vector3(0, 0.7f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canHeadBob = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool useFootSteps = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchkey = KeyCode.LeftControl;
    [SerializeField] private KeyCode Interact = KeyCode.Mouse0;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 5.0f;
    [SerializeField] private float crouchSpeed = 1.0f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Head Bob Parameters")]
    [SerializeField] private float walkBobSpeed = 10.0f;
    [SerializeField] private float walkBobSensi = 0.1f;
    [SerializeField] private float sprintBobSpeed = 18.0f;
    [SerializeField] private float sprintBobSensi = 0.3f;
    [SerializeField] private float crouchBobSpeed = 5.0f;
    [SerializeField] private float crouchBobSensi = 0.05f;

    private float defaultYPos = 0f;
    private float timer;

    [Header("Health regen")]
    [SerializeField] private float maxHelth = 100;
    [SerializeField] private float timeBeforeRegen = 3.0f;
    [SerializeField] private float healthValueInc = 1.0f;
    [SerializeField] private float helthTimeInc = 0.1f;
    private float currentHealth;
    private Coroutine regenratingHealth;
    public static Action<float> onTakeDamage;
    public static Action<float> onDamage;
    public static Action<float> onHeal;


    [Header("Stamina")]
    public static float FpsStamina = 100;
   

    private float RunSpeedAmt;





    [Header("interaction ")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;
    [Header("footsteps")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footStepAudioSource = default;
    [SerializeField] private AudioClip[] grass = default;
    [SerializeField] private AudioClip[] sticky = default;
    [SerializeField] private AudioClip[] concrete = default;
    private float footstepTimer;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;




    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector2 currentInput;
    private float xRot = 0;


    private void Start()
    {
        RunSpeedAmt = sprintSpeed;
        SaveScript.health = 100;
    }
    private void OnEnable()
    {
        onTakeDamage += ApplyDamage;

    }

    private void OnDisable()
    {
        onTakeDamage -= ApplyDamage;
    }

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultYPos = playerCamera.transform.localPosition.y;
        currentHealth = maxHelth;
    }

    void Update()
    {
       
        if (SaveScript.isMenuActive == false)
        {




            if (Input.GetKeyDown(KeyCode.I))
            {
                if (InventorySwitchOn == false)
                {
                    InventorySwitchOn = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {

                    InventorySwitchOn = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            if (FpsStamina < 20)
            {
                sprintSpeed = walkSpeed;

            }
            else
            {
                sprintSpeed = RunSpeedAmt;
            }

            if (InventorySwitchOn == false && canmove)
            {
                HandleMovementInput();
                HandleMouseLook();
                if (canJump) handleJump();
                if (canCrouch) handleCrouch();
                if (canHeadBob) handleHeadBob();
                if (canInteract)
                {
                    handleInteractionCheck();
                    handleInteractionInput();
                }
                if (useFootSteps)
                {
                    handleFootSteps();
                }

                ApplyFinalMovement();
            }
        }
    }

    private void HandleMovementInput()
    {
        if (InventorySwitchOn) return;

        currentInput = new Vector2((isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"),
                                   (isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        if (InventorySwitchOn) return;

        xRot -= Input.GetAxis("Mouse Y") * lookSpeedY;
        xRot = Mathf.Clamp(xRot, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void handleJump()
    {
        if (shouldJump)
        {
            moveDirection.y = jumpForce;
        }
    }

    private void handleCrouch()
    {
        if (shouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
        {
            yield break;
        }

        duringCrouchAnim = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;
        duringCrouchAnim = false;
    }

    private void handleHeadBob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobSensi : isSprinting ? sprintBobSensi : walkBobSensi),
                playerCamera.transform.localPosition.z);
        }
    }


    private void handleInteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == 9 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()
                ))
            {
                hit.collider.TryGetComponent(out currentInteractable);
                if (currentInteractable)
                    currentInteractable.onFocus();
            }


            else if (currentInteractable)
            {
                currentInteractable.onLoseFocus();
                currentInteractable = null;
            }
        }
    }

    private void handleInteractionInput()
    {
        if (Input.GetKeyDown(Interact) && currentInteractable != null &&
            Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer
            ))
        {
            currentInteractable.onIntract();
        }
    }

    private void handleFootSteps()
    {
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 5))
            {
              //  Debug.Log("Raycast hit: " + hit.collider.tag); // Debugging statement

                switch (hit.collider.tag)
                {
                    case "Footstep/grass":
                        if (grass.Length > 0)
                        {
                            footStepAudioSource.PlayOneShot(grass[UnityEngine.Random.Range(0, grass.Length)]);
                            Debug.Log("Playing grass footstep sound."); // Debugging statement
                        }
                        else
                        {
                            Debug.LogWarning("No grass footstep sounds assigned."); // Debugging statement
                        }
                        break;
                    default:
                        Debug.LogWarning("No matching tag for footstep sound."); // Debugging statement
                        break;
                }
            }
            else
            {
                Debug.LogWarning("Raycast did not hit any collider."); // Debugging statement
            }

            footstepTimer = GetCurrentOffset;
        }
    }


    private void ApplyDamage(float damage)
    {
        currentHealth -= damage;

        onDamage?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            KillPlayer();
        }
        else if (regenratingHealth != null)
        {
            StopCoroutine(regenratingHealth);
        }
        regenratingHealth = StartCoroutine(regenrateHealth());
    }
    private void KillPlayer()
    {
        currentHealth = 0;
        if (regenratingHealth != null)
        {
            StopCoroutine(regenratingHealth);
        }

    }

    private IEnumerator regenrateHealth()
    {
        yield return new WaitForSeconds(timeBeforeRegen);
        WaitForSeconds timetoWait = new WaitForSeconds(healthValueInc);



        while (currentHealth < maxHelth)
        {
            currentHealth += healthValueInc;

            if ((currentHealth > maxHelth))
            {
                currentHealth = maxHelth;
            }
            onHeal?.Invoke(currentHealth);
            yield return timetoWait;
        }
        regenratingHealth = null;
    }

    private void ApplyFinalMovement()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }
}
