using UnityEngine;

public class ConveyorMotion : MonoBehaviour
{
    private float speed = 0.2f; // Speed of movement
    private string currentConveyorTag = null; // Tracks the conveyor the object is on
    private string nextConveyorTag = null; // Tracks the next conveyor to transition to

    void Update()
    {

        // Move the object based on the current conveyor
        if (currentConveyorTag == "zConveyor")
        {
            transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
        }
        else if (currentConveyorTag == "xConveyor")
        {
            transform.position += gameObject.name == "door1" || gameObject.name == "door2" ? 
                new Vector3(speed * Time.deltaTime, 0, 0): 
                new Vector3(-speed * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the object is already on a conveyor, set the next conveyor tag
        if (currentConveyorTag != null && (other.CompareTag("zConveyor") || other.CompareTag("xConveyor")))
        {
            nextConveyorTag = other.tag;
        }
        // If the object is not on a conveyor, set the current conveyor tag
        else if (currentConveyorTag == null && (other.CompareTag("zConveyor") || other.CompareTag("xConveyor")))
        {
            currentConveyorTag = other.tag;
        }
        if (other.CompareTag("Deleter"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the object exits the current conveyor
        if (other.CompareTag(currentConveyorTag))
        {
            // Transition to the next conveyor if it exists
            currentConveyorTag = nextConveyorTag;
            nextConveyorTag = null; // Reset next conveyor tag after transitioning
        }
    }
}
