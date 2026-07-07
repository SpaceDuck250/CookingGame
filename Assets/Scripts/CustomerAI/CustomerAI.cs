using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    // The Order date are set in the inspector or by a spawner
    // and the customer will choose one of these meals to order
    // So need make prefabs for the different meal and need to
    // randomise the prefabs to spawn in the spawner script

    public enum CustomerState
    {
        WalkingToCounter,
        WaitingToOrder,
        WalkingToSeat,
        WaitingForFood,
        LeavingHappy,
        LeavingAngry
    }

    public NavMeshAgent agent;
    public Animator animator;

    // Customer profile scriptable object set
    public CustomerProfileScript profile;

    // Points for the customer to move to
    public Transform counterPoint;
    public Transform seatPoint;
    public Transform exitPoint;

    // Set in inspector or by spawner
    // Order data for the customer
    public MealData[] possibleOrders;
    public List<MealData> wantedOrders = new List<MealData>();
    public List<MealData> servedOrders = new List<MealData>();
    public int mealRequired = 1;
    public int timeoutPay = 0;

    // Set in inspector or by spawner
    // If non-empty, the customer will order from this list instead of `possibleOrders
    public MealData[] preferredMeals;

    // Time the customer will wait before leaving
    public float maxWaitTime = 30f;
    private float waitTimer;

    // Payment amounts
    public int fullPay = 20;
    public int wrongOrderPay = 8;

    public CustomerState currentState;

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        ApplyProfile();
        GoToCounter();
    }

    void Update()
    {
        // Mostly just for animation purposes, but could be used for other things too
        UpdateAnimation();

        switch (currentState)
        {
            case CustomerState.WalkingToCounter:
                if (HasArrived())
                {
                    StartWaitingForOrder();
                }
                break;

            case CustomerState.WaitingForFood:
                UpdateWaitingTimer();
                break;

            case CustomerState.WalkingToSeat:
                if (HasArrived())
                {

                    SitAndWaitForFood();
                }
                break;

            case CustomerState.LeavingHappy:
            case CustomerState.LeavingAngry:
                if (HasArrived())
                {
                    Destroy(gameObject);
                }
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

        // Check if the profile has a favorite meal
        // and if so, set it as the preferred meal
        maxWaitTime = profile.maxWaitTime;
        fullPay = profile.fullPayment;
        wrongOrderPay = profile.wrongOrderPayment;
        timeoutPay = profile.timeoutPayment;
        mealRequired = profile.mealRequired;

        if (profile.favoriteMeal != null && profile.favoriteMeal.Length > 0)
        {
            preferredMeals = profile.favoriteMeal;
        }
    }

    // Customer sets counter as destination and walks to it
    private void GoToCounter()
    {
        if (counterPoint == null)
        {
            Debug.Log("Customer has no counterPoint.");
            return;
        }

        currentState = CustomerState.WalkingToCounter;
        agent.isStopped = false;
        agent.SetDestination(counterPoint.position);
    }

    // Customer waits at the counter for the player to take their order
    private void StartWaitingForOrder()
    {
        currentState = CustomerState.WaitingToOrder;
        agent.isStopped = true;
        agent.ResetPath();
    }

    // Player takes the customer's order and the customer walks to their seat
    public void TakeOrder()
    {
        if (currentState != CustomerState.WaitingToOrder)
        {
            return;
        }

        wantedOrders.Clear();
        servedOrders.Clear();

        for (int i = 0; i < mealRequired; i++)
        {
            MealData chosenMeal = ChooseOrder();

            if (chosenMeal != null)
            {
                wantedOrders.Add(chosenMeal);
            }
        }

        if (wantedOrders.Count == 0)
        {
            Debug.LogWarning("Customer has no available meal to order.");
            LeaveAngry(0);
            return;
        }

        PrintOrders();

        GoToSeat();
    }

    private void PrintOrders()
    {
        string orderText = "Customer ordered: ";

        for (int i = 0; i < wantedOrders.Count; i++)
        {
            orderText += wantedOrders[i].mealName;

            if (i < wantedOrders.Count - 1)
            {
                orderText += ", ";
            }
        }

        Debug.Log(orderText);
    }

    // Sets the customer's wanted order based on their preferences or possible orders
    private MealData ChooseOrder()
    {
        if (preferredMeals != null && preferredMeals.Length > 0)
        {
            return preferredMeals[Random.Range(0, preferredMeals.Length)];
        }

        if (possibleOrders != null && possibleOrders.Length > 0)
        {
            return possibleOrders[Random.Range(0, possibleOrders.Length)];
        }

        return null;
    }

    // Customer walks to their seat after ordering
    private void GoToSeat()
    {
        if (seatPoint == null)
        {
            Debug.LogWarning("Customer has no seatPoint.");
            LeaveAngry(0);
            return;
        }

        currentState = CustomerState.WalkingToSeat;

        agent.isStopped = false;
        agent.SetDestination(seatPoint.position);
    }

    // Customer sits at their seat and waits for the player to serve their food
    private void SitAndWaitForFood()
    {
        currentState = CustomerState.WaitingForFood;

        agent.isStopped = true;
        agent.ResetPath();

        transform.position = seatPoint.position;
        transform.rotation = seatPoint.rotation;

        waitTimer = maxWaitTime;

        Debug.Log("Customer is waiting for food.");
    }

    // Customer waits for the player to serve their food, and leaves angry if they wait too long
    private void UpdateWaitingTimer()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
        {
            Debug.Log("Customer waited too long and left angry.");
            LeaveAngry(timeoutPay);
        }
    }

    public void ServeFood(MealData servedFood)
    {
        // FoodOrder servedFood

        if (currentState != CustomerState.WaitingForFood)
            return;

        if (servedFood == null)
        {
            Debug.Log("No valid food was served.");
            LeaveAngry(wrongOrderPay);
            return;
        }

        MealData matchingMeal = FindMatchingUnservedMeal(servedFood);

        if (matchingMeal == null)
        {
            Debug.Log("Wrong order served: " + servedFood.mealName);
            LeaveAngry(wrongOrderPay);
            return;
        }

        servedOrders.Add(matchingMeal);

        Debug.Log("Served correct meal: " + matchingMeal.mealName);

        // put the served food directly below this line
        // something like this maybe, whatever the script is called
        if (servedOrders.Count >= wantedOrders.Count)
        {
            Debug.Log("Full order served correctly.");
            LeaveHappy(fullPay);
        }
        else
        {
            Debug.Log("Customer is still waiting for more food.");
        }
    }

    private MealData FindMatchingUnservedMeal(MealData servedFood)
    {
        foreach (MealData wantedMeal in wantedOrders)
        {
            if (wantedMeal.mealID != servedFood.mealID)
                continue;

            int wantedCount = CountMeal(wantedOrders, wantedMeal);
            int servedCount = CountMeal(servedOrders, wantedMeal);

            if (servedCount < wantedCount)
                return wantedMeal;
        }

        return null;
    }

    private int CountMeal(List<MealData> mealList, MealData meal)
    {
        int count = 0;

        foreach (MealData item in mealList)
        {
            if (item.mealID == meal.mealID)
            {
                count++;
            }
        }

        return count;
    }

    // Customer leaves happy and gives the player full payment
    private void LeaveHappy(int payment)
    {
        GivePlayerMoney(payment);

        currentState = CustomerState.LeavingHappy;

        if (exitPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(exitPoint.position);
    }

    // Customer leaves angry and gives the player partial payment
    private void LeaveAngry(int payment)
    {
        GivePlayerMoney(payment);

        currentState = CustomerState.LeavingAngry;

        if (exitPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(exitPoint.position);
    }

    // Give the player money after serving the customer
    private void GivePlayerMoney(int amount)
    {
        Debug.Log("Player earned: " + amount);

        // Put the money system below this line
    }

    // Check if the customer has arrived at their destination
    private bool HasArrived()
    {
        if (agent.pathPending)
            return false;

        return agent.remainingDistance <= agent.stoppingDistance + 0.1f;
    }

    // Update the customer's animation based on their state and movement
    private void UpdateAnimation()
    {
        if (animator == null || agent == null)
        {
            return;
        }

        bool isWalking = agent.velocity.sqrMagnitude > 0.05f;

        animator.SetBool("Walking", isWalking);
        animator.SetBool("Sitting", currentState == CustomerState.WaitingForFood);
        animator.SetBool("Angry", currentState == CustomerState.LeavingAngry);
        animator.SetBool("Happy", currentState == CustomerState.LeavingHappy);
    }

    // Allow a spawner or other system to assign explicit preferences
    public void SetPreferences(MealData[] prefs)
    {
        preferredMeals = prefs;
    }
}
