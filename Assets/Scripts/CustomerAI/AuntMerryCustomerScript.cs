using Customer;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AuntMerryCustomerScript : MonoBehaviour
{
    public CustomerStateMachine stateMachine; public CustomerMovementScript movementScript; public AIEventSystemScript eventSystem;

    public Transform visionOrigin;
    public LayerMask obstacleLayer;
    public LayerMask groundLayer;
    public Transform[] inspectionPoints;
    private string[] foodTags = { "Food", "Cooked", "Raw", "Burnt", "HealthViolation" };

    // Aunt Merry's vision size
    public float visionDistance = 8f;
    public float fieldOfViewAngle = 120f;

    // Aunt Merry's patrols
    public float inspectionArrivalDistance = 0.4f;
    public float lookDurationAtPoint = 1.5f;
    public float pointTravelTimeout = 15f;
    public float inspectionTurnSpeed = 360f;

    // How close food must be to the floor to count as lying on it
    public float foodGroundDistance = 0.1f;
    public float visionCheckInterval = 0.2f;

    // Maximum number of food colliders examined during one scan
    public int visionBufferSize = 32;

    public bool inspectorRequestPending;
    public bool returnAfterSpottingFood = true;
    public bool inspectionStarted;
    public bool inspectionFinished;
    public GameObject spottedFood;

    private Collider[] visionColliderBuffer;
    private Coroutine inspectionRouteCoroutine;

    private void Awake()
    {
        ResolveReferences();
        CreateVisionBuffer();
    }

    private void OnEnable()
    {
        ResolveReferences();
        CreateVisionBuffer();

        if (stateMachine != null)
        {
            stateMachine.OnCustomerChangeState += HandleCustomerStateChanged;
        }

        AIEventSystemScript.OnEventFinished += HandleEventFinished;
    }

    private void OnDisable()
    {
        if (stateMachine != null)
        {
            stateMachine.OnCustomerChangeState -= HandleCustomerStateChanged;
        }

        AIEventSystemScript.OnEventFinished -= HandleEventFinished;

        if (inspectionRouteCoroutine != null)
        {
            StopCoroutine(inspectionRouteCoroutine);
            inspectionRouteCoroutine = null;
        }

        inspectionStarted = false;
    }

    private void ResolveReferences()
    {
        if (stateMachine == null)
        {
            stateMachine = GetComponent<CustomerStateMachine>();
        }

        if (movementScript == null)
        {
            movementScript = GetComponent<CustomerMovementScript>();
        }

        if (eventSystem == null)
        {
            eventSystem = AIEventSystemScript.Instance;
        }

        if (visionOrigin == null)
        {
            visionOrigin = transform;
        }
    }

    private void CreateVisionBuffer()
    {
        int safeBufferSize = Mathf.Max(1, visionBufferSize);

        if (visionColliderBuffer == null || visionColliderBuffer.Length != safeBufferSize)
        {
            visionColliderBuffer = new Collider[safeBufferSize];
        }
    }

    private void HandleCustomerStateChanged(CustomerState newState)
    {
        if (newState != CustomerState.WalkingToCounter)
        {
            return;
        }

        StartInspectionRoute();
    }

    public void SetInspectionPoints(Transform[] suppliedPoints)
    {
        inspectionPoints = suppliedPoints;
    }

    private void StartInspectionRoute()
    {
        if (inspectionStarted || inspectionFinished)
        {
            return;
        }

        ResolveReferences();

        if (movementScript == null)
        {
            Debug.Log("Aunt Merry cannot inspect the store because CustomerMovementScript was not found.");

            return;
        }

        if (inspectionPoints == null || inspectionPoints.Length == 0)
        {
            Debug.Log("Aunt Merry has no inspection points and she will continue directly to the queue.");

            inspectionFinished = true;
            return;
        }

        inspectionStarted = true;

        inspectionRouteCoroutine = StartCoroutine(InspectionRouteRoutine());
    }

    private IEnumerator InspectionRouteRoutine()
    {
        // Wait one frame so the normal WalkingToCounter finishes setting Aunt Merry's initial destination
        yield return null;

        bool stopRoute = false;

        for (int i = 0; i < inspectionPoints.Length; i++)
        {
            Transform inspectionPoint = inspectionPoints[i];

            if (inspectionPoint == null)
            {
                continue;
            }

            Debug.Log($"Aunt Merry is walking to inspection point {(i + 1)}.");

            yield return MoveToInspectionPoint(inspectionPoint);

            if (inspectionPoint == null)
            {
                continue;
            }

            yield return FaceInspectionDirection(inspectionPoint);

            float lookEndTime = Time.time + lookDurationAtPoint;

            // perform at least one vision check
            do
            {
                if (TrySeeFoodOnGround(out GameObject visibleFood))
                {
                    spottedFood = visibleFood;
                    inspectorRequestPending = true;

                    Debug.Log($"Aunt Merry saw food while inspecting: {visibleFood.name}");

                    TrySummonInspector();

                    if (returnAfterSpottingFood)
                    {
                        stopRoute = true;
                    }

                    break;
                }

                yield return new WaitForSeconds(visionCheckInterval
                );
            }
            while (Time.time < lookEndTime);

            if (stopRoute)
            {
                break;
            }
        }

        inspectionStarted = false;
        inspectionFinished = true;
        inspectionRouteCoroutine = null;

        ReturnToQueue();
    }

    private IEnumerator MoveToInspectionPoint(Transform inspectionPoint)
    {
        if (inspectionPoint == null || movementScript == null || stateMachine == null)
        {
            yield break;
        }

        NavMeshAgent agent = stateMachine.agent;

        if (agent == null)
        {
            Debug.Log("Aunt Merry does not have a NavMeshAgent.");
            yield break;
        }

        if (!agent.enabled || !agent.isOnNavMesh)
        {
            Debug.Log("Aunt Merry's NavMeshAgent is unavailable.");
            yield break;
        }

        // Find the nearest valid NavMesh position around the point.
        if (!NavMesh.SamplePosition(inspectionPoint.position, out NavMeshHit navMeshHit, 2f, NavMesh.AllAreas))
        {
            Debug.Log($"Inspection point is not close to the NavMesh: {inspectionPoint.name}");
            yield break;
        }

        agent.isStopped = false;
        agent.updateRotation = true;
        agent.ResetPath();

        movementScript.OnNewDestinationChange?.Invoke(inspectionPoint);

        Debug.Log($"Aunt Merry is moving toward {inspectionPoint.name}.");

        // Give the agent one frame to calculate its path.
        yield return null;

        float timeoutTime = Time.time + pointTravelTimeout;

        while (Time.time < timeoutTime)
        {
            if (!agent.enabled || !agent.isOnNavMesh)
            {
                yield break;
            }

            if (agent.pathPending)
            {
                yield return null;
                continue;
            }

            // Account for the agent's own stopping distance.

            float arrivalDistance = Mathf.Max(inspectionArrivalDistance, agent.stoppingDistance + 0.05f);

            bool reachedPoint = !float.IsInfinity(agent.remainingDistance) && agent.remainingDistance <= arrivalDistance;

            if (reachedPoint)
            {
                // Aunt Merry has reached the inspection point stop her normal movement before she begins looking
                agent.isStopped = true;

                movementScript.destinationPoint = null;

                Debug.Log($"Aunt Merry reached inspection point: {inspectionPoint.name}");

                yield break;
            }

            yield return null;
        }

        Debug.LogWarning($"Aunt Merry timed out while moving to {inspectionPoint.name}. Remaining distance: {agent.remainingDistance}");
    }

    private IEnumerator FaceInspectionDirection(Transform inspectionPoint)
    {
        if (inspectionPoint == null)
        {
            yield break;
        }

        NavMeshAgent agent = stateMachine != null ? stateMachine.agent : null;

        bool canControlAgent = agent != null && agent.enabled && agent.isOnNavMesh;

        if (canControlAgent)
        {
            // Stop the agent from fighting Aunt Merry's manual rotation.
            agent.isStopped = true;
            agent.updateRotation = false;
        }

        Vector3 lookDirection = Vector3.ProjectOnPlane(inspectionPoint.forward, Vector3.up);

        if (lookDirection.sqrMagnitude <= Mathf.Epsilon)
        {
            yield break;
        }

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);

        // Prevent the turning coroutine from running forever.
        float turnTimeout = Time.time + 2f;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 2f && Time.time < turnTimeout)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, inspectionTurnSpeed * Time.deltaTime);

            yield return null;
        }

        if (canControlAgent)
        {
            agent.updateRotation = true;
        }
    }

    private void ReturnToQueue()
    {
        if (stateMachine == null)
        {
            Debug.Log("Aunt Merry cannot return because CustomerStateMachine is missing.");
            return;
        }

        if (stateMachine.queuePoint == null)
        {
            Debug.Log("Aunt Merry cannot return because she does not have a queue point.");
            return;
        }

        NavMeshAgent agent = stateMachine.agent;

        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
            agent.ResetPath();
        }

        // inspectionFinished is already true, so invoking WalkingToCounter will not restart the inspection route
        stateMachine.OnCustomerChangeState?.Invoke(CustomerState.WalkingToCounter);

        Debug.Log("Aunt Merry finished inspecting and is returning to the queue.");
    }

    private bool TrySeeFoodOnGround(out GameObject visibleFood)
    {
        visibleFood = null;

        if (visionOrigin == null)
        {
            return false;
        }

        // Search all nearby colliders Food objects are filtered afterward using the Food tag
        int colliderCount = Physics.OverlapSphereNonAlloc(visionOrigin.position, visionDistance, visionColliderBuffer, ~0, QueryTriggerInteraction.Collide);

        for (int i = 0; i < colliderCount; i++)
        {
            Collider detectedCollider = visionColliderBuffer[i];

            if (detectedCollider == null)
            {
                continue;
            }

            GameObject foodObject = FindTaggedFoodObject(detectedCollider);

            if (foodObject == null)
            {
                continue;
            }

            if (!IsFoodOnGround(detectedCollider))
            {
                continue;
            }

            Vector3 targetPosition = detectedCollider.bounds.center;

            Vector3 directionToFood = targetPosition - visionOrigin.position;

            float distanceToFood = directionToFood.magnitude;

            if (distanceToFood <= 0f || distanceToFood > visionDistance)
            {
                continue;
            }

            if (!IsInsideFieldOfView(directionToFood))
            {
                continue;
            }

            if (IsVisionBlocked(targetPosition, distanceToFood))
            {
                continue;
            }

            visibleFood = foodObject;

            return true;
        }

        return false;
    }

    private GameObject FindTaggedFoodObject(Collider detectedCollider)
    {
        if (detectedCollider == null)
        {
            return null;
        }

        // Check the Rigidbody root first in case child colliders often share the root Rigidbody
        Rigidbody attachedRigidbody = detectedCollider.attachedRigidbody;

        if (attachedRigidbody != null && HasFoodTag(attachedRigidbody.gameObject))
        {
            return attachedRigidbody.gameObject;
        }

        // Search the collider and its parents for the Food tag
        Transform currentTransform = detectedCollider.transform;

        while (currentTransform != null)
        {
            if (HasFoodTag(currentTransform.gameObject))
            {
                return currentTransform.gameObject;
            }

            currentTransform = currentTransform.parent;
        }

        return null;
    }

    // Find the food tags and compare it with the food array
    private bool HasFoodTag(GameObject objectToCheck)
    {
        if (objectToCheck == null || foodTags == null || foodTags.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < foodTags.Length; i++)
        {
            string tagToCheck = foodTags[i];

            if (string.IsNullOrWhiteSpace(tagToCheck))
            {
                continue;
            }

            if (objectToCheck.CompareTag(tagToCheck))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsInsideFieldOfView(Vector3 directionToFood)
    {
        // Use horizontal directions so food on the floor is not rejected simply because it is below Aunt Merry's eyes
        Vector3 horizontalForward = Vector3.ProjectOnPlane(visionOrigin.forward, Vector3.up);

        Vector3 horizontalDirectionToFood = Vector3.ProjectOnPlane(directionToFood, Vector3.up);

        // Food directly beneath Aunt Merry counts as visible
        if (horizontalDirectionToFood.sqrMagnitude <= Mathf.Epsilon)
        {
            return true;
        }

        if (horizontalForward.sqrMagnitude <= Mathf.Epsilon)
        {
            horizontalForward = transform.forward;
        }

        float angleToFood = Vector3.Angle(horizontalForward.normalized, horizontalDirectionToFood.normalized);

        return angleToFood <= fieldOfViewAngle * 0.5f;
    }

    private bool IsFoodOnGround(Collider foodCollider)
    {
        Bounds foodBounds = foodCollider.bounds;

        // Start near the bottom of the food collider and look downward for the floor
        Vector3 groundCheckOrigin = new Vector3(foodBounds.center.x, foodBounds.min.y + 0.05f, foodBounds.center.z);

        return Physics.Raycast(groundCheckOrigin, Vector3.down, foodGroundDistance + 0.05f, groundLayer, QueryTriggerInteraction.Ignore);
    }

    private bool IsVisionBlocked(Vector3 targetPosition, float targetDistance)
    {
        Vector3 direction = (targetPosition - visionOrigin.position).normalized;

        // Only the obstacle layer is checked here as food itself should not be included in obstacleLayer. Unless
        return Physics.Raycast(visionOrigin.position, direction, targetDistance, obstacleLayer, QueryTriggerInteraction.Ignore);
    }

    private void HandleEventFinished(HawkerEventType finishedEvent)
    {
        if (!inspectorRequestPending)
        {
            return;
        }

        // The active event has finished, so Aunt Merry can retry her pending Inspector request
        TrySummonInspector();
    }

    private void TrySummonInspector()
    {
        ResolveReferences();

        if (!inspectorRequestPending)
        {
            return;
        }

        if (eventSystem == null)
        {
            Debug.Log("Aunt Merry could not summon the Inspector because AIEventSystemScript was not found.");

            return;
        }

        // If Inspector is already active, her request has effectively been fulfilled
        if (eventSystem.currentEvent == HawkerEventType.Inspector)
        {
            inspectorRequestPending = false;
            return;
        }

        // Does not overlap Rush Hour or Fussy Customer as HandleEventFinished will retry afterward
        if (eventSystem.currentEvent != HawkerEventType.None)
        {
            Debug.Log($"Aunt Merry is waiting for {eventSystem.currentEvent} to finish before summoning the Inspector.");

            return;
        }

        // Start the Inspector events
        bool inspectorStarted = eventSystem.TryStartPriorityEvent(HawkerEventType.Inspector);

        if (!inspectorStarted)
        {
            return;
        }

        inspectorRequestPending = false;

        Debug.Log("Aunt Merry summoned the Inspector after seeing food lying on the floor.");
    }

    private void OnDrawGizmosSelected()
    {
        Transform origin = visionOrigin != null ? visionOrigin : transform;

        Gizmos.DrawWireSphere(origin.position, visionDistance);

        Vector3 leftBoundary = Quaternion.Euler(0f, -fieldOfViewAngle * 0.5f, 0f) * origin.forward;

        Vector3 rightBoundary = Quaternion.Euler(0f, fieldOfViewAngle * 0.5f, 0f) * origin.forward;

        Gizmos.DrawLine(origin.position, origin.position + leftBoundary * visionDistance);

        Gizmos.DrawLine(origin.position, origin.position + rightBoundary * visionDistance);
    }

}