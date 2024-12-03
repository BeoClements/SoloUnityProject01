using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public Rigidbody playerBody;
    public bool lockCursor = true;

    private float xRotation = 0f;

    void Start()
    {
        // Lock the cursor at the center of the screen and hide it
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Apply vertical rotation clamping to avoid flipping over
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to the camera and player body
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        Quaternion deltaRotation = Quaternion.Euler(0, mouseX, 0);
        playerBody.MoveRotation(playerBody.rotation * deltaRotation);
    }
}