using System.Collections.Generic;
using UnityEngine;

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

    // Global flag, true while Uncle Fedrick instance is present in the scene
    private static bool isUnclePresent = false;

    // Track the currently-present Uncle instance and whether companions were spawned for it
    private CustomerStateMachine currentUncle = null;
    private bool companionsSpawnedForCurrentUncle = false;

    private void Start()
    {
        spawner = CustomerSpawnerScript.instance;
        if (spawner == null)
        {
            Debug.Log("UncleFedrickSpawnHandler, No CustomerSpawnerScript.instance found in scene");
            enabled = false;
            return;
        }

        // Subscribe to spawn notification (instance event) and to the spawner's static exit event
        spawner.OnCustomerSpawned += OnCustomerSpawned;
        CustomerSpawnerScript.OnCustomerExit += OnCustomerExit;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnCustomerSpawned -= OnCustomerSpawned;
            CustomerSpawnerScript.OnCustomerExit -= OnCustomerExit;
        }
    }

    private void OnCustomerSpawned(CustomerStateMachine customer, GameObject prefab)
    {
        if (customer == null || customer.profile == null)
        {
            return;
        }

        // Only handle Uncle Fedrick here
        if (customer.profile.customerName != specialCustomerName)
        {
            return;
        }

        // If an Uncle is already present, remove this duplicate immediately
        if (isUnclePresent)
        {
            Debug.Log("UncleFedrickSpawnHandler: Duplicate Uncle spawned, removed duplicate");

            // It will remove it from its active list and free up chair/queue transforms
            CustomerSpawnerScript.OnCustomerExit?.Invoke(customer);

            // Now destroy the duplicate GameObject
            Destroy(customer.gameObject);

            return;
        }

        // Register the newly spawned Uncle and prevent further spawns until he exits
        isUnclePresent = true;
        currentUncle = customer;
        companionsSpawnedForCurrentUncle = false;

        // If in rush hour and companions haven't been spawned for this Uncle, spawn them now
        if (spawner.eventRushHour && !companionsSpawnedForCurrentUncle)
        {
            companionsSpawnedForCurrentUncle = true;

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

                // If no child specific prefab provided, fall back to spawner's consumer prefabs list
                if (childPrefab == null)
                {
                    if (spawner.customerPrefabList != null && spawner.customerPrefabList.Count > 0)
                    {
                        childPrefab = spawner.customerPrefabList[UnityEngine.Random.Range(0, spawner.customerPrefabList.Count)];
                    }
                    else
                    {
                        Debug.Log("UncleFedrickSpawnHandler: No child prefab available to spawn");
                        break;
                    }
                }

                // Use spawner's public SpawnCustomer with allowCompanions = false to avoid recursion
                spawner.SpawnCustomer(freeChair, freeQueue, false, childPrefab);
            }
        }

    }

    private void OnCustomerExit(CustomerStateMachine customer)
    {
        if (customer == null || customer.profile == null)
        {
            return;
        }

        if (customer.profile.customerName != specialCustomerName)
        {
            return;
        }

        // If the exiting customer is the tracked Uncle, clear the tracking so Uncle can spawn again
        if (customer == currentUncle)
        {
            currentUncle = null;
            companionsSpawnedForCurrentUncle = false;
            isUnclePresent = false;
            return;
        }

        // If some a duplicate of Uncle instance exits, this does not clear the global flag
        // unless we no longer have a tracked Uncle so gg
        // Unity's == operator returns true for destroyed UnityEngine.Object, so this
        // correctly handles the destroyed but not null case
        // otherwise ignore duplicate's exit and keep the original Uncle tracked
        if (currentUncle == null)
        {
            isUnclePresent = false;
            companionsSpawnedForCurrentUncle = false;
            currentUncle = null;
        }
    }
}
