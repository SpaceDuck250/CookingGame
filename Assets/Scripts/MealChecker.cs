using UnityEngine;
using System.Collections.Generic;
using System;

public class MealChecker : Interactable
{
    public List<FoodData> inputFoodDataList = new List<FoodData>();

    public MealData mealToCheck;

    public CustomerInteractScript customerScript;

    public event Action OnMealOrderFulfilled;

    public override void Interact(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeldObj.tag != "Platter")
        {
            return;
        }

        PlatterGiverScript platterGiver = playerHand.currentFoodHeldObj.GetComponent<PlatterGiverScript>();
        inputFoodDataList = platterGiver.GiveFoodDataListFromPlatter();

        mealToCheck = customerScript.currentMealOrder;

        if (CheckIfMealMatchesOrder())
        {
            OnMealOrderFulfilled?.Invoke();
            NpcDialogueScript.OnOrderMetTalk?.Invoke();

            playerHand.ClearFoodFromHand();
        }
        else
        {
            print("meal doesnt match order");
        }

    }

    public bool CheckIfMealMatchesOrder()
    {
        foreach (FoodData foodIngredient in mealToCheck.foodIngredients)
        {
            if (!inputFoodDataList.Contains(foodIngredient))
            {
                return false;
            }
        }

        int correctIngredientsCount = mealToCheck.foodIngredients.Count;

        if (correctIngredientsCount == inputFoodDataList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
