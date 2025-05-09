using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;

    private bool isCollidingWithCar = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car" || other.gameObject.tag == "Object" || other.gameObject.tag == "leftFront" || other.gameObject.tag == "leftBack" || other.gameObject.tag == "rightFront" || other.gameObject.tag == "rightBack")
        {
            isCollidingWithCar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Car" || other.gameObject.tag == "Object" || other.gameObject.tag == "leftFront" || other.gameObject.tag == "leftBack" || other.gameObject.tag == "rightFront" || other.gameObject.tag == "rightBack")
        {
            isCollidingWithCar = false;
        }
    }

    public void Update()
    {
        if (!isCollidingWithCar)
        {
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
        }
    }
}
