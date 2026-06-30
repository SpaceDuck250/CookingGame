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
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

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