using UnityEngine;
using System.Collections.Generic;

public class BirdIdleState : BirdState
{
    // Some have perchable tag i guess
    public Transform parentWithIdlePoints;
    private List<Transform> idlePointsList = new List<Transform>();

    private Transform currentFlyPoint;

    private void Start()
    {
        BirdMovementScript.FillListWithChildrenFromTransform(parentWithIdlePoints, ref idlePointsList);
    }

    public override void DoAction()
    {

        if (currentFlyPoint == null)
        {
            currentFlyPoint = PickRandomFlyPoint();
        }

        birdMovementScript.FlyToPoint(currentFlyPoint);
        
        // Add a timer later
        if (birdMovementScript.CheckIfCloseEnough())
        {
            currentFlyPoint = PickRandomFlyPoint();
        }

    }

    public Transform PickRandomFlyPoint()
    {
        int randomIndex = Random.Range(0, idlePointsList.Count);
        Transform randomPoint = idlePointsList[randomIndex];

        return randomPoint;

    }


}
