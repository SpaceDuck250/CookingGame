using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class QueueManager : MonoBehaviour
{
    public Transform[] queuePoints;

    private List<AI> queue = new();

    public void JoinQueue(AI customer)
    {
        queue.Add(customer);
        UpdateQueuePositions();
    }

    public void LeaveQueue(AI customer)
    {
        queue.Remove(customer);
        UpdateQueuePositions();
    }

    void UpdateQueuePositions()
    {
        for (int i = 0; i < queue.Count && i < queuePoints.Length; i++)
{
    queue[i].MoveToQueueSpot(queuePoints[i]);
}
    }

    public AI FrontCustomer()
    {
        if (queue.Count == 0)
            return null;

        return queue[0];
    }
}
