using UnityEngine;
using System.Collections.Generic;

public class BirdAIManager : MonoBehaviour
{
    public BirdState currentState;

    public BirdMovementScript movementScript;

    public Transform foodSpawnParent;
    public FoodData searchFood;

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

    public void CreateSearchItem(FoodData foodData)
    {
        searchFood = foodData;
        CookingInputOutputScript.SpawnDisplayFoodInPosition(searchFood, foodSpawnParent, Vector3.zero, false);
    }

}