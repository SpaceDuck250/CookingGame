using System.Collections;
using UnityEngine;

public class RushHourEventScript : MonoBehaviour
{
    public AIEventSystemScript eventSystem;
    public CustomerSpawnerScript customerSpawner;

    // The number of customers to spawn and duration during the Rush Hour event
    public int customersToSpawn = 4;
    public int spawnedCustomerCount;
    public float maximumDuration = 60f;
    public bool eventRunning;
    public Coroutine timeoutRoutine;

    private void OnEnable()
    {
        AIEventSystemScript.OnEventStarted += HandleEventStarted;
    }

    private void OnDisable()
    {
        AIEventSystemScript.OnEventStarted -= HandleEventStarted;

        StopEventWithoutCompletion();
    }

    // Rush Hour event starts when the AIEventSystemScript triggers the OnEventStarted event
    private void HandleEventStarted(HawkerEventType eventType)
    {
        if (eventType != HawkerEventType.RushHour)
        {
            return;
        }

        if (eventRunning)
        {
            return;
        }

        if (eventSystem == null || customerSpawner == null)
        {
            Debug.Log("Rush Hour Event is missing an Inspector reference.");

            return;
        }

        eventRunning = true;
        spawnedCustomerCount = 0;

        // Subscribe to the existing event in CustomerSpawnerScript.
        customerSpawner.OnCustomerSpawned += HandleCustomerSpawned;

        // The existing spawner handles the 2f to 8f interval.
        customerSpawner.eventRushHour = true;

        if (customersToSpawn <= 0)
        {
            FinishEvent();
            return;
        }

        timeoutRoutine = StartCoroutine(EventTimeout());
    }

    // This method is called whenever a customer is spawned during the Rush Hour event or other events
    private void HandleCustomerSpawned(CustomerStateMachine customer, GameObject spawnedPrefab)
    {
        if (!eventRunning)
        {
            return;
        }

        spawnedCustomerCount++;

        //Debug.Log("Rush Hour customers spawned: " + spawnedCustomerCount + "/" + customersToSpawn);

        if (spawnedCustomerCount >= customersToSpawn)
        {
            FinishEvent();
        }
    }

    // Coroutine to handle the maximum duration of the Rush Hour event use to time the event
    private IEnumerator EventTimeout()
    {
        yield return new WaitForSeconds(maximumDuration);

        timeoutRoutine = null;

        Debug.LogWarning("Rush Hour reached its maximum duration.");

        FinishEvent();
    }

    // This method is called when the Rush Hour event is completed
    // either by reaching the required number of spawned customers or by reaching the maximum duration
    // or whatever we decide to use to end the event
    private void FinishEvent()
    {
        if (!eventRunning)
        {
            return;
        }

        eventRunning = false;

        // The existing spawner handles the 15f to 35f interval.
        customerSpawner.eventRushHour = false;
        spawnedCustomerCount = 0;

        // Unsubscribe to the existing event in CustomerSpawnerScript.
        customerSpawner.OnCustomerSpawned -= HandleCustomerSpawned;

        if (timeoutRoutine != null)
        {
            StopCoroutine(timeoutRoutine);
            timeoutRoutine = null;
        }

        eventSystem.CompleteEvent(HawkerEventType.RushHour);
    }

    // This method is called when the Rush Hour event is stopped without completion
    private void StopEventWithoutCompletion()
    {
        // The existing spawner handles the 15f to 35f interval.
        if (customerSpawner != null)
        {
            customerSpawner.eventRushHour = false;
            customerSpawner.OnCustomerSpawned -= HandleCustomerSpawned;
        }

        // Stop the timeout coroutine if it's running
        if (timeoutRoutine != null)
        {
            StopCoroutine(timeoutRoutine);
            timeoutRoutine = null;
        }

        eventRunning = false;
    }
}
