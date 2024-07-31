using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerLook))]
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;

    private Vector2 movementInput;
    private Vector2 lookInput;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        if (motor == null || look == null)
        {
            Debug.LogError("Missing required components: PlayerMotor or PlayerLook");
            enabled = false;
            return;
        }

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.StartSprint();
        onFoot.Sprint.canceled += ctx => motor.StopSprint();
        onFoot.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        onFoot.Movement.canceled += ctx => movementInput = Vector2.zero;
        onFoot.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        onFoot.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (motor != null)
        {
            motor.ProcessMovement(movementInput);
        }
    }

    void LateUpdate()
    {
        if (look != null)
        {
            look.ProcessLook(lookInput);
        }
    }

    void OnEnable()
    {
        onFoot.Enable();
    }

    void OnDisable()
    {
        onFoot.Disable();
    }
}
