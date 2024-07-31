using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private bool lockCursor = true;
    [SerializeField] private bool unlockFramerate = true;
    [SerializeField] private int targetFramerate = 60;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (unlockFramerate)
        {
            Application.targetFrameRate = targetFramerate;
        }
    }

    void Update()
    {
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
