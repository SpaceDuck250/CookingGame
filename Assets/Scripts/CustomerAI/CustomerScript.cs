using UnityEngine;
using UnityEngine.AI;
using System;

public class CustomerScript : Interactable
{
    public MealData orderData;

    public Transform stallTransform;
    public Transform tableTransform;
    public Transform exitTransform;

    public float sitTime;

    public bool hasOrdered = false;
    public bool hasBeenServed = false;

    private bool isHeadingToStall = false;
    private bool hasReachedStall = false;
    private bool hasReachedTable = false;
    private bool isHeadingToExit = false;

    private float sitTimer;

    public Action<CustomerScript> OnCustomerOrdered;
    public Action<CustomerScript> OnCustomerServed;
    public Action<CustomerScript> OnCustomerLeft;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckIfReachedStall();
        CheckIfReachedTable();
        CheckIfReachedExit();
    }
    public void WalkToQueuePosition(Transform queuePosition)
    {
        agent.isStopped = false;
        agent.SetDestination(queuePosition.position);
    }
    public void WalkToStall()
    {
        if (stallTransform == null)
        {
            return;
        }

        isHeadingToStall = true;
        agent.isStopped = false;
        agent.SetDestination(stallTransform.position);
    }

    private void WalkToTable()
    {
        if (tableTransform == null)
        {
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(tableTransform.position);
    }

    private void WalkToExit()
    {
        if (exitTransform == null)
        {
            return;
        }

        isHeadingToExit = true;
        agent.isStopped = false;
        agent.SetDestination(exitTransform.position);
    }

    private void CheckIfReachedStall()
    {
        if (!isHeadingToStall || hasReachedStall)
        {
            return;
        }

        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            hasReachedStall = true;
            isHeadingToStall = false;
            agent.isStopped = true;
        }
    }

    private void CheckIfReachedTable()
    {
        if (!hasBeenServed || hasReachedTable)
        {
            return;
        }

        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            hasReachedTable = true;
            agent.isStopped = true;
        }
    }

    private void CheckIfReachedExit()
    {
        if (!isHeadingToExit)
        {
            return;
        }

        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            OnCustomerLeft?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (hasBeenServed)
        {
            return;
        }

        if (!hasOrdered)
        {
            TakeOrder();
            return;
        }

        TryServeFood(playerHand);
    }

    private void TakeOrder()
    {
        if (!hasReachedStall)
        {
            return;
        }

        hasOrdered = true;

        print("Customer ordered: " + orderData.name);

        OnCustomerOrdered?.Invoke(this);
    }

    private void TryServeFood(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeld == null)
        {
            return;
        }

        if (!CheckIfFoodMatchesOrder(playerHand.currentFoodHeld))
        {
            print("Wrong food!");
            return;
        }

        playerHand.currentFoodHeld = null;
        Destroy(playerHand.currentFoodHeldObj);
        playerHand.currentFoodHeldObj = null;

        WalkToTable();
        OnCustomerServed?.Invoke(this);

        hasBeenServed = true;
        StartCoroutine(SitThenLeave());
    }

    private System.Collections.IEnumerator SitThenLeave()
    {
        yield return new WaitUntil(() => hasReachedTable);

        yield return new WaitForSeconds(sitTime);

        WalkToExit();
    }

    private bool CheckIfFoodMatchesOrder(FoodData foodData)
    {
        foreach (FoodData ingredient in orderData.foodIngredients)
        {
            if (ingredient == foodData)
            {
                return true;
            }
        }

        return false;
    }
}