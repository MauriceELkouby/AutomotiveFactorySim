using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;

    private bool isCollidingWithCar = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            isCollidingWithCar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
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
