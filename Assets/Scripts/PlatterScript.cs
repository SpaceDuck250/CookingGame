using UnityEngine;
using System;

public class PlatterScript : Interactable
{
    public FoodData[] foodHeldArray = new FoodData[4];

    public Transform[] placeAreasArray = new Transform[4];

    public int currentIndex = 0;

    public Vector3 upOffset;

    public Action<FoodData> OnFoodTakenOutOfPlatter;

    private void Start()
    {
        OnFoodTakenOutOfPlatter += TakeFoodOutOfPlatter;
    }

    private void OnDestroy()
    {
        OnFoodTakenOutOfPlatter -= TakeFoodOutOfPlatter;
    }

    private void TakeFoodOutOfPlatter(FoodData foodData)
    {
        for (int i = 0; i < 4; i++)
        {
            if (foodData == foodHeldArray[i])
            {
                foodHeldArray[i] = null;
            }
        }
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (!CheckIfPlayerIsHoldingFood(playerHand))
        {
            return;
        }

        FindFreeSpotAndPlace(playerHand.currentFoodHeld);

    }

    public bool CheckIfPlayerIsHoldingFood(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeld == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void FindFreeSpotAndPlace(FoodData foodData)
    {

        int emptySlotIndex = -1;

        for (int i = 0; i < 4; i++)
        {
            if (foodHeldArray[i] == null)
            {
                emptySlotIndex = i;
                break;
            }
        }

        if (emptySlotIndex == -1)
        {
            return;
        }

        Transform placeParent = placeAreasArray[emptySlotIndex];

        GameObject newFood = CookingInputOutputScript.SpawnDisplayFoodInPosition(foodData, placeParent, upOffset, true);
        newFood.GetComponent<HoldableFoodScript>().platterIn = this;

        foodHeldArray[emptySlotIndex] = foodData;
    }
}
