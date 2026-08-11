using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RatSpawnerScript : MonoBehaviour
{
    public GameObject ratPrefab;

    public float changeOfSpawning = 0.1f;

    public int maxRats = 1;

    public float spawnTimer;
    public float waitTime = 20;

    public Transform ratContainer;

    private void Update()
    {
        if (ratContainer.childCount >= maxRats)
        {
            return;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waitTime)
        {
            spawnTimer = 0;

            TrySpawningRat();
        }
    }

    public void TrySpawningRat()
    {

    }

    public bool CheckIfTheresFoodOnFloor()
    {
        return false;
    }


}
