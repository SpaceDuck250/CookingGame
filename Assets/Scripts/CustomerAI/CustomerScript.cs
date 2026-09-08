using UnityEngine;
using UnityEngine.AI;
using System;
using Customer;
using System.Collections;

public class CustomerMovementScript : MonoBehaviour
{
    public MealData orderData;

    //public Transform stallQueuePointTransform;
    //public Transform chairTransform;
    //public Transform exitTransform;
    //public Transform platterAreaTransform;

    public Transform destinationPoint;

    public float sitTime;

    private float sitTimer;

    public NavMeshAgent agent;

    public Action<Transform> OnNewDestinationChange;

    public float closeEnough;
    public bool orderDone = false;
    public bool sitting = false;

    public bool holdingTray = false;

    public Vector3 normalTrayLocalPosition;
    public Vector3 seatedTrayLocalPosition;

    public Action OnCustomerMove;
    public Action OnCustomerIdle;

    public MealChecker mealChecker;

    public CustomerStateMachine customerStateMachine;
    public LeaveWhenAngryScript leaveWhenAngryScript;

    public bool paused = false;

    private void Awake()
    {
        OnNewDestinationChange += SetNewDestination;
    }

    private void OnDestroy()
    {
        OnNewDestinationChange -= SetNewDestination;
    }

    private void Update()
    {
        if (sitting)
        {
            return;
        }
        WalkToDestination();

        // For a bug fix dumb as fuck
        if (customerStateMachine.currentState == CustomerState.WalkingToCounter && CheckIfCloseEnoughToDestination())
        {
            customerStateMachine.interactScript.RotateTo(CustomerSpawnerScript.instance.mainCounterPoint.gameObject);
        }
    }

    private void SetNewDestination(Transform destination)
    {
        //if (paused)
        //{
        //    return;
        //}

        this.destinationPoint = destination;

        //StartCoroutine(DontAllowAbruptDestinationChange());
    }

    public void WalkToDestination()
    {
        if (destinationPoint == null)
        {
            return;
        }

        if (CheckIfCloseEnoughToDestination())
        {
            //print("Close enough");
            OnCustomerIdle?.Invoke();
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
            return;
        }
        else
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
            }
        }

        OnCustomerMove?.Invoke();
        agent.SetDestination(destinationPoint.position);
    }

    public bool CheckIfCloseEnoughToDestination()
    {
        if (destinationPoint == null)
        {
            return false;
        }

        float distance = Vector3.Distance(transform.position, destinationPoint.position);
        if (distance <= closeEnough)
        {
            return true;
        }

        return false;
    }

    // To fix some stupid bugs, the customer isnt able to change its destination within a short amount of time
    //public IEnumerator DontAllowAbruptDestinationChange()
    //{
    //    paused = true;

    //    float waitTime = 0.3f;
    //    yield return new WaitForSeconds(waitTime);

    //    paused = false;


    //}

}