using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 playerVelocity;

    private bool isGrounded;
    private bool lerpCrouch;
    private bool sprinting;
    private bool crouching;
    private float crouchTimer = 0f;
    private float motorSpeed = 5f;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchDuration = 1f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleGroundStatus();
        HandleCrouch();
        ApplyGravity();
        MovePlayer();
    }

    private void HandleGroundStatus()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
    }

    private void HandleCrouch()
    {
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float progress = crouchTimer / crouchDuration;
            progress *= progress;

            characterController.height = Mathf.Lerp(characterController.height, crouching ? crouchHeight : standingHeight, progress);

            if (progress > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
    }

    private void MovePlayer()
    {
        if (playerVelocity != Vector3.zero)
        {
            characterController.Move(playerVelocity * Time.deltaTime);
        }
    }

    public void ProcessMovement(Vector2 input)
    {
        Vector3 movementDirection = new Vector3(input.x, 0, input.y);
        if (movementDirection != Vector3.zero)
        {
            characterController.Move(transform.TransformDirection(movementDirection) * motorSpeed * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0f;
        lerpCrouch = true;
        motorSpeed = crouching ? crouchSpeed : walkSpeed;
    }

    public void StartSprint()
    {
        sprinting = true;
        motorSpeed = sprintSpeed;
    }

    public void StopSprint()
    {
        sprinting = false;
        motorSpeed = walkSpeed;
    }
}
