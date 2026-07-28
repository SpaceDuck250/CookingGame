using UnityEngine;
using System.Collections.Generic;

public class BirdFlyIntoTheSunsetState : BirdState
{
    public Transform parentWithExitPoints;
    private List<Transform> flyPointsList = new List<Transform>();

    private Transform currentFlyPoint;
    public int currentFlyIndex = 0;

    public float closeEnoughValue = 0.1f;

    public float moveSpeed;
    public float rotateSpeed;

    private void Start()
    {
        BirdMovementScript.FillListWithChildrenFromTransform(parentWithExitPoints, ref flyPointsList);
    }

    public override void DoAction()
    {
        Fly();
    }

    public void Fly()
    {
        if (flyPointsList.Count == 0)
        {
            print("Empty array");
            return;
        }

        if (currentFlyPoint == null)
        {
            currentFlyPoint = flyPointsList[0];
            currentFlyIndex = 0;

        }

        
        birdMovementScript.FlyToPoint(currentFlyPoint);

        if (birdMovementScript.CheckIfCloseEnough())
        {
            IncrementFlyPoint();
        }
    }

    public void IncrementFlyPoint()
    {
        currentFlyIndex++;

        if (currentFlyIndex >= flyPointsList.Count)
        {
            currentFlyIndex = 0;

            BirdState returnState = transitionStates[0];
            stateManager.TransitionToNewState(returnState);
            return;
        }

        currentFlyPoint = flyPointsList[currentFlyIndex];


    }

}
