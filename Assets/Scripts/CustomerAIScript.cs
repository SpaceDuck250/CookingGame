using UnityEngine;
using UnityEngine.AI;

public class CustomerAIScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public SeatScript targetSeat;
    public GameObject exitGameObject;

    public bool walkingToSeat = false;
    public bool walkingToExit = false;
    public bool isSitting = false;
    public float arriveExtraDistance = 0.5f;

    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Find the exit GameObject after spawn
        exitGameObject = GameObject.FindGameObjectWithTag("Exit");

        // Find a seat at spawn
        FindSeat();
    }

    private void Update()
    {

        // Check if the customer is walking to a seat or exit and check if they have reached their destination
        if (walkingToSeat == true && isSitting == false)
        {
            CheckIfReachedSeat();
        }

        // Check if the customer is walking to an exit and check if they have reached their destination
        if (walkingToExit == true && isSitting == false)
        {
            CheckIfReachedExit();
        }
    }

    // Find the nearest free seat and set it as the target seat
    public void FindSeat()
    {
        targetSeat = SeatManager.Instance.GetNearestFreeSeat(transform.position);

        if (targetSeat == null)
        {
            Debug.Log("No free seat found.");
            return;
        }

        walkingToSeat = true;
        walkingToExit = false;
        isSitting = false;

        agent.enabled = true;
        agent.isStopped = false;
        agent.SetDestination(targetSeat.sitTarget.position);

        if (animator != null)
        {
            // Set animation here if needed, e.g., animator.SetBool("isWalking", true);
        }
    }

    // Find the nearest exit and set it as the target exit
    public void FindExit()
    {
        if (exitGameObject == null)
        {
            Debug.Log("No free exit found.");
            return;
        }

        targetSeat.Release();

        walkingToSeat = false;
        walkingToExit = true;
        isSitting = false;

        if (agent.enabled == false)
        {
            agent.enabled = true;
        }

        agent.updatePosition = true;
        agent.updateRotation = true;

        agent.isStopped = false;
        agent.SetDestination(exitGameObject.transform.position);

        if (animator != null)
        {
            // Set animation here if needed, e.g., animator.SetBool("isWalking", true);
        }
    }

    // Check if the customer has reached the target seat and sit down if so
    public void CheckIfReachedSeat()
    {
        if (agent.pathPending == true)
        {
            return;
        }

        float distanceLeft = agent.remainingDistance;

        if (distanceLeft <= agent.stoppingDistance + arriveExtraDistance)
        {
            if (agent.velocity.magnitude < 0.1f)
            {
                SitDown();
            }
        }
    }

    // Check if the customer has reached the target exit and leave if so
    public void CheckIfReachedExit()
    {
        if (agent.pathPending == true)
        {
            return;
        }

        float distanceLeft = agent.remainingDistance;

        if (distanceLeft <= agent.stoppingDistance + arriveExtraDistance)
        {
            if (agent.velocity.magnitude < 0.1f)
            {
                Leave();
            }
        }
    }

    // Sit down at the target seat and occupy it
    // Later add the customer order function to or from other scripts
    public void SitDown()
    {
        walkingToSeat = false;
        walkingToExit = false;
        isSitting = true;

        agent.isStopped = true;
        agent.ResetPath();
        agent.updatePosition = false;
        agent.updateRotation = false;

        transform.position = targetSeat.sitTarget.position;
        transform.rotation = targetSeat.sitTarget.rotation;

        targetSeat.Occupy();

        print("Customer is sitting down at the seat.");

        if (animator != null)
        {
            // Set animation here if needed, e.g., animator.SetBool("isWalking", false);
        }
    }

    // Leave the target exit and destroy the customer game object
    public void Leave()
    {
        walkingToExit = false;

        agent.isStopped = true;
        agent.enabled = false;

        Destroy(gameObject);
    }
}
