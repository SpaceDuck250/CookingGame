using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    float moveSpeed = 5;
    Chair currentChair;
    Chair targetChair;
    Transform table;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        table = GameObject.Find("Table").transform;
        Chair[] chairs = FindObjectsByType<Chair>(
        FindObjectsSortMode.None);

        foreach (Chair chair in chairs)
        {
        if (!chair.seated)
            { 
        FindSeat(chair);
        break;
            }
        }
        
    }

    void FindSeat(Chair chair)
    {
        if(chair.seated)
        {
            return;
        }

        chair.seated = true;
        chair.seated = true;

        if (currentChair != null)
            currentChair.seated = false;

        targetChair = chair;
        agent.SetDestination(targetChair.transform.position);
    }

    void Update()
    {
        if (targetChair == null) return;

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            SitDown();
        }
    }

    void SitDown()
    {
        transform.position = targetChair.transform.position;
        transform.rotation = targetChair.transform.rotation;

        currentChair = targetChair;
        targetChair = null;

        animator.SetTrigger("Sit");
        transform.LookAt(table);
        
        Debug.Log("Sat down");
    }
}
