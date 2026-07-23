using System.Collections;
using System.Collections.Generic;
using Customer;
using UnityEngine;

public class SpecialCustomerEventScript : MonoBehaviour
{
    public AIEventSystemScript eventSystem;
    public CustomerSpawnerScript customerSpawner;

    // The prefab of the special customer to spawn during the event
    public GameObject specialCustomerPrefab;
    public int customerAmount = 1;
    public float spawnDelay = 5f;
    public float maximumEventDuration = 120f;

    // Tracks the special customers spawned by this event that are still active in the scene
    private readonly HashSet<CustomerStateMachine> activeSpecialCustomers = new HashSet<CustomerStateMachine>();
    public int spawnedCustomerCount;
    public bool eventRunning;
    public bool spawningFinished;

    public Coroutine spawnRoutine;
    public Coroutine timeoutRoutine;

    public HawkerEventType eventType;

    private void OnEnable()
    {
        AIEventSystemScript.OnEventStarted += HandleEventStarted;

        if (customerSpawner != null)
        {
            customerSpawner.OnCustomerSpawned += HandleCustomerSpawned;
        }

        CustomerSpawnerScript.OnCustomerExit += HandleCustomerExit;
    }

    private void OnDisable()
    {
        AIEventSystemScript.OnEventStarted -= HandleEventStarted;

        if (customerSpawner != null)
        {
            customerSpawner.OnCustomerSpawned -= HandleCustomerSpawned;
        }

        CustomerSpawnerScript.OnCustomerExit -= HandleCustomerExit;

        StopEventWithoutCompletion();
    }

    // The event starts when the AIEventSystemScript triggers the OnEventStarted event
    private void HandleEventStarted(HawkerEventType startedEvent)
    {
        if (startedEvent != eventType)
        {
            return;
        }

        if (eventRunning)
        {
            return;
        }

        // The event type must be either FussyCustomer or Inspector for this script to function correctly
        if (eventType != HawkerEventType.FussyCustomer && eventType != HawkerEventType.Inspector)
        {
            Debug.Log(name + " must use FussyCustomer or Inspector.");

            eventSystem.CompleteEvent(startedEvent);
            return;
        }

        // Check for missing references in the Inspector
        if (eventSystem == null || customerSpawner == null || specialCustomerPrefab == null)
        {
            Debug.LogError(name + " is missing an Inspector reference.");

            if (eventSystem != null)
            {
                eventSystem.CompleteEvent(startedEvent);
            }

            return;
        }

        // Reset event state
        eventRunning = true;
        spawningFinished = false;
        spawnedCustomerCount = 0;

        activeSpecialCustomers.Clear();

        spawnRoutine = StartCoroutine(SpawnSpecialCustomers());

        timeoutRoutine = StartCoroutine(EventTimeout());
    }

    // Coroutine that spawns special customers at intervals until the specified amount is reached or the event ends
    private IEnumerator SpawnSpecialCustomers()
    {
        while (eventRunning && spawnedCustomerCount < customerAmount)
        {
            Transform freeChair = customerSpawner.GetFreeChair();

            Transform freeQueuePoint = customerSpawner.GetFreeQueuePoint();

            if (freeChair != null && freeQueuePoint != null)
            {
                customerSpawner.SpawnCustomer(freeChair, freeQueuePoint, false, specialCustomerPrefab);
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        spawnRoutine = null;
        spawningFinished = true;

        TryFinishEvent();
    }

    // This method is called whenever a customer is spawned during the special customer event
    private void HandleCustomerSpawned(CustomerStateMachine customer, GameObject spawnedPrefab)
    {
        if (!eventRunning)
        {
            return;
        }

        // CustomerSpawnerScript passes the original prefab through
        // OnCustomerSpawned so it can be compared directly
        if (spawnedPrefab != specialCustomerPrefab)
        {
            return;
        }

        if (activeSpecialCustomers.Add(customer))
        {
            spawnedCustomerCount++;

            Debug.Log(eventType + " customers spawned: " + spawnedCustomerCount + "/" + customerAmount);
        }
    }

    private void HandleCustomerExit(CustomerStateMachine customer)
    {
        if (!eventRunning)
        {
            return;
        }

        if (!activeSpecialCustomers.Remove(customer))
        {
            return;
        }

        TryFinishEvent();
    }

    private void TryFinishEvent()
    {
        if (!eventRunning)
        {
            return;
        }

        if (!spawningFinished)
        {
            return;
        }

        if (activeSpecialCustomers.Count > 0)
        {
            return;
        }

        FinishEvent();
    }

    private IEnumerator EventTimeout()
    {
        yield return new WaitForSeconds(maximumEventDuration);

        timeoutRoutine = null;

        Debug.Log(eventType +" reached its maximum duration.");

        FinishEvent();
    }

    private void FinishEvent()
    {
        if (!eventRunning)
        {
            return;
        }

        eventRunning = false;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        if (timeoutRoutine != null)
        {
            StopCoroutine(timeoutRoutine);
            timeoutRoutine = null;
        }

        activeSpecialCustomers.Clear();

        eventSystem.CompleteEvent(eventType);
    }

    private void StopEventWithoutCompletion()
    {
        eventRunning = false;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        if (timeoutRoutine != null)
        {
            StopCoroutine(timeoutRoutine);
            timeoutRoutine = null;
        }

        activeSpecialCustomers.Clear();
    }
}
