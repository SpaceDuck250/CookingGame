using System;
using UnityEngine;
using Customer;

[RequireComponent(typeof(CustomerStateMachine))]
public class UncleFedrickHandler : MonoBehaviour
{
    private CustomerStateMachine stateMachine;
    private bool hasReturnedOnce = false;

    // Enable Uncle Fedrick special return behaviour for this customer
    public bool uncleFedrickReturn = true;

    // Range 0f - 1f
    public float returnChance = 0.3f;

    private void Awake()
    {
        stateMachine = GetComponent<CustomerStateMachine>();

        if (stateMachine != null)
        {
            stateMachine.OnCustomerChangeState += OnCustomerStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (stateMachine != null)
        {
            stateMachine.OnCustomerChangeState -= OnCustomerStateChanged;
        }
    }

    private void OnCustomerStateChanged(Customer.CustomerState newState)
    {
        if (newState != Customer.CustomerState.LeavingMap)
        {
            return;
        }

        TryHandleReturn();
    }

    private void TryHandleReturn()
    {
        if (hasReturnedOnce || stateMachine == null || stateMachine.profile == null)
        {
            return;
        }

        if (stateMachine.profile.customerName != "Uncle Fedrick")
        {
            return;
        }

        float chance = UnityEngine.Random.value; // 0.0 - 1.0

        // allow default leaving behaviour
        if (!uncleFedrickReturn || chance > returnChance)
        {
            return;
        }

        hasReturnedOnce = true;

        // Reset relevant state so the customer effectively returns to the counter and orders again
        stateMachine.orderDone = false;
        stateMachine.sitting = false;

        if (stateMachine.interactScript != null)
        {
            stateMachine.interactScript.orderComplete = false;
            stateMachine.interactScript.talkedTo = false;
            stateMachine.interactScript.finishedInteract = false;
        }

        if (stateMachine.movementScript != null)
        {
            stateMachine.movementScript.orderDone = false;
            stateMachine.movementScript.sitting = false;
            stateMachine.movementScript.holdingTray = false;
        }

        // Choose a new meal for the customer
        if (stateMachine.mealChecker != null)
        {
            stateMachine.mealChecker.SetMeal();
        }

        // Request a free queue point from the spawner and set counterPoint before walking
        Transform freeQueue = null;
        Transform freeChair = null;

        if (CustomerSpawnerScript.instance != null)
        {
            freeQueue = CustomerSpawnerScript.instance.GetFreeQueuePoint();
            freeChair = CustomerSpawnerScript.instance.GetFreeChair();
        }

        if (freeQueue != null && freeChair != null)
        {
            stateMachine.queuePoint = freeQueue;
            stateMachine.seatPoint = freeChair;

            // Change state back to walking to counter
            stateMachine.OnCustomerChangeState?.Invoke(Customer.CustomerState.WalkingToCounter);
            Debug.Log("Uncle Fedrick is returning to counter.");
            return;
        }
        else
        {
            // No free queue point available, fallback to exit destination
            if (stateMachine.movementScript != null)
            {
                stateMachine.movementScript.OnNewDestinationChange?.Invoke(stateMachine.exitPoint);
            }
            else
            {
                stateMachine.OnCustomerChangeState?.Invoke(Customer.CustomerState.LeavingMap);
            }
            Debug.Log("No free queue point available for Uncle Fedrick return, sending to exit.");
        }
    }
}
