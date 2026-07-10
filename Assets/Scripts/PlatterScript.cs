using UnityEngine;
using System;
using System.Collections.Generic;

public class PlatterScript : Interactable
{
    public List<FoodData> foodHeldList = new List<FoodData>();

    public Transform[] placeAreasArray = new Transform[4];

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
            if (foodHeldList[i] == foodData)
            {
                foodHeldList.RemoveAt(i);
                break;
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
        return playerHand.currentFoodHeld != null;
    }

    public void FindFreeSpotAndPlace(FoodData foodData)
    {
        if (foodHeldList.Count >= 4)
        {
            return;
        }

        int emptySlotIndex = -1;

        for (int i = 0; i < 4; i++)
        {
            if (placeAreasArray[i].childCount == 0)
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

        GameObject newFood = CookingInputOutputScript.SpawnDisplayFoodInPosition(
            foodData,
            placeParent,
            upOffset,
            true);

        newFood.GetComponent<HoldableFoodScript>().platterIn = this;

        foodHeldList.Add(foodData);
    }

    public void ClearAllInPlatter()
    {
        foodHeldList.Clear();
        foreach (Transform placeArea in placeAreasArray)
        {
            if (placeArea.childCount > 0)
            {
                Destroy(placeArea.GetChild(0).gameObject);
            }
        }

        currentIndex = 0;
    }
}
