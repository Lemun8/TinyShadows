using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 200f;      // Speed of the rotation
    private bool isRotating = false;        // Flag for rotation status
    private Quaternion targetRotation;      // Holds the target rotation
    private FezMove playerMove;             // Reference to the FezMove script on the player

    void Start()
    {
        targetRotation = transform.rotation;                // Initialize the starting rotation
        playerMove = FindObjectOfType<FezMove>();           // Find and reference the FezMove script on the player
    }

    void Update()
    {
        HandleInput();
        SmoothRotate();
    }

    void HandleInput()
    {
        if (isRotating) return;

        if (Input.GetKeyDown(KeyCode.Q))                    // Rotate left
        {
            RotateEnvironment(Vector3.up, 90);
        }
        else if (Input.GetKeyDown(KeyCode.E))               // Rotate right
        {
            RotateEnvironment(Vector3.up, -90);
        }
    }

    void RotateEnvironment(Vector3 axis, float angle)
    {
        isRotating = true;
        targetRotation *= Quaternion.Euler(axis * angle);   // Set the new target rotation for the pivot
    }

    void SmoothRotate()
    {
        if (isRotating)
        {
            // Smoothly rotate the pivot towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if rotation is almost complete
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;         // Snap to exact target rotation
                isRotating = false;                          // Allow next rotation input

                // Update the player's facing direction based on the current rotation angle
                UpdatePlayerFacingDirection();
            }
        }
    }

    void UpdatePlayerFacingDirection()
    {
        // Determine the facing direction based on the y-angle of rotation
        float currentYRotation = transform.eulerAngles.y;

        FezMove.FacingDirection newDirection;
        if (Mathf.Approximately(currentYRotation, 0f) || Mathf.Approximately(currentYRotation, 360f))
        {
            newDirection = FezMove.FacingDirection.Front;
        }
        else if (Mathf.Approximately(currentYRotation, 90f))
        {
            newDirection = FezMove.FacingDirection.Right;
        }
        else if (Mathf.Approximately(currentYRotation, 180f))
        {
            newDirection = FezMove.FacingDirection.Back;
        }
        else if (Mathf.Approximately(currentYRotation, 270f))
        {
            newDirection = FezMove.FacingDirection.Left;
        }
        else
        {
            // In case of rounding errors, default to Front
            newDirection = FezMove.FacingDirection.Front;
        }

        // Update the character's facing direction
        playerMove.UpdateToFacingDirection(newDirection, currentYRotation);
    }
}
