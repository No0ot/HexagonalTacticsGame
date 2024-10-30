using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    // Movement parameters
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    // Zoom parameters
    public float zoomSpeed = 10f;
    public float minDistance = 5f;
    public float maxDistance = 50f;

    // Rotation parameters
    public float rotationSpeed = 5f;
    public Transform rotationPoint;
    public float minVerticalAngle = 0f;
    public float maxVerticalAngle = 90f;

    private Vector3 initialPosition;
    private float currentDistance; // Distance from the rotation point

    void Start()
    {
        initialPosition = transform.position;

        // Calculate initial distance between the camera and the rotation point
        currentDistance = Vector3.Distance(transform.position, rotationPoint.position);
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 pos = transform.position;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Flatten the movement along the horizontal plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        if (Input.GetKey("w"))
        {
            pos += forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos -= forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos += right * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos -= right * panSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            // Move the camera forward or backward relative to the rotation point
            Vector3 direction = (transform.position - rotationPoint.position).normalized;
            currentDistance -= scroll * zoomSpeed * 100f * Time.deltaTime;

            // Clamp the zoom distance
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

            // Update camera position based on zoom distance
            transform.position = rotationPoint.position + direction * currentDistance;
        }
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))  // Right mouse button for rotation
        {
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
            //
            //// Rotate only around the Y-axis
            //transform.RotateAround(rotationPoint.position, Vector3.up, horizontalRotation);
            // Horizontal rotation around target's Y axis
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            horizontalRotation += mouseX;
            transform.RotateAround(rotationPoint.position, Vector3.up, mouseX);

            // Vertical rotation around target's X axis
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            verticalRotation = Mathf.Clamp(verticalRotation + mouseY, minVerticalAngle, maxVerticalAngle);

            // Apply the vertical rotation, clamping to max/min limits
            transform.RotateAround(rotationPoint.position, transform.right, mouseY);

            // Ensure the clamped vertical angle is applied
            Vector3 clampedEulerAngles = transform.eulerAngles;
            clampedEulerAngles.x = Mathf.Clamp(clampedEulerAngles.x, minVerticalAngle, maxVerticalAngle);
            transform.eulerAngles = clampedEulerAngles;
        }
    }

    public void ResetCameraPosition()
    {
        transform.position = initialPosition;
        currentDistance = Vector3.Distance(transform.position, rotationPoint.position);  // Reset distance
    }
}