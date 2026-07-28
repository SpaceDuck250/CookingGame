using UnityEngine;
using System.Collections.Generic;

public class BirdAIManager : MonoBehaviour
{
    public BirdState currentState;

    public BirdMovementScript movementScript;

    private void Start()
    {
        TransitionToNewState(currentState);
    }

    private void Update()
    {
        currentState.DoAction();
    }

    public void TransitionToNewState(BirdState newState)
    {
        newState.SetupState(this, movementScript);

        currentState = newState;

    }



}