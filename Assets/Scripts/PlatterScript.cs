using UnityEngine;
using System;
using System.Collections.Generic;

public class PlatterScript : Interactable
{
    //public FoodData[] foodHeldArray = new FoodData[4];
    public List<FoodData> foodHeldList = new List<FoodData>();

    public Transform[] placeAreasArray = new Transform[4];

    public int currentIndex = 0;

    public Vector3 upOffset;

    public Action<FoodData> OnFoodTakenOutOfPlatter;

    public PlatterToggleScript platterModeToggler;

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
            if (foodData == foodHeldList[i])
            {
                foodHeldList.RemoveAt(i);
                break;
            }
        }
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (!CheckIfPlayerIsHoldingFood(playerHand) || platterModeToggler.currentMode == PlatterToggleScript.PlatterMode.Finished)
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

        //GameObject newFood = CookingInputOutputScript.SpawnDisplayFoodInPosition(foodData, placeParent, upOffset, true, false, true);
        GameObject newFood = CookingInputOutputScript.SpawnFoodInsidePlatter(foodData, placeParent, upOffset);
        PlayerHandScript.instance.ClearFoodFromHand();
        newFood.GetComponent<HoldableFoodScript>().platterIn = this;

        //foodHeldArray[emptySlotIndex] = foodData;
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

    public void MakeAllFoodPickupable(bool value, string newLayer)
    {
        List<GameObject> foodHeldObjList = new List<GameObject>();
        foreach (Transform placeArea in placeAreasArray)
        {
            if (placeArea.childCount > 0)
            {
                foodHeldObjList.Add(placeArea.GetChild(0).gameObject);
            }
        }

        foreach (GameObject food in foodHeldObjList)
        {
            HoldableFoodScript holdScript = food.GetComponent<HoldableFoodScript>();
            holdScript.canPickUp = value;
            food.GetComponent<Collider>().isTrigger = !value;
        }
    }
}
