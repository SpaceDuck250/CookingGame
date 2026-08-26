using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FussyCustomerOrderEventScript : MonoBehaviour
{
    public AIEventSystemScript eventSystem;
    public CustomerSpawnerScript customerSpawner;

    // Complete the Fussy Customer event immediately after the order changes
    public bool completeEventAfterOrderChange = true;

    // Flags to track the state of the event and order change
    public bool eventRunning;
    public bool subscribedToSpawner;

    // Every customer that spawns while the Fussy Customer event is active is stored here
    // They remain selected until the player talks to them even if the Fussy Customer event has already finished
    public List<CustomerStateMachine> selectedCustomers = new List<CustomerStateMachine>();

    // Customers that have already had their first conversation,
    // their order will change the next time the player talks to them
    public List<CustomerStateMachine> customersTalkedToOnce = new List<CustomerStateMachine>();

    // Coroutine reference to manage the order change process
    public Coroutine orderChangeCoroutine;

    private void OnEnable()
    {
        AIEventSystemScript.OnEventStarted += HandleEventStarted;
        AIEventSystemScript.OnEventFinished += HandleEventFinished;

        // CustomerInteractScript invokes this when the player opens a conversation with any customer
        CustomerInteractScript.OnAnyCustomerInteract += HandleCustomerInteract;
        CustomerSpawnerScript.OnCustomerExit += HandleCustomerExit;
    }

    private void OnDisable()
    {
        AIEventSystemScript.OnEventStarted -= HandleEventStarted;
        AIEventSystemScript.OnEventFinished -= HandleEventFinished;

        CustomerInteractScript.OnAnyCustomerInteract -= HandleCustomerInteract;
        CustomerSpawnerScript.OnCustomerExit -= HandleCustomerExit;

        UnsubscribeFromCustomerSpawner();

        eventRunning = false;

        selectedCustomers.Clear();
        customersTalkedToOnce.Clear();
    }

    private void ResolveReferences()
    {
        if (eventSystem == null)
        {
            eventSystem = AIEventSystemScript.Instance;
        }

        if (customerSpawner == null && eventSystem != null)
        {
            customerSpawner = eventSystem.customerSpawner;
        }

        if (customerSpawner == null)
        {
            customerSpawner = CustomerSpawnerScript.instance;
        }
    }

    private void HandleEventStarted(HawkerEventType eventType)
    {
        if (eventType != HawkerEventType.FussyCustomer)
        {
            return;
        }

        if (eventRunning)
        {
            return;
        }

        ResolveReferences();

        if (eventSystem == null || customerSpawner == null)
        {
            Debug.Log("FussyCustomerOrderEventScript is missing its event system or customer spawner reference.");
            return;
        }

        eventRunning = true;

        SubscribeToCustomerSpawner();

        Debug.Log("Fussy Customer event started. All customers spawned during the event will become Fussy Customers.");
    }

    // When Fussy Customer event is finished, either by completion or cancellation
    private void HandleEventFinished(HawkerEventType eventType)
    {
        if (eventType != HawkerEventType.FussyCustomer)
        {
            return;
        }

        // Stop selecting NEW customers after the effect finish. But Customers that spawned during the event should
        // still change their order when the player talks to them later
        eventRunning = false;

        UnsubscribeFromCustomerSpawner();

        Debug.Log($"Fussy Customer event finished selecting customers. {selectedCustomers.Count} selected customers are still waiting to be talked to.");
    }

    // Subscribe to the customer spawner's OnCustomerSpawned event
    private void SubscribeToCustomerSpawner()
    {
        if (subscribedToSpawner || customerSpawner == null)
        {
            return;
        }

        customerSpawner.OnCustomerSpawned += HandleCustomerSpawned;

        subscribedToSpawner = true;
    }

    // Unsubscribe from the customer spawner's OnCustomerSpawned event
    private void UnsubscribeFromCustomerSpawner()
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
    }

    // When a customer is spawned, check if the Fussy Customer event is running and if the order can be changed
    private void HandleCustomerSpawned(CustomerStateMachine spawnedCustomer, GameObject spawnedPrefab)
    {
        if (!eventRunning)
        {
            return;
        }

        if (spawnedCustomer == null)
        {
            return;
        }

        // Aunt Merry has her own change-of-mind behaviour,
        // so do not make her a Fussy Customer
        AuntMerryCustomerScript auntMerryScript = spawnedCustomer.GetComponent<AuntMerryCustomerScript>();

        if (auntMerryScript == null)
        {
            auntMerryScript = spawnedCustomer.GetComponentInChildren<AuntMerryCustomerScript>();
        }

        if (auntMerryScript != null)
        {
            Debug.Log("Aunt Merry spawned during the Fussy Customer event, but she was ignored because she has her own order change behaviour.");
            return;
        }

        // Prevent duplicate entries.
        if (selectedCustomers.Contains(spawnedCustomer))
        {
            return;
        }

        //Claim this customer and stop listening for other customers, to ensures that only one customer is changed

        selectedCustomers.Add(spawnedCustomer);

        Debug.Log($"Customer selected for Fussy Customer event: {spawnedCustomer.gameObject.name}");
    }

    // Called whenever the player opens a conversation with a customer
    private void HandleCustomerInteract(CustomerStateMachine customer)
    {
        if (customer == null)
        {
            return;
        }

        // Ignore normal customers as only customers that spawned during the
        // Fussy Customer event are allowed through
        if (!selectedCustomers.Contains(customer))
        {
            return;
        }

        // The first conversation, does not change the order yet
        if (!customersTalkedToOnce.Contains(customer))
        {
            customersTalkedToOnce.Add(customer);

            Debug.Log($"{customer.gameObject.name} gave the player their original order. They may change their mind on the next conversation.");
            return;
        }

        // The second conversation, the player knows the original order
        // then the customer changes their mind
        MealChecker mealChecker = customer.GetComponent<MealChecker>();

        if (mealChecker == null)
        {
            mealChecker = customer.GetComponentInChildren<MealChecker>();
        }

        if (mealChecker == null)
        {
            Debug.Log("The selected Fussy Customer does not have a MealChecker.");
            selectedCustomers.Remove(customer);
            return;
        }

        // OnAnyCustomerInteract is invoked before NpcDialogueScript displays mealToCheck
        // so the player sees the new order immediately
        MealData oldOrder = mealChecker.mealToCheck;
        bool changedSuccessfully = TryChangeToDifferentOrder(mealChecker);

        // This customer has now used their Fussy Customer change
        // Remove them so talking again does not repeatedly change their meal
        selectedCustomers.Remove(customer);

        if (!changedSuccessfully)
        {
            Debug.Log("The Fussy Customer had no alternative meal to change to.");
            return;
        }

        string oldOrderName = oldOrder != null ? oldOrder.name : "None";

        string newOrderName = mealChecker.mealToCheck != null ? mealChecker.mealToCheck.name : "None";

        Debug.Log($"The Fussy Customer changed their order from {oldOrderName} to {newOrderName}.");
    }

    private void RemoveFussyCustomer(CustomerStateMachine customer)
    {
        selectedCustomers.Remove(customer);
        customersTalkedToOnce.Remove(customer);
    }

    // Remove customers that leave before the player gets a chance to talk to them
    private void HandleCustomerExit(CustomerStateMachine exitingCustomer)
    {
        if (exitingCustomer == null)
        {
            return;
        }

        if (!selectedCustomers.Contains(exitingCustomer))
        {
            return;
        }

        RemoveFussyCustomer(exitingCustomer);

        Debug.Log("A selected Fussy Customer left before finishing their change-of-mind behaviour.");
    }

    // Attempt to change the customer's order to a different one from their possible orders
    private bool TryChangeToDifferentOrder(MealChecker mealChecker)
    {
        if (mealChecker == null)
        {
            Debug.Log("MealChecker is null.");
            return false;
        }

        if (mealChecker.customerScript == null)
        {
            Debug.Log("MealChecker does not have a CustomerInteractScript.");
            return false;
        }

        if (mealChecker.customerScript.heldCustomerData == null)
        {
            Debug.Log("The customer does not have CustomerData.");
            return false;
        }

        // Get the list of possible meal orders for the customer
        List<MealData> possibleOrders = mealChecker.customerScript.heldCustomerData.possibleMealOrders;

        if (possibleOrders == null || possibleOrders.Count == 0)
        {
            Debug.Log("The customer has no possible meal orders.");
            return false;
        }

        // Get the current order of the customer
        MealData currentOrder = mealChecker.mealToCheck;

        // Create a list of alternative orders that are different from the current order
        List<MealData> alternativeOrders = new List<MealData>();

        foreach (MealData possibleOrder in possibleOrders)
        {
            if (possibleOrder == null)
            {
                continue;
            }

            // Do not allow the customer to choose the same order.
            if (possibleOrder == currentOrder)
            {
                continue;
            }

            // Add the possible order to the list of alternative orders
            alternativeOrders.Add(possibleOrder);
        }

        // Should not be possible, but if the customer has no alternative orders, log a message and return false
        if (alternativeOrders.Count == 0)
        {
            Debug.Log("The customer has no order different from their current order.");
            return false;
        }

        // Randomly select one of the alternative orders and set it as the customer's new order
        int randomIndex = UnityEngine.Random.Range(0, alternativeOrders.Count);

        // Set the new order in the MealChecker
        MealData newOrder = alternativeOrders[randomIndex];

        // Update the MealChecker's mealToCheck to the new order
        mealChecker.mealToCheck = newOrder;

        return true;
    }
}
