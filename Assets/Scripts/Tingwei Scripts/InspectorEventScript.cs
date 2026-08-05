using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorEventScript : MonoBehaviour
{
    public AIEventSystemScript eventSystem;
    public AIEventDataScript eventData;
    public CustomerSpawnerScript customerSpawner;

    public LayerMask foodLayer;
    public GameObject inspectorPrefab;
    public float spawnRetryInterval = 2f;

    private readonly Dictionary<GameObject, int> foodOverlapCounts = new Dictionary<GameObject, int>();
    private readonly List<GameObject> destroyedFoodObjects = new List<GameObject>();

    private CustomerStateMachine activeInspector;

    private Coroutine inspectorSpawnCoroutine;

    private bool inspectorEventRunning;
    private bool waitingForInspectorSpawn;
    private int lastReportedFoodAmount = -1;

    private void Awake()
    {
        ResolveReferences();
    }

    private void Reset()
    {
        Collider triggerCollider = GetComponent<Collider>();

        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    // Subscribe and restore state
    private void OnEnable()
    {
        ResolveReferences();

        AIEventSystemScript.OnEventStarted += HandleEventStarted;
        AIEventSystemScript.OnEventFinished += HandleEventFinished;

        if (customerSpawner != null)
        {
            customerSpawner.OnCustomerSpawned += HandleCustomerSpawned;
        }

        CustomerSpawnerScript.OnCustomerExit += HandleCustomerExit;

        ReportFoodAmount();
    }

    // Unsubscribe and reset state
    private void OnDisable()
    {
        AIEventSystemScript.OnEventStarted -= HandleEventStarted;
        AIEventSystemScript.OnEventFinished -= HandleEventFinished;

        if (customerSpawner != null)
        {
            customerSpawner.OnCustomerSpawned -= HandleCustomerSpawned;
        }

        CustomerSpawnerScript.OnCustomerExit -= HandleCustomerExit;

        StopInspectorEventWithoutCompletion();

        foodOverlapCounts.Clear();
        lastReportedFoodAmount = -1;

        ReportFoodAmount();
    }

    private void Update()
    {
        RemoveDestroyedFoodObjects();
    }

    // Resolve references to the event system, event data, and customer spawner
    private void ResolveReferences()
    {
        if (eventSystem == null)
        {
            eventSystem = AIEventSystemScript.Instance;
        }

        if (eventSystem != null)
        {
            if (eventData == null)
            {
                eventData = eventSystem.eventData;
            }

            if (customerSpawner == null)
            {
                customerSpawner = eventSystem.customerSpawner;
            }
        }
    }

    // --------------------------------------------
    // Food Floor Tracking
    // --------------------------------------------

    // When food touch the ground
    private void OnTriggerEnter(Collider other)
    {
        GameObject foodObject = FindFoodObject(other);

        if (foodObject == null)
        {
            return;
        }

        if (foodOverlapCounts.ContainsKey(foodObject))
        {
            foodOverlapCounts[foodObject]++;
        }
        else
        {
            foodOverlapCounts.Add(foodObject, 1);
        }

        ReportFoodAmount();
    }

    // When food leave the ground
    private void OnTriggerExit(Collider other)
    {
        GameObject foodObject = FindFoodObject(other);

        if (foodObject == null)
        {
            return;
        }

        if (!foodOverlapCounts.ContainsKey(foodObject))
        {
            return;
        }

        foodOverlapCounts[foodObject]--;

        // Remove the food object only after all of its colliders have left the floor trigger
        if (foodOverlapCounts[foodObject] <= 0)
        {
            foodOverlapCounts.Remove(foodObject);
        }

        ReportFoodAmount();
    }

    // To check if this GameObject's layer included in foodLayer
    private bool IsInFoodLayer(GameObject objectToCheck)
    {
        if (objectToCheck == null)
        {
            return false;
        }

        return (foodLayer.value & (1 << objectToCheck.layer)) != 0;
    }

    private GameObject FindFoodObject(Collider other)
    {
        if (other == null)
        {
            return null;
        }

        // If the food has a Rigidbody on its root object, every child collider will share this Rigidbody
        // It prevents one food item from being treated as several separate objects
        Rigidbody attachedRigidbody = other.attachedRigidbody;

        if (attachedRigidbody != null &&
            IsInFoodLayer(attachedRigidbody.gameObject))
        {
            return attachedRigidbody.gameObject;
        }

        // The collider may be on a child object while the food layer is assigned to one of its parents
        Transform currentTransform = other.transform;
        GameObject foodObject = null;

        while (currentTransform != null)
        {
            if (IsInFoodLayer(currentTransform.gameObject))
            {
                foodObject = currentTransform.gameObject;
            }

            currentTransform = currentTransform.parent;
        }

        return foodObject;
    }

    // When the food leaves the ground, picked up etc.
    private void RemoveDestroyedFoodObjects()
    {
        destroyedFoodObjects.Clear();

        foreach (KeyValuePair<GameObject, int> foodEntry in foodOverlapCounts)
        {
            if (foodEntry.Key == null)
            {
                destroyedFoodObjects.Add(foodEntry.Key);
            }
        }

        if (destroyedFoodObjects.Count == 0)
        {
            return;
        }

        foreach (GameObject destroyedFood in destroyedFoodObjects)
        {
            foodOverlapCounts.Remove(destroyedFood);
        }

        ReportFoodAmount();
    }

    // Sends the total amount of food on the ground
    private void ReportFoodAmount()
    {
        if (eventData == null)
        {
            ResolveReferences();
        }

        if (eventData == null)
        {
            return;
        }

        int currentFoodAmount = foodOverlapCounts.Count;

        if (currentFoodAmount == lastReportedFoodAmount)
        {
            return;
        }

        lastReportedFoodAmount = currentFoodAmount;

        eventData.SetFoodLyingAround(currentFoodAmount);

        //Debug.Log("Food currently lying on the floor: " + currentFoodAmount);
    }

    // --------------------------------------------
    // Inspector Event
    // --------------------------------------------

    private void HandleEventStarted(HawkerEventType eventType)
    {
        if (eventType != HawkerEventType.Inspector)
        {
            return;
        }

        if (inspectorEventRunning)
        {
            return;
        }

        ResolveReferences();

        if (eventSystem == null || customerSpawner == null || inspectorPrefab == null)
        {
            Debug.Log("InspectorEventScript is missing the Event System, Customer Spawner, or Inspector Prefab reference.");

            // Prevent the central event system from becoming permanently stuck on Inspector

            if (eventSystem != null)
            {
                eventSystem.CompleteEvent(HawkerEventType.Inspector);
            }

            return;
        }

        inspectorEventRunning = true;
        waitingForInspectorSpawn = false;
        activeInspector = null;

        inspectorSpawnCoroutine = StartCoroutine(SpawnInspectorWhenSpaceIsAvailable());

        Debug.Log("Inspector event is waiting to spawn the Inspector.");
    }

    private IEnumerator SpawnInspectorWhenSpaceIsAvailable()
    {
        while (inspectorEventRunning && activeInspector == null)
        {
            bool belowMaximumCustomers = customerSpawner.ActiveCustomerCount < customerSpawner.maxCustomers;

            Transform freeChair = customerSpawner.GetFreeChair();

            Transform freeQueuePoint = customerSpawner.GetFreeQueuePoint();

            if (belowMaximumCustomers && freeChair != null && freeQueuePoint != null)
            {
                // Spawn inside the CustomerSpawner
                waitingForInspectorSpawn = true;

                customerSpawner.SpawnCustomer(freeChair, freeQueuePoint, false, inspectorPrefab);

                if (activeInspector != null)
                {
                    break;
                }

                // CustomerSpawner did not true this, so it would allow another attempt at spawning the inspector
                waitingForInspectorSpawn = false;
            }

            yield return new WaitForSeconds(spawnRetryInterval);
        }

        inspectorSpawnCoroutine = null;
    }

    // Make only claim the inspector prefab that spawned for the Inspector event
    private void HandleCustomerSpawned(CustomerStateMachine spawnedCustomer, GameObject spawnedPrefab)
    {
        if (!inspectorEventRunning || !waitingForInspectorSpawn)
        {
            return;
        }

        if (spawnedPrefab != inspectorPrefab)
        {
            return;
        }

        if (spawnedCustomer == null)
        {
            return;
        }

        activeInspector = spawnedCustomer;
        waitingForInspectorSpawn = false;

        Debug.Log("Inspector spawned: " + spawnedCustomer.gameObject.name);
    }

    // When Inspector leaves, the event will end
    private void HandleCustomerExit(CustomerStateMachine exitingCustomer)
    {
        if (!inspectorEventRunning)
        {
            return;
        }

        if (exitingCustomer != activeInspector)
        {
            return;
        }

        Debug.Log("The Inspector has exited.");

        activeInspector = null;

        eventSystem.CompleteEvent(HawkerEventType.Inspector);
    }

    private void HandleEventFinished(HawkerEventType eventType)
    {
        if (eventType != HawkerEventType.Inspector)
        {
            return;
        }

        Debug.Log("Inspector event finished.");

        StopInspectorEventWithoutCompletion();
    }

    private void StopInspectorEventWithoutCompletion()
    {
        inspectorEventRunning = false;
        waitingForInspectorSpawn = false;
        activeInspector = null;

        if (inspectorSpawnCoroutine != null)
        {
            StopCoroutine(inspectorSpawnCoroutine);
            inspectorSpawnCoroutine = null;
        }
    }
}
