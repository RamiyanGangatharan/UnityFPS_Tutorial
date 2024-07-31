using UnityEngine;

/* 
 * This class manages player input and ensures the proper handling of
 * player movement and look actions by interacting with PlayerMotor and 
 * PlayerLook components. It requires PlayerMotor and PlayerLook components 
 * to function correctly.
 */
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerLook))]

public class InputManager : MonoBehaviour
{
    /* 
     * Private member variables to store player input actions, movement 
     * & look inputs, and references to the PlayerMotor & PlayerLook components.
     */

    // Handles input actions
    private PlayerInput playerInput;

    // Specific input actions for on-foot movement
    private PlayerInput.OnFootActions onFoot;

    // Reference to the PlayerMotor component
    private PlayerMotor motor;

    // Reference to the PlayerLook component
    private PlayerLook look;

    // Stores movement input values
    private Vector2 movementInput;

    // Stores look input values
    private Vector2 lookInput;

    /* 
     * Awake is called when the script instance is being loaded.
     * It initializes player input, retrieves references to required components,
     * and sets up input action event handlers.
     */
    void Awake()
    {
        // Initialize player input
        playerInput = new PlayerInput();

        // Get on-foot actions
        onFoot = playerInput.OnFoot;

        // Get PlayerMotor component
        motor = GetComponent<PlayerMotor>();

        // Get PlayerLook component
        look = GetComponent<PlayerLook>();

        /* 
         * Check for missing components and log an error if any required component is missing.
         * Disable this script if components are missing to prevent errors.
         */
        if (motor == null || look == null)
        {
            Debug.LogError("Missing required components: PlayerMotor or PlayerLook");

            // Disable this script
            enabled = false;

            // Exit the function
            return;
        }

        /* 
         * Subscribe to input action events for Jump, Crouch, Sprint, Movement, and Look.
         * These events are triggered based on player input and call the appropriate methods 
         * in PlayerMotor and PlayerLook components.
         * 
         * Explanation:
         * Each input action event uses a lambda expression to specify the method to call or 
         * the operation to perform when the event is triggered. The 'ctx' parameter is of type 
         * 'InputAction.CallbackContext' and provides context about the input event, such as the 
         * current value of the input. The lambda expression (ctx => ...) is a concise way to define
         * an anonymous function that takes 'ctx' as an input parameter and executes the specified 
         * method or operation. For example, 'onFoot.Jump.performed += ctx => motor.Jump();' means 
         * that when the Jump action is performed, the motor's Jump method is called. Similarly, 
         * 'onFoot.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();' means 
         * that when the Movement action is performed, the movementInput is updated with the current 
         * input value.
         */

        // Subscribe Jump action to motor's Jump method
        onFoot.Jump.performed += ctx => motor.Jump();

        // Subscribe Crouch action to motor's Crouch method
        onFoot.Crouch.performed += ctx => motor.Crouch();

        // Subscribe Sprint action to motor's StartSprint method
        onFoot.Sprint.performed += ctx => motor.StartSprint();

        // Subscribe Sprint cancel action to motor's StopSprint method
        onFoot.Sprint.canceled += ctx => motor.StopSprint();

        // Update movement input on performed
        onFoot.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();

        // Reset movement input on cancel
        onFoot.Movement.canceled += ctx => movementInput = Vector2.zero;

        // Update look input on performed
        onFoot.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();

        // Reset look input on cancel
        onFoot.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    /* 
     * FixedUpdate is called at a fixed interval and is independent of frame rate.
     * It processes movement input by calling the ProcessMovement method in the 
     * PlayerMotor component.
     */
    void FixedUpdate()
    {
        if (motor != null)
        {
            // Process movement input
            motor.ProcessMovement(movementInput);
        }
    }

    /* 
     * LateUpdate is called after all Update functions have been called. It processes 
     * look input by calling the ProcessLook method in the PlayerLook component.
     */
    void LateUpdate()
    {
        if (look != null)
        {
            // Process look input
            look.ProcessLook(lookInput);
        }
    }

    /* 
     * OnEnable is called when the object becomes enabled and active. It enables 
     * the on-foot input actions to start receiving player input.
     */
    void OnEnable()
    {
        // Enable on-foot input actions
        onFoot.Enable();
    }

    /* 
     * OnDisable is called when the behaviour becomes disabled. It disables the 
     * on-foot input actions to stop receiving player input.
     */
    void OnDisable()
    {
        // Disable on-foot input actions
        onFoot.Disable();
    }
}
