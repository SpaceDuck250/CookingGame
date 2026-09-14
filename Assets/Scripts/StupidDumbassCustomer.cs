using UnityEngine;
using System.Collections.Generic;

public class StupidDumbassCustomer : MonoBehaviour
{
    public float waitTime;
    public float timer;

    public CustomerMovementScript moveScript;

    public List<Transform> wanderPointList = new List<Transform>();

    public float iq;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            int randomIndex = Random.Range(0, wanderPointList.Count);
            Transform newDestination = wanderPointList[randomIndex];
            moveScript.OnNewDestinationChange(newDestination);

            timer = 0;
        }
    }
}
