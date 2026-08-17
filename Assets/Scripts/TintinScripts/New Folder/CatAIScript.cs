using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using Cat;

namespace Cat
{
    public enum CatState
    {
        WalkingToStall,
        Waiting,  
        Eating,
        Leaving,
    }
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class CatAIScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform waitPoint;
    public Transform exitPoint;

    private Transform parentWithWanderPoints;
    private List<Transform> wanderPoints = new List<Transform>();

    public float sitDuration = 3f; 

    public FoodData requiredFood;

    public AudioSource audioSource;
    public AudioClip meowSound;
    public float meowInterval = 4f;

    public float eatDuration = 1.5f;

    public float rotationOffsetDegrees = 90f;

    public CatState currentState;
    public Action<CatState> OnCatChangeState;

    public static event Action OnCatLeft;

    private float meowTimer = 0f;
    private float eatTimer = 0f;

    private float sitTimer = 0f;
    private bool isMovingToNextSpot = false;

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        OnCatChangeState += ChangeCatState;
    }

    private void OnDestroy()
    {
        OnCatChangeState -= ChangeCatState;
    }

    public void BeginVisit(Transform wait, Transform exit, Transform wanderParent = null)
    {
        waitPoint = wait;
        exitPoint = exit;

        parentWithWanderPoints = wanderParent;
        wanderPoints.Clear();

        if (parentWithWanderPoints != null)
        {
            BirdMovementScript.FillListWithChildrenFromTransform(parentWithWanderPoints, ref wanderPoints);
            Debug.Log("[Cat] Loaded " + wanderPoints.Count + " wander points from " + parentWithWanderPoints.name);
        }

        OnCatChangeState?.Invoke(CatState.WalkingToStall);
    }

    public void ChangeCatState(CatState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case CatState.WalkingToStall:
                agent.SetDestination(waitPoint.position);
                break;

            case CatState.Waiting:
                meowTimer = 0f;
                sitTimer = 0f;
                isMovingToNextSpot = false;
                PlayMeow();
                break;

            case CatState.Eating:
                eatTimer = 0f;
                break;

            case CatState.Leaving:
                agent.SetDestination(exitPoint.position);
                break;
        }
    }

    private void Update()
    {
        RotateTowardsMovement();

        switch (currentState)
        {
            case CatState.WalkingToStall:
                if (HasReachedDestination())
                {
                    OnCatChangeState?.Invoke(CatState.Waiting);
                }
                break;

            case CatState.Waiting:
                TickMeow();
                TickWander();
                break;

            case CatState.Eating:
                TickEatTimer();
                break;

            case CatState.Leaving:
                if (HasReachedDestination())
                {
                    OnCatLeft?.Invoke();
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void TickMeow()
    {
        meowTimer += Time.deltaTime;

        if (meowTimer < meowInterval)
        {
            return;
        }

        meowTimer = 0f;
        PlayMeow();
    }

    private void PlayMeow()
    {
        if (audioSource != null && meowSound != null)
        {
            audioSource.PlayOneShot(meowSound);
        }
    }

    private void TickWander()
    {
        if (wanderPoints.Count == 0)
        {
            return;
        }

        if (isMovingToNextSpot)
        {
            if (HasReachedDestination())
            {
                isMovingToNextSpot = false;
                sitTimer = 0f;
            }
            return;
        }

        sitTimer += Time.deltaTime;

        if (sitTimer < sitDuration)
        {
            return;
        }

        Transform nextSpot = PickRandomWanderPoint();
        agent.SetDestination(nextSpot.position);
        isMovingToNextSpot = true;
    }

    private Transform PickRandomWanderPoint()
    {
        int randomIndex = UnityEngine.Random.Range(0, wanderPoints.Count);
        return wanderPoints[randomIndex];
    }

    private void TickEatTimer()
    {
        eatTimer += Time.deltaTime;

        if (eatTimer < eatDuration)
        {
            return;
        }

        OnCatChangeState?.Invoke(CatState.Leaving);
    }

    public bool TryFeedFish(FoodData offeredFood)
    {
        if (currentState != CatState.Waiting)
        {
            return false;
        }

        if (offeredFood != requiredFood)
        {
            return false;
        }

        OnCatChangeState?.Invoke(CatState.Eating);
        return true;
    }

    private bool HasReachedDestination()
    {
        if (agent.pathPending)
        {
            return false;
        }

        return agent.remainingDistance <= agent.stoppingDistance + 0.05f;
    }

    private void RotateTowardsMovement()
    {
        if (agent.velocity.magnitude <= 0.01f)
        {
            return;
        }

        Vector3 moveDirection = agent.velocity;
        moveDirection.y = 0;

        float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle + rotationOffsetDegrees, 0);
    }
}