using UnityEngine;
using System;

public class BrownFoodBox : Interactable
{
    public Transform foodSpawnArea;
    public FoodData foodStored;

    public int foodStoredCount;
    public int maxFoodCount;

    public event Action<int, int> OnFoodAmountChangedInBox;

    public override void Interact(PlayerHandScript playerHand)
    {
        if (foodStored == null)
        {
            return;
        }

        if (playerHand.currentFoodHeld == foodStored)
        {
            TryPutFood();
        }
        else if (playerHand.currentFoodHeld == null && playerHand.currentFoodHeldObj == null)
        {
            TryTakeOutFood();
        }
    }

    public void TryPutFood()
    {
        if (foodStoredCount >= maxFoodCount)
        {
            return;
        }
        foodStoredCount++;

        PlayerHandScript.instance.ClearFoodFromHand();
        OnFoodAmountChangedInBox?.Invoke(foodStoredCount, maxFoodCount);
    }

    public void TryTakeOutFood()
    {
        if (foodStoredCount <= 0)
        {
            return;
        }
        foodStoredCount--;

        //GameObject newFood = CookingInputOutputScript.SpawnDisplayFoodInPosition(foodStored, foodSpawnArea, Vector3.zero, true);
        PlayerHandScript.instance.BringFoodDirectlyToHand(foodStored);

        OnFoodAmountChangedInBox?.Invoke(foodStoredCount, maxFoodCount);
    }

}

