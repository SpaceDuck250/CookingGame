using System;
using System.Collections.Generic;
using UnityEngine;
using Customer;

[DisallowMultipleComponent]
public class UncleFedrickSpawnHandler : MonoBehaviour
{
    // Name used to identify Uncle Fedrick in CustomerData.customerName
    public string specialCustomerName = "Uncle Fedrick";

    // Number of companions to spawn when Uncle Fedrick arrives
    public int specialCompanionCount = 2;

    // Child prefabs to spawn as companions
    public List<GameObject> fedrickChildrenPrefabs = new List<GameObject>();

    private CustomerSpawnerScript spawner;

    private void Start()
    {
        spawner = CustomerSpawnerScript.instance;
        if (spawner == null)
        {
            Debug.Log("UncleFedrickSpawnHandler, No CustomerSpawnerScript.instance found in scene");
            enabled = false;
            return;
        }

        // Subscribe to spawn notifications
        spawner.OnCustomerSpawned += OnCustomerSpawned;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnCustomerSpawned -= OnCustomerSpawned;
        }
    }

    private void OnCustomerSpawned(CustomerStateMachine customer, GameObject prefab)
    {
        if (customer == null || customer.profile == null)
        {
            return;
        }

        // Only trigger for Uncle Fedrick (and only during rush hour per original behaviour)
        if (!spawner.eventRushHour)
        {
            return;
        }

        if (customer.profile.customerName != specialCustomerName)
        {
            return;
        }

        // Determine how many companions we can actually spawn without exceeding maxCustomers
        int availableSlots = Mathf.Max(0, spawner.maxCustomers - spawner.ActiveCustomerCount);
        int spawnCount = Mathf.Min(specialCompanionCount, availableSlots);

        for (int i = 0; i < spawnCount; i++)
        {
            Transform freeChair = spawner.GetFreeChair();
            Transform freeQueue = spawner.GetFreeQueuePoint();

            if (freeChair == null || freeQueue == null)
            {
                break;
            }

            // Randomly select a child prefab from the list, if available
            GameObject childPrefab = null;
            if (fedrickChildrenPrefabs != null && fedrickChildrenPrefabs.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, fedrickChildrenPrefabs.Count);
                childPrefab = fedrickChildrenPrefabs[index];
            }

            // If no child specific prefab provided, fall back to spawner's consumer prefabs list.
            if (childPrefab == null)
            {
                if (spawner.customerPrefabList != null && spawner.customerPrefabList.Count > 0)
                {
                    childPrefab = spawner.customerPrefabList[UnityEngine.Random.Range(0, spawner.customerPrefabList.Count)];
                }
                else
                {
                    Debug.Log("UncleFedrickSpawnHandler: No child prefab available to spawn.");
                    break;
                }
            }

            // Use spawner's public SpawnCustomer with allowCompanions = false to avoid recursion
            spawner.SpawnCustomer(freeChair, freeQueue, false, childPrefab);
        }
    }
}
