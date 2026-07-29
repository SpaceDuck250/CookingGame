using UnityEngine;
using System.Collections.Generic;

public abstract class BirdState : MonoBehaviour
{
    public List<BirdState> transitionStates = new List<BirdState>();

    protected BirdAIManager stateManager;
    public BirdMovementScript birdMovementScript;


    public abstract void DoAction();

    public virtual void SetupState(BirdAIManager manager, BirdMovementScript movementScript)
    {
        stateManager = manager;
        birdMovementScript = movementScript;
    }
}
