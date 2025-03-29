
using UnityEngine;

public class RotateAroundPoint : MonoBehaviour
{
    public Transform pointToRotateAround; // The point you want to rotate around.
    public float rotationSpeed = 10f;     // Speed of rotation in degrees per second.
    [SerializeField] private string axis;

    void Update()
    {
        if (axis == "Y")
        {
            // Rotate the object around the specified point.
            transform.RotateAround(
                pointToRotateAround.position,   // Point to rotate around
                Vector3.up,                    // Axis of rotation (e.g., Vector3.up for Y-axis)
                rotationSpeed * Time.deltaTime // Rotation angle (degrees per frame)
            );
        }
        if (axis == "Z")
        {
            // Rotate the object around the specified point.
            transform.RotateAround(
                pointToRotateAround.position,   // Point to rotate around
                Vector3.forward,                    // Axis of rotation (e.g., Vector3.up for Y-axis)
                rotationSpeed * Time.deltaTime // Rotation angle (degrees per frame)
            );
        }
        if (axis == "X")
        {
            // Rotate the object around the specified point.
            transform.RotateAround(
                pointToRotateAround.position,   // Point to rotate around
                Vector3.right,                    // Axis of rotation (e.g., Vector3.up for Y-axis)
                rotationSpeed * Time.deltaTime // Rotation angle (degrees per frame)
            );
        }
    }
}
