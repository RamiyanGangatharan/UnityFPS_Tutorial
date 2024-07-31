using UnityEngine;

/* 
 * This class manages game settings such as locking the cursor and setting the target framerate.
 * It provides options to lock the cursor within the game window and to unlock the framerate.
 */
public class GameSettings : MonoBehaviour
{
    /* 
     * Serialized fields for configuring whether the cursor should be locked, 
     * whether the framerate should be unlocked, and the target framerate.
     */
    [SerializeField] private bool lockCursor = true;
    [SerializeField] private bool unlockFramerate = true;
    [SerializeField] private int targetFramerate = 60;

    /* 
     * Start is called before the first frame update.
     * It applies the initial settings for cursor locking and framerate.
     */
    void Start()
    {
        /* 
         * Locks the cursor to the window if lockCursor is true.
         * The cursor remains locked until the user presses the Escape key.
         */
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /* 
         * Unlocks the framerate and sets it to the target framerate if unlockFramerate is true.
         * By default, Unity caps the framerate (likely to 30FPS), so this setting overrides it.
         */
        if (unlockFramerate)
        {
            Application.targetFrameRate = targetFramerate;
        }
    }

    /* 
     * Update is called once per frame.
     * It manages the cursor locking and unlocking based on user input.
     */
    void Update()
    {
        /* 
         * Handles cursor locking and unlocking based on user input.
         * If the Escape key is pressed, the cursor is unlocked and becomes visible.
         * If the left mouse button is clicked, the cursor is locked to the window and becomes invisible.
         */
        if (lockCursor)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
