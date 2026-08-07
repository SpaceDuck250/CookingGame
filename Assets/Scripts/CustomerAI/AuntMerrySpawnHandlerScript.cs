using UnityEngine;

public class AuntMerrySpawnHandlerScript : MonoBehaviour
{
    public CustomerSpawnerScript customerSpawner;

    public GameObject auntMerryPrefab;
    public CustomerStateMachine activeAuntMerry;
    public Transform[] inspectionPoints;

    public bool inspectorRequestPending;
    private int auntMerryOriginalIndex = -1;
    public bool shouldInspectNextVisit;
    private bool auntMerryRemovedFromSpawnList;
    private bool subscribedToSpawner;

    private void Awake()
    {
        ResolveReferences();
    }

    private void Start()
    {
        ResolveReferences();

        if (subscribedToSpawner || customerSpawner == null)
        {
            return;
        }

        customerSpawner.OnCustomerSpawned += HandleCustomerSpawned;

        subscribedToSpawner = true;

        RememberAuntMerryListIndex();
    }

    private void OnEnable()
    {
        ResolveReferences();

        if (subscribedToSpawner || customerSpawner == null)
        {
            return;
        }

        customerSpawner.OnCustomerSpawned += HandleCustomerSpawned;

        subscribedToSpawner = true;

        CustomerSpawnerScript.OnCustomerExit -= HandleCustomerExit;
        CustomerSpawnerScript.OnCustomerExit += HandleCustomerExit;

        RememberAuntMerryListIndex();
    }

    private void OnDisable()
    {
        if (!subscribedToSpawner)
        {
            return;
        }

        if (customerSpawner != null)
        {
            customerSpawner.OnCustomerSpawned -= HandleCustomerSpawned;
        }

        subscribedToSpawner = false;

        CustomerSpawnerScript.OnCustomerExit -= HandleCustomerExit;
    }

    private void Update()
    {
        // Fixed the spawner if Aunt Merry was destroyed without invoking OnCustomerExit
        if (activeAuntMerry == null && auntMerryRemovedFromSpawnList)
        {
            RestoreAuntMerryToSpawnList();
        }
    }

    // Set all the references
    private void ResolveReferences()
    {
        if (customerSpawner == null)
        {
            customerSpawner = CustomerSpawnerScript.instance;
        }
    }

    public void SetLastInspectionResult(bool sawFood)
    {
        shouldInspectNextVisit = sawFood;

        if (sawFood)
        {
            Debug.Log(
                "Aunt Merry saw food this visit, she will inspect again next time.");
        }
        else
        {
            Debug.Log("Aunt Merry saw no food this visit, she will skip inspection next time.");
        }
    }

    private void HandleCustomerSpawned(CustomerStateMachine spawnedCustomer, GameObject spawnedPrefab)
    {
        if (spawnedPrefab != auntMerryPrefab)
        {
            return;
        }

        if (spawnedCustomer == null)
        {
            return;
        }

        // Handles duplicate situations just in case
        if (activeAuntMerry != null && activeAuntMerry != spawnedCustomer)
        {
            Debug.Log("A duplicate Aunt Merry was detected and destroyed.");

            Destroy(spawnedCustomer.gameObject);
            return;
        }

        activeAuntMerry = spawnedCustomer;
        // Prevent the random spawner from selecting another Aunt Merry while this one remains in the scene
        RemoveAuntMerryFromSpawnList();

        Debug.Log("Aunt Merry spawned and was temporarily removed from the random customer list.");

        AuntMerryCustomerScript auntMerryScript = spawnedCustomer.GetComponent<AuntMerryCustomerScript>();

        if (auntMerryScript == null)
        {
            auntMerryScript = spawnedCustomer.GetComponentInChildren<AuntMerryCustomerScript>();
        }

        if (auntMerryScript != null)
        {
            auntMerryScript.SetInspectionSetup(inspectionPoints, shouldInspectNextVisit, this);
        }
        else
        {
            Debug.Log("Aunt Merry does not have AuntMerryCustomerScript.");
        }
    }

    // Resets Aunt Merry spawn
    private void HandleCustomerExit(CustomerStateMachine exitingCustomer)
    {
        if (exitingCustomer == null || exitingCustomer != activeAuntMerry)
        {
            return;
        }

        activeAuntMerry = null;

        RestoreAuntMerryToSpawnList();

        Debug.Log("Aunt Merry exited and can be randomly spawned again.");
    }

    // Saved Aunt Merry data for the next spawn
    private void RememberAuntMerryListIndex()
    {
        if (customerSpawner == null || auntMerryPrefab == null || customerSpawner.customerPrefabListToSpawn == null)
        {
            return;
        }

        int foundIndex = customerSpawner.customerPrefabListToSpawn.IndexOf(auntMerryPrefab);

        if (foundIndex >= 0)
        {
            auntMerryOriginalIndex = foundIndex;
        }
    }

    private void RemoveAuntMerryFromSpawnList()
    {
        if (customerSpawner == null || auntMerryPrefab == null || customerSpawner.customerPrefabListToSpawn == null)
        {
            return;
        }

        if (auntMerryRemovedFromSpawnList)
        {
            return;
        }

        // Remove every Aunt Merry list entry while one is active in case there are duplicates in the spawn list
        for (int i = customerSpawner.customerPrefabListToSpawn.Count - 1; i >= 0; i--)
        {
            if (customerSpawner.customerPrefabListToSpawn[i] != auntMerryPrefab)
            {
                continue;
            }

            if (auntMerryOriginalIndex < 0 || i < auntMerryOriginalIndex)
            {
                auntMerryOriginalIndex = i;
            }

            customerSpawner.customerPrefabListToSpawn.RemoveAt(i);

            auntMerryRemovedFromSpawnList = true;
        }
    }

    private void RestoreAuntMerryToSpawnList()
    {
        if (customerSpawner == null || auntMerryPrefab == null || customerSpawner.customerPrefabListToSpawn == null)
        {
            return;
        }

        if (!auntMerryRemovedFromSpawnList)
        {
            return;
        }

        // Prevent accidental duplicate restoration
        if (customerSpawner.customerPrefabListToSpawn.Contains(auntMerryPrefab))
        {
            auntMerryRemovedFromSpawnList = false;
            return;
        }

        // Make sure that the spawn list is set to include Aunt Merry
        int safeIndex = Mathf.Clamp(auntMerryOriginalIndex, 0, customerSpawner.customerPrefabListToSpawn.Count);
        customerSpawner.customerPrefabListToSpawn.Insert(safeIndex, auntMerryPrefab);

        auntMerryRemovedFromSpawnList = false;
    }
}
