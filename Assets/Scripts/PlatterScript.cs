using UnityEngine;
using System;
<<<<<<< HEAD

public class PlatterScript : Interactable
{
    public FoodData[] foodHeldArray = new FoodData[4];
=======
using System.Collections.Generic;

public class PlatterScript : Interactable
{
    //public FoodData[] foodHeldArray = new FoodData[4];
    public List<FoodData> foodHeldList = new List<FoodData>();
>>>>>>> origin/newestAlex

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
<<<<<<< HEAD
            if (foodData == foodHeldArray[i])
            {
                foodHeldArray[i] = null;
=======
            if (foodData == foodHeldList[i])
            {
                foodHeldList.RemoveAt(i);
                break;
>>>>>>> origin/newestAlex
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
<<<<<<< HEAD
=======
        if (foodHeldList.Count >= 4)
        {
            return;
        }
>>>>>>> origin/newestAlex

        int emptySlotIndex = -1;

        for (int i = 0; i < 4; i++)
        {
<<<<<<< HEAD
            if (foodHeldArray[i] == null)
=======
            if (placeAreasArray[i].childCount == 0)
>>>>>>> origin/newestAlex
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

<<<<<<< HEAD
        foodHeldArray[emptySlotIndex] = foodData;
=======
        //foodHeldArray[emptySlotIndex] = foodData;
        foodHeldList.Add(foodData);
>>>>>>> origin/newestAlex
    }
}
