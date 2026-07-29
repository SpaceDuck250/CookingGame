using System;
using UnityEngine;

public class AIEventSystemScript : MonoBehaviour
{
    public static AIEventSystemScript Instance { get; private set; }

    public static event Action<HawkerEventType> OnEventStarted;
    public static event Action<HawkerEventType> OnEventFinished;

    public CustomerSpawnerScript customerSpawner;
    public AIEventDataScript eventData;

    // Event minimum parameters for Rush Hour, Fussy Customer, and Inspector
    public int minimumCustomersBeforeRushHour = 4;
    public int minimumDishesForFussyCustomer = 4;
    public int minimumFoodForInspector = 4;
    public float eventDuration = 30f;
    public float eventCooldown = 100f;
    public float checkInterval = 10f;

    // Event parameters for Fussy Customer
    public int fussyCustomerAmount = 2;

    // Event parameters for Served Dish within a time window
    public float servedDishWindow = 10f;

    // Event parameters for Inspector
    public int foodLyingAround;

    public HawkerEventType currentEvent = HawkerEventType.None;
    public float nextCheckTime;
    public float nextEventAllowedTime;

    public bool fussyCustomerEventArmed = true;
    public bool inspectorEventArmed = true;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        if (customerSpawner == null || eventData == null)
        {
            return;
        }

        RearmEvents();

        // Check for events at regular intervals
        if (Time.time < nextCheckTime)
        {
            return;
        }

        nextCheckTime = Time.time + checkInterval;

        if (currentEvent != HawkerEventType.None)
        {
            return;
        }

        // Check if the cooldown period has passed before allowing a new event
        if (Time.time < nextEventAllowedTime)
        {
            return;
        }

        CheckForEvent();
    }

    private void RearmEvents()
    {
        // Rearm the Fussy Customer event if the number of dishes served at once is below the minimum threshold
        if (eventData.DishesServedAtOnce < minimumDishesForFussyCustomer)
        {
            fussyCustomerEventArmed = true;
        }

        // Rearm the Inspector event if the amount of food lying around is below the minimum threshold
        if (eventData.FoodLyingAround < minimumFoodForInspector)
        {
            inspectorEventArmed = true;
        }
    }

    private void CheckForEvent()
    {
        // First decision in the eveet list, the RushHour
        if (customerSpawner.ActiveCustomerCount < minimumCustomersBeforeRushHour)
        {
            StartEvent(HawkerEventType.RushHour);
            return;
        }

        // Second decision in the eveet list, the FussyCustomer
        if (fussyCustomerEventArmed && eventData.DishesServedAtOnce >= minimumDishesForFussyCustomer)
        {
            fussyCustomerEventArmed = false;

            // Prevents the same dish burst from triggering again
            eventData.ClearServedDishData();

            StartEvent(HawkerEventType.FussyCustomer);
            return;
        }

        // Third decision in the eveet list, the Inspector
        if (inspectorEventArmed && eventData.FoodLyingAround >= minimumFoodForInspector)
        {
            inspectorEventArmed = false;

            StartEvent(HawkerEventType.Inspector);
        }
    }

    private void StartEvent(HawkerEventType eventType)
    {
        if (currentEvent != HawkerEventType.None)
        {
            return;
        }

        currentEvent = eventType;

        Debug.Log("Started Hawker Event: " + eventType);

        OnEventStarted?.Invoke(eventType);
    }

    // Checks if the current event is the same as the one being completed
    // Completes it and sets the next event allowed time
    public void CompleteEvent(HawkerEventType eventType)
    {
        if (currentEvent != eventType)
        {
            return;
        }

        Debug.Log("Finished Hawker Event: " + eventType);

        currentEvent = HawkerEventType.None;
        nextEventAllowedTime = Time.time + eventCooldown;

        OnEventFinished?.Invoke(eventType);
    }

    // Testing purposes
    [ContextMenu("Test Fussy Customer Event")]
    public void TestFussyCustomerEvent()
    {
        if (currentEvent != HawkerEventType.None)
        {
            Debug.Log("Cannot test Fussy Customer because another event is active.");

            return;
        }

        StartEvent(HawkerEventType.FussyCustomer);
    }
}
