using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float xAxisSensitivity = 30f;
    [SerializeField] private float yAxisSensitivity = 30f;

    private float xAxisRotation = 0f;

    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (cam == null)
        {
            Debug.LogError("No camera assigned to PlayerLook and no main camera found in the scene.");
            enabled = false;
        }
    }

    public void ProcessLook(Vector2 input)
    {
        if (input == Vector2.zero) return;

        float mouseX = input.x;
        float mouseY = input.y;

        // Calculate camera rotation for the x-axis
        xAxisRotation -= mouseY * yAxisSensitivity * Time.deltaTime;
        xAxisRotation = Mathf.Clamp(xAxisRotation, -80f, 80f);

        // Apply the camera rotation
        cam.transform.localRotation = Quaternion.Euler(xAxisRotation, 0, 0);

        // Rotate the player around the y-axis
        transform.Rotate(Vector3.up * mouseX * xAxisSensitivity * Time.deltaTime);
    }
}
