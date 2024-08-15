using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 10f;          // Speed of the camera movement
    public float rotationSpeed = 100f;         // Speed of the camera rotation
    public float heightAdjustmentSpeed = 5f;   // Speed of the camera height adjustment
    public float smoothRotationSpeed = 5f;     // Speed of smoothing rotation

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        // Camera movement using WASD or arrow keys
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.Self);

        // Camera height adjustment using Q and E keys
        float heightAdjustment = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            heightAdjustment = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            heightAdjustment = 1f;
        }
        transform.Translate(0f, heightAdjustment * heightAdjustmentSpeed * Time.deltaTime, 0f);

        // Camera rotation using the mouse
        rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        // Clamping the vertical rotation to prevent flipping
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Apply the rotation with smoothing
        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothRotationSpeed * Time.deltaTime);
    }
}
