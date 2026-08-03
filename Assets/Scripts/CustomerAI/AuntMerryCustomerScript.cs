using Customer;
using UnityEngine;

public class AuntMerryCustomerScript : MonoBehaviour
{
    public CustomerStateMachine stateMachine;
    public AIEventSystemScript eventSystem;
    public AIEventDataScript eventData;

    public bool hasCheckedFloor;
    public bool inspectorRequestPending;

    private void Awake()
    {
        ResolveReferences();
    }

    private void Start()
    {
        // Start runs after CustomerSpawnerScript has instantiated and configured Aunt Merry
        CheckFloorOnce();
    }

    private void OnEnable()
    {
        ResolveReferences();

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
    }

    private void ResolveReferences()
    {
        if (stateMachine == null)
        {
            stateMachine = GetComponent<CustomerStateMachine>();
        }

        if (eventSystem == null)
        {
            eventSystem = AIEventSystemScript.Instance;
        }

        if (eventData == null && eventSystem != null)
        {
            eventData = eventSystem.eventData;
        }
    }

    private void HandleCustomerStateChanged(CustomerState newState)
    {
        if (newState == CustomerState.WalkingToCounter)
        {
            CheckFloorOnce();
        }
    }

    private void HandleEventFinished(HawkerEventType finishedEvent)
    {
        if (!inspectorRequestPending)
        {
            return;
        }

        // The other event has finished, so Aunt Merry can now try to summon the Inspector
        TrySummonInspector();
    }

    private void CheckFloorOnce()
    {
        if (hasCheckedFloor)
        {
            return;
        }

        hasCheckedFloor = true;

        ResolveReferences();

        if (eventData == null)
        {
            Debug.Log("Aunt Merry could not check the floor because AIEventDataScript was not found.");

            return;
        }

        int foodAmount = eventData.FoodLyingAround;

        if (foodAmount <= 0)
        {
            Debug.Log("Aunt Merry checked the floor and found no food.");
            return;
        }

        Debug.Log($"Aunt Merry spotted {foodAmount} food objects on the floor.");

        inspectorRequestPending = true;

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
            Debug.LogWarning("Aunt Merry could not summon the Inspector because AIEventSystemScript was not found.");
            return;
        }

        // The Inspector is already active, so Aunt Merry's so request has effectively been fulfilled
        if (eventSystem.currentEvent == HawkerEventType.Inspector)
        {
            inspectorRequestPending = false;
            return;
        }

        // Not overlap Rush Hour or Fussy Customer as HandleEventFinished will retry when they finish
        if (eventSystem.currentEvent != HawkerEventType.None)
        {
            Debug.Log($"Aunt Merry is waiting for {eventSystem.currentEvent} to finish before summoning the Inspector.");
            return;
        }

        bool inspectorStarted = eventSystem.TryStartPriorityEvent(HawkerEventType.Inspector);

        if (!inspectorStarted)
        {
            return;
        }

        inspectorRequestPending = false;

        Debug.Log("Aunt Merry summoned the Inspector.");
    }
}
