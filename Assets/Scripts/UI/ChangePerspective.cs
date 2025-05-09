using UnityEngine;

public class ChangePerspective : MonoBehaviour
{
    [Header("Initial Position & Rotation")]
    public Transform startTransform;

    [Header("Target Position & Rotation")]
    public Transform targetTransform;

    [Header("Smooth Move Settings")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    private bool movingToTarget = false;
    private bool position = false;
    void Start()
    {
        if (startTransform != null)
        {
            // Immediately set camera to start position and rotation
            transform.position = startTransform.position;
            transform.rotation = startTransform.rotation;
        }
    }

    void Update()
    {
        if (movingToTarget && targetTransform != null && !position)
        {
            // Smoothly move to target position
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * moveSpeed);

            // Smoothly rotate to target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, Time.deltaTime * rotateSpeed);

            // Optional: stop moving when close enough
            if (Vector3.Distance(transform.position, targetTransform.position) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetTransform.rotation) < 0.1f)
            {
                movingToTarget = false;
            }
        }
        else if (movingToTarget && targetTransform != null && position)
        {
            // Smoothly move to target position
            transform.position = Vector3.Lerp(transform.position, startTransform.position, Time.deltaTime * moveSpeed);

            // Smoothly rotate to target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, startTransform.rotation, Time.deltaTime * rotateSpeed);

            // Optional: stop moving when close enough
            if (Vector3.Distance(transform.position, startTransform.position) < 0.01f &&
                Quaternion.Angle(transform.rotation, startTransform.rotation) < 0.1f)
            {
                movingToTarget = false;
            }
        }
    }

    public void MoveToTarget()
    {
        if (!position)
        {
            movingToTarget = true;
            position = true;

        }
        else
        {
            movingToTarget = true;
            position = false;
        }
        
    }
}
