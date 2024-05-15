using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BrokenVector.LowPolyFencePack;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 90f; // Max vertical rotation angle

    public float interactionDistance = 2f; // Distance for interaction
    public TMP_Text interactionText; // UI text for interaction

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    public bool canMove = true;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Hide interaction text initially
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleRotation();
        HandleInteraction();
    }

    // Handle player movement
    private void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.y = movementDirectionY;
    }

    // Handle player jumping
    private void HandleJumping()
    {
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime; // Apply gravity when not grounded
        }
    }

    // Handle player rotation
    private void HandleRotation()
    {
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    // Handle player interaction with objects
    private void HandleInteraction()
    {
        bool showText = false;
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                DoorToggle doorToggle = hit.transform.GetComponent<DoorToggle>();
                if (doorToggle != null)
                {
                    doorToggle.SendMessage("OnMouseDown");
                }
            }
        }
        else
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                DoorToggle doorToggle = hit.transform.GetComponent<DoorToggle>();
                if (doorToggle != null)
                {
                    showText = true;
                }
            }
        }

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(showText);
        }
    }
}
