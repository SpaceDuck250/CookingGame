using UnityEngine;

public class DumbNPCScript : MonoBehaviour
{
    public Vector3 originPoint;
    public float offsetRange;

    public float moveSpeed;

    public Vector3 destinationPoint;
    public Vector3 upOffset;

    [SerializeField]
    private float waitTimer;
    public float waitTime;

    public Animator animator;

    private void Start()
    {
        originPoint = transform.position;
    }

    private void Update()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            destinationPoint = GetNewPointAroundOrigin();
            waitTimer = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, destinationPoint, Time.deltaTime * moveSpeed);
        if (CheckIfCloseEnough())
        {
            animator.SetBool("walking", false);
        }
        else
        {
            animator.SetBool("walking", true);
        }
    }

    public Vector3 GetNewPointAroundOrigin()
    {
        float randomX = Random.Range(-offsetRange, offsetRange);
        float randomZ = Random.Range(-offsetRange, offsetRange);

        Vector3 newRandomPoint = originPoint + new Vector3(randomX, 0, randomZ);

        return newRandomPoint;
    }

    public bool CheckIfCloseEnough()
    {
        if (Vector3.Distance(transform.position, destinationPoint) <= 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

