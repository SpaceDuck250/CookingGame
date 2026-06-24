using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    NavMeshAgent agent;
    Seat targetSeat;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent on " + gameObject.name);
            return;
        }

        FindChair();
    }


    void FindChair()
    {
        GameObject[] chairs = GameObject.FindGameObjectsWithTag("Chair");

        float closestDistance = Mathf.Infinity;
        Seat closestSeat = null;


        foreach (GameObject chair in chairs)
        {
            Seat seat = chair.GetComponent<Seat>();

            if (seat != null && seat.IsAvailable())
            {
                float distance = Vector3.Distance(
                    transform.position,
                    chair.transform.position
                );


                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSeat = seat;
                }
            }
        }


        if (closestSeat != null)
        {
            targetSeat = closestSeat;
            targetSeat.TakeSeat();

            agent.SetDestination(
                targetSeat.transform.position
            );
        }
        else
        {
            Debug.Log("No empty chairs!");
        }
    }


    void Update()
    {
        if (targetSeat != null &&
           !agent.pathPending &&
           agent.remainingDistance <= agent.stoppingDistance)
        {
            Sit();
        }
    }


    void Sit()
    {
        agent.isStopped = true;

        Animator anim = GetComponent<Animator>();

        if (anim)
            anim.SetTrigger("Sit");
    }
}