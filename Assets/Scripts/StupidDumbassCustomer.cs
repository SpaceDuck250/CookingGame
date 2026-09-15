using UnityEngine;
using System.Collections.Generic;

public class StupidDumbassCustomer : MonoBehaviour
{
    public float waitTimeOffset;

    [SerializeField]
    private float waitTime;


    private float extraRandomTime;


    public float timer;

    public CustomerMovementScript moveScript;

    public List<Transform> wanderPointList = new List<Transform>();

    public float iq;

    private void Start()
    {
        extraRandomTime = GetNewOffsetAddedTime();
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime + extraRandomTime)
        {
            int randomIndex = Random.Range(0, wanderPointList.Count);
            Transform newDestination = wanderPointList[randomIndex];
            moveScript.OnNewDestinationChange(newDestination);

            extraRandomTime = GetNewOffsetAddedTime();

            timer = 0;
        }
    }

    private float GetNewOffsetAddedTime()
    {
        return Random.Range(-waitTimeOffset, waitTimeOffset);
    }
}
