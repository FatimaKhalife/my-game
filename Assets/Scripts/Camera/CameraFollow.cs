using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float aheadDistance = 3f; // How far ahead the camera should look
    public float cameraSpeed = 2f; // Smoothing speed for the look ahead effect
    private float lookAhead = 0f; // The current look ahead value
    private float maxLookAhead = 5f; // Maximum look ahead distance

    public Camera mainCamera; // Reference to the camera
    public float zoomLevel = 6f; // Desired orthographic size (zoom level)

    private void Start()
    {
        transform.position = player.position + offset;

        // Ensure that we have a reference to the camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Get the main camera if not assigned
        }

        // Set the camera's orthographic size to the desired zoom level
        mainCamera.orthographicSize = zoomLevel;
    }

    private void Update()
    {
        float playerDirection = Input.GetAxis("Horizontal"); // direction

        // Update the lookAhead based on player's direction
        if (playerDirection > 0)
        {
            lookAhead = Mathf.Lerp(lookAhead, aheadDistance, Time.deltaTime * cameraSpeed); // Look ahead right
        }
        else if (playerDirection < 0)
        {
            lookAhead = Mathf.Lerp(lookAhead, -aheadDistance, Time.deltaTime * cameraSpeed); // Look ahead left
        }
        else
        {
            lookAhead = Mathf.Lerp(lookAhead, 0, Time.deltaTime * cameraSpeed); // No movement, no look ahead
        }

        // Clamp the lookAhead value to avoid the camera moving too far ahead
        lookAhead = Mathf.Clamp(lookAhead, -maxLookAhead, maxLookAhead);

        // Calculate the camera position based on the player's position and the offset
        Vector3 desiredPosition = player.position + offset;

        // Apply the look ahead
        desiredPosition.x += lookAhead;

        // Smooth camera movement towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
