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
    public bool orderChanged;
    public bool orderChangeInProgress;
    public bool subscribedToSpawner;

    // Coroutine reference to manage the order change process
    public Coroutine orderChangeCoroutine;

    private void OnEnable()
    {
        AIEventSystemScript.OnEventStarted += HandleEventStarted;
        AIEventSystemScript.OnEventFinished += HandleEventFinished;
    }

    private void OnDisable()
    {
        AIEventSystemScript.OnEventStarted -= HandleEventStarted;
        AIEventSystemScript.OnEventFinished -= HandleEventFinished;

        UnsubscribeFromCustomerSpawner();

        if (orderChangeCoroutine != null)
        {
            StopCoroutine(orderChangeCoroutine);
            orderChangeCoroutine = null;
        }

        eventRunning = false;
        orderChanged = false;
        orderChangeInProgress = false;
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

        if (eventSystem == null)
        {
            eventSystem = AIEventSystemScript.Instance;
        }

        if (customerSpawner == null && eventSystem != null)
        {
            customerSpawner = eventSystem.customerSpawner;
        }

        if (eventSystem == null || customerSpawner == null)
        {
            Debug.Log("FussyCustomerOrderEventScript is missing its event system or customer spawner reference.");

            return;
        }

        eventRunning = true;
        orderChanged = false;
        orderChangeInProgress = false;

        // Wait for the next customer spawned after the Fussy Customer event begins

        SubscribeToCustomerSpawner();

        Debug.Log("Fussy Customer event is waiting for the next spawned customer.");
    }

    // When Fussy Customer event is finished, either by completion or cancellation
    private void HandleEventFinished(HawkerEventType eventType)
    {
        if (eventType != HawkerEventType.FussyCustomer)
        {
            return;
        }

        UnsubscribeFromCustomerSpawner();

        if (orderChangeCoroutine != null)
        {
            StopCoroutine(orderChangeCoroutine);
            orderChangeCoroutine = null;
        }

        eventRunning = false;
        orderChanged = false;
        orderChangeInProgress = false;
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
        if (!eventRunning || orderChanged || orderChangeInProgress)
        {
            return;
        }

        if (spawnedCustomer == null)
        {
            return;
        }

        //Claim this customer and stop listening for other customers, to ensures that only one customer is changed

        UnsubscribeFromCustomerSpawner();

        orderChangeInProgress = true;

        orderChangeCoroutine = StartCoroutine(ChangeOrderAfterCustomerStarts(spawnedCustomer));
    }

    // Coroutine to change the customer's order after they have started their interaction
    private IEnumerator ChangeOrderAfterCustomerStarts(CustomerStateMachine spawnedCustomer)
    {

        // CustomerSpawnerScript invokes OnCustomerSpawned immediately after Instantiate()
        // waits one frame gives MealChecker.Start() time to call, SetMeal() and create the customer's original order

        yield return null;

        orderChangeCoroutine = null;

        if (!eventRunning || spawnedCustomer == null)
        {
            orderChangeInProgress = false;
            yield break;
        }

        MealChecker mealChecker = spawnedCustomer.GetComponent<MealChecker>();

        if (mealChecker == null)
        {
            mealChecker = spawnedCustomer.GetComponentInChildren<MealChecker>();
        }

        if (mealChecker == null)
        {
            Debug.Log("The spawned customer does not have a MealChecker.");

            orderChangeInProgress = false;

            // Wait for another spawned customer
            SubscribeToCustomerSpawner();

            yield break;
        }

        int timeInSeconds = Random.Range(15, 30);

        yield return new WaitForSeconds(timeInSeconds);

        // Attempt to change the customer's order to a different one
        bool changedSuccessfully = TryChangeToDifferentOrder(mealChecker);

        orderChangeInProgress = false;

        if (!changedSuccessfully)
        {
            // This customer had no alternative order
            // Continue waiting for another eligible customer.

            SubscribeToCustomerSpawner();
            yield break;
        }

        orderChanged = true;

        Debug.Log("The Fussy Customer changed their order to: " + mealChecker.mealToCheck.name);

        // Complete the Fussy Customer event
        if (completeEventAfterOrderChange && eventSystem != null)
        {
            eventSystem.CompleteEvent(HawkerEventType.FussyCustomer);
        }
    }

    // Attempt to change the customer's order to a different one from their possible orders
    private bool TryChangeToDifferentOrder(MealChecker mealChecker)
    {
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
