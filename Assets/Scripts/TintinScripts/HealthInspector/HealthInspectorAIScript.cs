using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;
using Inspector;

namespace Inspector
{
    public enum InspectorState
    {
        WalkingToPoint,
        Inspecting,
        WalkingToExit,
        Leaving,
    }
}

public class HealthInspectorAIScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public List<HealthInspectionPointScript> inspectionPoints = new List<HealthInspectionPointScript>();
    public Transform exitPoint;

    public float inspectDuration = 2f;

    public decimal fineAmountPerViolation = 50;

    public GameObject floatingTextPrefab;
    public Transform popupSpawnPoint;

    public InspectorState currentState;
    public Action<InspectorState> OnInspectorChangeState;

    public static event Action OnInspectionComplete;

    private int currentPointIndex = 0;
    private float inspectTimer = 0f;
    private int violationsFound = 0;

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        OnInspectorChangeState += ChangeInspectorState;
    }

    private void OnDestroy()
    {
        OnInspectorChangeState -= ChangeInspectorState;
    }

    public void BeginInspection(Transform exit, List<HealthInspectionPointScript> pointsToCheck)
    {
        exitPoint = exit;
        inspectionPoints = pointsToCheck;

        currentPointIndex = 0;
        violationsFound = 0;

        if (inspectionPoints == null || inspectionPoints.Count == 0)
        {
            Debug.LogWarning("[Health Inspector] No inspection points assigned - leaving immediately.");
            OnInspectorChangeState?.Invoke(InspectorState.WalkingToExit);
            return;
        }

        OnInspectorChangeState?.Invoke(InspectorState.WalkingToPoint);
    }

    public void ChangeInspectorState(InspectorState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case InspectorState.WalkingToPoint:
                GoToCurrentPoint();
                break;

            case InspectorState.Inspecting:
                inspectTimer = 0f;
                break;

            case InspectorState.WalkingToExit:
                GoToExit();
                break;

            case InspectorState.Leaving:
                FinishVisit();
                break;
        }
    }

    private void Update()
    {
        RotateTowardsMovement();

        switch (currentState)
        {
            case InspectorState.WalkingToPoint:
                if (HasReachedDestination())
                {
                    OnInspectorChangeState?.Invoke(InspectorState.Inspecting);
                }
                break;

            case InspectorState.Inspecting:
                TickInspectTimer();
                break;

            case InspectorState.WalkingToExit:
                if (HasReachedDestination())
                {
                    OnInspectorChangeState?.Invoke(InspectorState.Leaving);
                }
                break;
        }
    }

    private void GoToCurrentPoint()
    {
        HealthInspectionPointScript point = inspectionPoints[currentPointIndex];
        Transform destination = point.standPoint != null ? point.standPoint : point.transform;
        agent.SetDestination(destination.position);
    }

    private void TickInspectTimer()
    {
        inspectTimer += Time.deltaTime;

        if (inspectTimer < inspectDuration)
        {
            return;
        }

        CheckCurrentPoint();
        currentPointIndex++;

        if (currentPointIndex >= inspectionPoints.Count)
        {
            OnInspectorChangeState?.Invoke(InspectorState.WalkingToExit);
        }
        else
        {
            OnInspectorChangeState?.Invoke(InspectorState.WalkingToPoint);
        }
    }

    private void CheckCurrentPoint()
    {
        HealthInspectionPointScript point = inspectionPoints[currentPointIndex];

        string violationDescription;
        if (point.CheckForViolation(out violationDescription))
        {
            violationsFound++;
            Debug.Log("[Health Inspector] Violation found at " + point.pointName + " - " + violationDescription);
        }
    }

    private void GoToExit()
    {
        if (exitPoint != null)
        {
            agent.SetDestination(exitPoint.position);
        }
        else
        {
            OnInspectorChangeState?.Invoke(InspectorState.Leaving);
        }
    }

    private void FinishVisit()
    {
        if (violationsFound > 0)
        {
            decimal totalFine = fineAmountPerViolation * violationsFound;
            ApplyFine(totalFine);
            Debug.Log("[Health Inspector] Failed inspection - " + violationsFound + " violation(s). Fined $" + totalFine);
        }
        else
        {
            Debug.Log("[Health Inspector] Passed inspection - no violations found.");
        }

        OnInspectionComplete?.Invoke();
        Destroy(gameObject);
    }

    private void ApplyFine(decimal amount)
    {
        if (MoneyManager.instance != null)
        {
            MoneyManager.instance.ChangeMoneyAmount(-amount);
        }
        else
        {
            Debug.LogWarning("[Health Inspector] MoneyManager instance not found - fine not applied.");
        }

        if (floatingTextPrefab != null)
        {
            Vector3 spawnPosition = popupSpawnPoint != null ? popupSpawnPoint.position : transform.position + Vector3.up * 2f;
            GameObject popupInstance = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);

            SnitchFloatingTextScript floatingText = popupInstance.GetComponent<SnitchFloatingTextScript>();
            if (floatingText != null)
            {
                floatingText.SetText("-$" + amount, Color.red);
            }
        }
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
        transform.rotation = Quaternion.Euler(0, angle + 90, 0);
    }
}