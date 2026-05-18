using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [Header("Sensitivity")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float controllerSensitivity = 120f;
    [SerializeField] private float deadzone = 0.15f;

    private float xRotation;
    private Vector2 lookInput;
    private bool usingMouseLook;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 finalLookInput = GetLookInput(out bool isMouse);

        float lookX;
        float lookY;

        if (isMouse)
        {
            // Mouse delta is already frame based, so no Time.deltaTime here
            lookX = finalLookInput.x * mouseSensitivity;
            lookY = finalLookInput.y * mouseSensitivity;
        }
        else
        {
            // Controller stick is continuous input, so use Time.deltaTime
            lookX = finalLookInput.x * controllerSensitivity * Time.deltaTime;
            lookY = finalLookInput.y * controllerSensitivity * Time.deltaTime;
        }

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookX);
    }

    private Vector2 GetLookInput(out bool isMouse)
    {
        isMouse = false;

        // Arcade setup: controller 2 left stick controls look
        if (Gamepad.all.Count > 1)
        {
            Vector2 controller2LeftStick = Gamepad.all[1].leftStick.ReadValue();

            if (controller2LeftStick.magnitude > deadzone)
            {
                return controller2LeftStick;
            }
        }

        // Normal controller setup: right stick controls look
        if (Gamepad.current != null)
        {
            Vector2 rightStick = Gamepad.current.rightStick.ReadValue();

            if (rightStick.magnitude > deadzone)
            {
                return rightStick;
            }
        }

        // Mouse / Input System Look action
        if (lookInput.sqrMagnitude > 0.001f)
        {
            isMouse = true;
            return lookInput;
        }

        return Vector2.zero;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}