using UnityEngine;
using UnityEngine.AI;
using System;

public class CustomerMovementScript : MonoBehaviour
{
    public MealData orderData;

    public Transform stallQueuePointTransform;
    public Transform tableTransform;
    public Transform exitTransform;

    public Transform destinationPoint;

    public float sitTime;

    private float sitTimer;

    public NavMeshAgent agent;

    public Action<Transform> OnNewDestinationChange;

    public float closeEnough;
    public bool orderDone = false;

    public Action OnCustomerMove;
    public Action OnCustomerIdle;

    private void Start()
    {
        SetNewDestination(stallQueuePointTransform);

        OnNewDestinationChange += SetNewDestination;
    }

    private void OnDestroy()
    {
        OnNewDestinationChange -= SetNewDestination;
    }

    private void Update()
    {
        WalkToDestination();
    }

    private void SetNewDestination(Transform destination)
    {
        this.destinationPoint = destination;
    }

    public void WalkToDestination()
    {
        if (CheckIfCloseEnoughToDestination())
        {
            print("Close enough");
            OnCustomerIdle?.Invoke();
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        OnCustomerMove?.Invoke();
        agent.SetDestination(destinationPoint.position);
    }

    public bool CheckIfCloseEnoughToDestination()
    {
        float distance = Vector3.Distance(transform.position, destinationPoint.position);
        if (distance <= closeEnough)
        {
            return true;
        }

        return false;
    }
}