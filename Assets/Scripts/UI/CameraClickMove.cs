using UnityEngine;

public class CameraClickMove : MonoBehaviour
{
    public Camera mainCamera;               // Reference to the main camera
    public float moveSpeed = 5f;            // Camera movement speed
    public Vector3 positionOffset;          // Customizable XYZ offset from the target object

    private Transform targetObject;         // Currently selected target
    private Vector3 initialPosition;        // Original position of the camera
    private Quaternion initialRotation;     // Original rotation of the camera
    private bool isReturning = false;       // Flag to return camera to initial position

    void Start()
    {
        // Store the initial position and rotation of the camera
        initialPosition = mainCamera.transform.position;
        initialRotation = mainCamera.transform.rotation;
    }

    void Update()
    {
        // Check for mouse input to select a new object
        if (Input.GetMouseButtonDown(0))  // Left-click
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Selectable"))  // Clicked a valid object
                {
                    StartFollowing(hit.transform);
                }
            }
        }

        // Return camera to initial position when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToInitialPosition();
        }

        // Follow the target or return to initial position smoothly
        if (targetObject != null && !isReturning)
        {
            FollowTarget();
        }
        else if (isReturning)
        {
            ReturnSmoothly();
        }
    }

    /// <summary>
    /// Start following the target object.
    /// </summary>
    void StartFollowing(Transform newTarget)
    {
        targetObject = newTarget;
        isReturning = false;
    }

    /// <summary>
    /// Follows the target object with specified XYZ offset.
    /// </summary>
    void FollowTarget()
    {
        Vector3 targetPosition = targetObject.position + targetObject.right * positionOffset.x
                                                      + targetObject.up * positionOffset.y
                                                      + targetObject.forward * positionOffset.z;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Returns the camera to its initial position smoothly.
    /// </summary>
    void ReturnToInitialPosition()
    {
        targetObject = null;  // Stop following the object
        isReturning = true;   // Start returning to initial position
    }

    /// <summary>
    /// Smoothly returns the camera to the original position and rotation.
    /// </summary>
    void ReturnSmoothly()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, initialPosition, moveSpeed * Time.deltaTime);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, initialRotation, moveSpeed * Time.deltaTime);

        // Stop moving if close to the initial position and rotation
        if (Vector3.Distance(mainCamera.transform.position, initialPosition) < 0.1f &&
            Quaternion.Angle(mainCamera.transform.rotation, initialRotation) < 1f)
        {
            isReturning = false;  // Stop moving once back to the original position
        }
    }
}
