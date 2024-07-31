using UnityEngine;

/* 
 * This class handles the player's movement, including walking, sprinting, crouching, and jumping.
 * It interacts with the CharacterController component to move the player character.
 */
[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    // Reference to the CharacterController component
    private CharacterController characterController;

    // Stores the player's current velocity
    private Vector3 playerVelocity;

    // State variables
    private bool isGrounded;
    private bool lerpCrouch;
    private bool sprinting;
    private bool crouching;
    private float crouchTimer = 0f;
    private float motorSpeed = 5f;

    /* 
     * Serialized fields for configuring gravity, jump height, crouch height, standing height, 
     * crouch duration, sprint speed, walk speed, and crouch speed.
     */
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchDuration = 1f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2f;

    /*
     * Start is called before the first frame update.
     * It initializes the CharacterController component.
     */
    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
    }

    /*
     * Update is called once per frame.
     * It handles the player's ground status, crouching, gravity application, and movement.
     */
    void Update()
    {
        HandleGroundStatus();
        HandleCrouch();
        ApplyGravity();
        MovePlayer();
    }

    /*
     * Handles the player's ground status.
     * If the player is grounded and their vertical velocity is negative, 
     * it resets the vertical velocity to a small negative value to keep them grounded.
     */
    private void HandleGroundStatus()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
    }

    /*
     * Handles the player's crouching state.
     * 
     * If lerpCrouch is true, it interpolates the CharacterController's height between 
     * the standing and crouching heights based on crouchTimer and crouchDuration.
     * 
     * Explanation of Lerp:
     * 
     * - Mathf.Lerp(start, end, t) performs linear interpolation between the start and end 
     *   values based on the t parameter, which is a value between 0 and 1.
     *   
     * - When t is 0, the result is start; when t is 1, the result is end. Values between 0 
     *   and 1 result in a value proportionally between start and end.
     *   
     * - In this code, the characterController.height is interpolated between the current 
     *   height and the target height (crouchHeight or standingHeight) based on the progress.
     *   
     * - progress is calculated as the square of (crouchTimer / crouchDuration) to create a 
     *   smoothing effect, making the transition more gradual.
    */
    private void HandleCrouch()
    {
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float progress = crouchTimer / crouchDuration;
            progress *= progress;

            // Interpolate the CharacterController's height
            characterController.height = Mathf.Lerp(characterController.height, crouching ? crouchHeight : standingHeight, progress);

            if (progress > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    /*
    
     */

    /*
     * Applies gravity to the player's vertical velocity if they are not grounded.
     * 
     * Explanation of Gravity:
     * - Gravity is a constant force that pulls objects towards the center of the Earth. 
     *   In this code, it is represented by a negative value to simulate this downward force.
     *   
     * - playerVelocity.y represents the player's vertical velocity. When the player is 
     *   not grounded, gravity is applied to this velocity.
     *   
     * - Each frame, gravity * Time.deltaTime is added to playerVelocity.y. Time.deltaTime ensures 
     *   that the gravity effect is frame rate independent.
     *   
     * - Over time, this causes the player to accelerate downwards, simulating the effect of gravity.
     */

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
    }

    /*
     * Moves the player based on their current velocity.
     * This method is called in Update to ensure consistent movement.
     */
    private void MovePlayer()
    {
        if (playerVelocity != Vector3.zero)
        {
            characterController.Move(playerVelocity * Time.deltaTime);
        }
    }

    /*
     * Processes the player's movement based on input.
     * Converts the input vector to a movement direction and moves the player accordingly.
     * 
     * Parameters:
     *   input (Vector2): The input vector containing x and y values for movement.
     */
    public void ProcessMovement(Vector2 input)
    {
        Vector3 movementDirection = new Vector3(input.x, 0, input.y);
        if (movementDirection != Vector3.zero)
        {
            characterController.Move(transform.TransformDirection(movementDirection) * motorSpeed * Time.deltaTime);
        }
    }

    /*
     * Makes the player jump by setting their vertical velocity based on jump height and gravity.
     * Only allows jumping if the player is grounded.
     */
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    /*
     * Toggles the player's crouching state and starts the crouch interpolation.
     * Adjusts the player's movement speed based on their crouching state.
     */
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0f;
        lerpCrouch = true;
        motorSpeed = crouching ? crouchSpeed : walkSpeed;
    }

    /*
     * Starts sprinting by setting the player's movement speed to sprint speed.
     */
    public void StartSprint()
    {
        sprinting = true;
        motorSpeed = sprintSpeed;
    }

    /*
     * Stops sprinting by setting the player's movement speed back to walk speed.
     */
    public void StopSprint()
    {
        sprinting = false;
        motorSpeed = walkSpeed;
    }
}
