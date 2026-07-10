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

    public bool orderDone = false;
    public bool sitting = false;

    public Vector3 normalTrayLocalPosition;
    public Vector3 seatedTrayLocalPosition;

    private void Awake()
    {
        OnCustomerChangeState += ChangeCustomerState;
    }

    void Start()
    {
        ApplyProfile();

        currentMood = CustomerMood.Normal;

    }

    private void OnDestroy()
    {
        OnCustomerChangeState -= ChangeCustomerState;
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

    // Still working on this
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

            currentMood = CustomerMood.Angry;
        }
    }

    //private void LeaveHappy(int payment)
    //{
    //    //GivePlayerMoney(payment);

    //    currentState = CustomerState.LeavingHappy;

    //    if (exitPoint == null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    agent.isStopped = false;
    //    agent.SetDestination(exitPoint.position);
    //}

    // Customer leaves angry and gives the player partial payment
    //private void LeaveAngry(int payment)
    //{
    //    //GivePlayerMoney(payment);

    //    currentState = CustomerState.LeavingAngry;

    //    if (exitPoint == null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    agent.isStopped = false;
    //    agent.SetDestination(exitPoint.position);
    //}


    //public void SetPreferences(MealData[] prefs)
    //{
    //    preferredMeals = prefs;
    //}


}
