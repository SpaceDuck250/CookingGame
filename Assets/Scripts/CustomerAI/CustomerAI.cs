using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
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

    // Points for the customer to move to
    public Transform counterPoint;
    public Transform seatPoint;
    public Transform exitPoint;

    // Order data for the customer
    public MealData[] possibleOrders;
    public MealData wantedOrder;

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
    }

    void Update()
    {
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

    // Customer sets counter as destination and walks to it
    private void GoToCounter()
    {
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

        // put the customer order directly below this line
        // something like this maybe, whatever the script is called
        wantedOrder = possibleOrders[Random.Range(0, possibleOrders.Length)];
        Debug.Log("Customer ordered: " + wantedOrder.mealName);


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
            LeaveAngry(0);
        }
    }

    public void ServeFood(MealData servedFood)
    {
        // FoodOrder servedFood

        if (currentState != CustomerState.WaitingForFood)
        {
            return;
        }

        // put the served food directly below this line
        // something like this maybe, whatever the script is called
        if (servedFood == wantedOrder)
        {
            Debug.Log("Correct order served.");
            LeaveHappy(fullPay);
        }
        else
        {
            Debug.Log("Wrong order served.");
            LeaveAngry(wrongOrderPay);
        }
    }

    // Customer leaves happy and gives the player full payment
    private void LeaveHappy(int payment)
    {
        GivePlayerMoney(payment);

        currentState = CustomerState.LeavingHappy;
        agent.isStopped = false;
        agent.SetDestination(exitPoint.position);
    }

    // Customer leaves angry and gives the player partial payment
    private void LeaveAngry(int payment)
    {
        GivePlayerMoney(payment);

        currentState = CustomerState.LeavingAngry;
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
        bool isWalking = agent.velocity.sqrMagnitude > 0.05f;

        animator.SetBool("Walking", isWalking);
        animator.SetBool("Sitting", currentState == CustomerState.WaitingForFood);
        animator.SetBool("Angry", currentState == CustomerState.LeavingAngry);
        animator.SetBool("Happy", currentState == CustomerState.LeavingHappy);
    }
}
