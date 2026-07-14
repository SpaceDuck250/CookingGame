using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Customer;

// If want to use these enums in other scripts put "using Customer" at the top of the script
namespace Customer
{
    public enum CustomerState
    {
        WalkingToCounter,
        WalkingToSeat,
        Seated,
        WaitingForFood,
        PayingForFood,
        ReturningTray,
        LeavingMap,
    }

    // Depending on the mood 
    public enum CustomerMood
    {
        Normal,
        Happy,
        Angry
    }
}

public class CustomerStateMachine : MonoBehaviour
{
    // The Order date are set in the inspector or by a spawner
    // and the customer will choose one of these meals to order
    // So need make prefabs for the different meal and need to
    // randomise the prefabs to spawn in the spawner script

    public NavMeshAgent agent;
    public Animator animator;

    // Customer profile scriptable object set
    public CustomerData profile;
    public CustomerMovementScript movementScript;
    public MealChecker mealChecker;
    public CustomerAnimator customerAnimator;
    public CustomerInteractScript interactScript;

    // Points for the customer to move to
    public Transform counterPoint;
    public Transform seatPoint;
    public Transform exitPoint;
    public Transform trayReturnPoint;

    // Time the customer will wait before leaving
    public float maxWaitTime = 30f;
    public float waitTimer;
    public bool canRunTimer = false;

    public CustomerState currentState;
    public CustomerMood currentMood;

    public Action<CustomerState> OnCustomerChangeState;
    public Action<CustomerMood> OnCustomerMoodChange;

    public bool orderDone = false;
    public bool sitting = false;

    public Vector3 normalTrayLocalPosition;
    public Vector3 seatedTrayLocalPosition;

    public float upOffsetChair;
    public float forwardOffset;

    // Track if this customer has already used their "return" chance
    private bool hasReturnedOnce = false;

    private void Awake()
    {
        OnCustomerChangeState += ChangeCustomerState;
        OnCustomerMoodChange += ChangeCustomerMood;
    }

    void Start()
    {
        ApplyProfile();
        hasReturnedOnce = false;
        OnCustomerMoodChange?.Invoke(CustomerMood.Normal);

    }

    private void OnDestroy()
    {
        OnCustomerChangeState -= ChangeCustomerState;
        OnCustomerMoodChange -= ChangeCustomerMood;
    }

    private void Update()
    {
        TryUpdateWaitingTimer();
    }

    public void ChangeCustomerState(CustomerState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case CustomerState.WalkingToCounter:
                GoToCounter();
                break;

            case CustomerState.WaitingForFood:
                StartWaitingForOrder();
                break;

            case CustomerState.PayingForFood:
                SetupPay();
                break;

            case CustomerState.WalkingToSeat:
                GoToSeat();
                break;

            case CustomerState.Seated:
                TakeSeat();
                break;

            case CustomerState.ReturningTray:
                ReturnTray();
                break;

            case CustomerState.LeavingMap:
                LeaveMap();
                break;

        }
    }

    public void ChangeCustomerMood(CustomerMood newMood)
    {
        currentMood = newMood;
    }

    // Applies the customer profile to the customer state machine
    private void ApplyProfile()
    {
        if (profile == null)
        {
            return;
        }

        maxWaitTime = profile.waitTime;
        interactScript.heldCustomerData = profile;
    }

    // Customer sets counter as destination and walks to it
    private void GoToCounter()
    {
        if (counterPoint == null)
        {
            Debug.Log("Customer has no counterPoint.");
            return;
        }

        print("here3");

        movementScript.OnNewDestinationChange?.Invoke(counterPoint);
    }

    // Customer waits at the counter for the player to take their order
    private void StartWaitingForOrder()
    {
        StartWaitTimer();
    }

    private void SetupPay()
    {
        StopTimer();
        MoneyManager.OnPayForOrder?.Invoke(mealChecker.mealToCheck, currentMood, profile);

        // Determine mood depending on the timer value
    }

    private void GoToSeat()
    {
        movementScript.OnNewDestinationChange?.Invoke(seatPoint);
    }

    private void TakeSeat()
    {
        movementScript.destinationPoint = null;
        customerAnimator.Sit();
        sitting = true;
    }

    private void ReturnTray()
    {
        movementScript.OnNewDestinationChange?.Invoke(trayReturnPoint);
    }

    private void LeaveMap()
    {
        // Special behaviour: Uncle Fedrick has a 30% chance to return and order one more meal.
        // Only allow this to happen once per customer instance.
        if (!hasReturnedOnce && profile != null && profile.customerName == "Uncle Fedrick")
        {
            print("Uncle Fedrick has a chance to return!");
            float chance = UnityEngine.Random.value; // 0.0 - 1.0

            if (chance <= 0.3f)
            {
                hasReturnedOnce = true;

                // Reset relevant state so the customer effectively returns to the counter and orders again
                orderDone = false;
                sitting = false;

                if (interactScript != null)
                {
                    interactScript.orderComplete = false;
                    interactScript.TalkedTo = false;
                    interactScript.finishedInteract = false;
                }

                if (movementScript != null)
                {
                    movementScript.orderDone = false;
                    movementScript.sitting = false;
                    movementScript.holdingTray = false;
                }

                // Choose a new meal for the customer
                if (mealChecker != null)
                {
                    mealChecker.SetMeal();
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
                    // The change state to walking to counter
                    counterPoint = freeQueue;
                    seatPoint = freeChair;
                    OnCustomerChangeState?.Invoke(CustomerState.WalkingToCounter);
                    print("Uncle Fedrick is returning to counter.");
                    return;
                }
                else
                {
                    // No free queue point available, fallback to leaving map
                    Debug.Log("No free queue point available for return, sending to exit.");
                    movementScript.OnNewDestinationChange?.Invoke(exitPoint);
                    return;
                }
            }
        }

        // Default leave behaviour (either not Fedrick, already returned, or chance failed)
        movementScript.OnNewDestinationChange?.Invoke(exitPoint);
    }

    public void StartWaitTimer()
    {
        canRunTimer = true;
    }

    public void StopTimer()
    {
        canRunTimer = false;

    }

    public void TryUpdateWaitingTimer()
    {
        if (!canRunTimer)
        {
            return;
        }

        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
        {
            canRunTimer = false;
            waitTimer = 0f;

            OnCustomerMoodChange?.Invoke(CustomerMood.Angry);
        }
    }


}
