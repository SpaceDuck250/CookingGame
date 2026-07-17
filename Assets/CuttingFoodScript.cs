using UnityEngine;
using System;
using System.Collections.Generic;

public class CuttingFoodScript : Interactable
{
    //public FoodData[] foodHeldArray = new FoodData[4];
    public List<FoodData> foodHeldList = new List<FoodData>();

    public Transform placeAreasArray;

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
        for (int i = 0; i < foodHeldList.Count; i++)
        {
            if (foodData == foodHeldList[i])
            {
                foodHeldList.RemoveAt(i);
                break;
            }
        }
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (!CheckIfPlayerIsHoldingFood(playerHand) && placeAreasArray.childCount > 0)
        {
            
        }
        else if (!CheckIfPlayerIsHoldingFood(playerHand))
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
        if (foodHeldList.Count >= 4)
        {
            return;
        }



        Transform placeParent = placeAreasArray;

        GameObject newFood = CookingInputOutputScript.SpawnDisplayFoodInPosition(foodData, placeParent, upOffset, true);
        newFood.GetComponent<HoldableFoodScript>().cuttingIn = this;

        //foodHeldArray[emptySlotIndex] = foodData;
    }
}
