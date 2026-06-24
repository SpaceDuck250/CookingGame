using UnityEngine;
using UnityEngine.AI;

public class CustomerAIScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public SeatScript targetSeat;

    public bool walkingToSeat = false;
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

        // Find a seat at spawn
        FindSeat();
    }

    private void Update()
    {
        if (walkingToSeat == true)
        {
            CheckIfReachedSeat();
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

        agent.isStopped = false;
        agent.SetDestination(targetSeat.sitTarget.position);

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

    // Sit down at the target seat and occupy it
    // Later add the customer order function to or from other scripts
    public void SitDown()
    {
        walkingToSeat = false;

        agent.isStopped = true;
        agent.enabled = false;

        transform.position = targetSeat.sitTarget.position;
        transform.rotation = targetSeat.sitTarget.rotation;

        targetSeat.Occupy();

        if (animator != null)
        {
            // Set animation here if needed, e.g., animator.SetBool("isWalking", false);
        }
    }
}
