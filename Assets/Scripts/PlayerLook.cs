using UnityEngine;

/* 
 * This class handles the player's look functionality, including rotating the camera
 * based on mouse input and clamping the vertical rotation to prevent over-rotation.
 * It requires a CharacterController component to function correctly.
 */
[RequireComponent(typeof(CharacterController))]

public class PlayerLook : MonoBehaviour
{
    /* 
     * Serialized fields for assigning the camera and setting sensitivity for 
     * x-axis and y-axis movements.
     */
    [SerializeField] private Camera cam;
    [SerializeField] private float xAxisSensitivity = 30f;
    [SerializeField] private float yAxisSensitivity = 30f;

    // Private variable to keep track of the current x-axis rotation
    private float xAxisRotation = 0f;

    /* 
     * Awake is called when the script instance is being loaded.
     * It checks if the camera is assigned and logs an error if no camera is found.
     */
    private void Awake()
    {
        // Assign the main camera if no camera is assigned
        if (cam == null)
        {
            cam = Camera.main;
        }

        // Log an error and disable the script if no camera is found
        if (cam == null)
        {
            Debug.LogError("No camera assigned to PlayerLook and no main camera found in the scene.");
            enabled = false;
        }
    }

    /* 
     * Processes the look input for rotating the camera and the player.
     * It clamps the vertical rotation to prevent over-rotation.
     * 
     * Parameters:
     *   input (Vector2): The input vector containing x and y values for mouse movement.
     *   
     * Explanation:
     * - The input vector contains x and y values representing the mouse movement.
     * - mouseX is the horizontal movement, and mouseY is the vertical movement.
     * 
     * - Vertical Rotation (x-axis):
     *   - xAxisRotation is adjusted by subtracting mouseY multiplied by 
     *     yAxisSensitivity and Time.deltaTime.
     *     
     *   - Time.deltaTime ensures the rotation is frame rate independent.
     *   
     *   - The rotation is clamped between -80 and 80 degrees to prevent 
     *     excessive vertical rotation.
     *     
     *   - The camera's local rotation is updated to reflect the new xAxisRotation.
     * 
     * - Horizontal Rotation (y-axis):
     *   - The player rotates around the y-axis based on mouseX multiplied by 
     *     xAxisSensitivity and Time.deltaTime.
     *     
     *   - The transform.Rotate method applies this rotation to the player's transform.
     */

    public void ProcessLook(Vector2 input)
    {
        // Return if no input is detected
        if (input == Vector2.zero) return;

        float mouseX = input.x;
        float mouseY = input.y;

        /* 
         * Calculate camera rotation for the x-axis. The vertical rotation is adjusted 
         * by subtracting the product of mouseY, yAxisSensitivity, and Time.deltaTime. 
         * Time.deltaTime ensures the rotation is frame rate independent.
         */
        xAxisRotation -= mouseY * yAxisSensitivity * Time.deltaTime;

        /* 
         * Clamp the vertical rotation to prevent the camera from rotating too far up or down.
         * Mathf.Clamp restricts xAxisRotation to the range [-80, 80] degrees.
         */
        xAxisRotation = Mathf.Clamp(xAxisRotation, -80f, 80f);

        // Apply the camera rotation.
        cam.transform.localRotation = Quaternion.Euler(xAxisRotation, 0, 0);

        /* 
         * Rotate the player around the y-axis.
         * The horizontal rotation is adjusted by multiplying mouseX by xAxisSensitivity and Time.deltaTime.
         * The rotation is applied to the player's transform around the y-axis using Vector3.up.
         */
        transform.Rotate(Vector3.up * mouseX * xAxisSensitivity * Time.deltaTime);
    }
}
