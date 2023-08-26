using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float mouseSensitivity = 50.0f;
    public float gamepadSensitivity = 20.0f;
    public float maxPitch = 90.0f;

    private Camera playerCamera;
    private Vector2 currentLookDelta;
    private PlayerControls controls;

    private float pitch = 0.0f;

    private void Awake()
    {
        controls = new PlayerControls();

        // Bind the look action
        controls.Player.Look.performed += ctx => {
            currentLookDelta = ctx.ReadValue<Vector2>();

            // Adjust sensitivity based on device
            if (ctx.control.device is Mouse)
            {
                currentLookDelta *= mouseSensitivity;
            }
            else if (ctx.control.device is Gamepad)
            {
                currentLookDelta *= gamepadSensitivity * 10.0f;
            }
        };

        controls.Player.Look.canceled += ctx => currentLookDelta = Vector2.zero;

        playerCamera = GetComponentInChildren<Camera>();

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        HandleLook();
    }

    private void HandleLook()
    {
        float lookX = currentLookDelta.x * Time.deltaTime;
        float lookY = currentLookDelta.y * Time.deltaTime;

        // Calculate pitch
        pitch -= lookY;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        // Rotate the player object for yaw (left/right movement)
        transform.Rotate(Vector3.up * lookX);

        // Rotate the camera for pitch (up/down movement)
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
