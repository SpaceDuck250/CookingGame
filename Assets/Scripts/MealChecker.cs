using UnityEngine;
using System.Collections.Generic;
using System;

public class MealChecker : MonoBehaviour
{
    public List<FoodData> inputFoodDataList = new List<FoodData>();

    public MealData mealToCheck;

    public CustomerInteractScript customerScript;

    public event Action OnMealOrderFulfilled;

    public Transform customerHand;

    public PlatterScript platterHeld;

    public void CheckOrder(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeldObj.tag != "Platter")
        {
            return;
        }

        PlatterGiverScript platterGiver = playerHand.currentFoodHeldObj.GetComponent<PlatterGiverScript>();
        inputFoodDataList = platterGiver.GiveFoodDataListFromPlatter();
        platterHeld = platterGiver.platterScript;

        mealToCheck = customerScript.currentMealOrder;

        if (CheckIfMealMatchesOrder())
        {
            print("correct");
            OnMealOrderFulfilled?.Invoke();
            NpcDialogueScript.OnOrderMetTalk?.Invoke(customerScript.heldCustomerData);

            playerHand.TransferPlatterToCustomer(customerHand, Quaternion.identity * Quaternion.Euler(0, 90 + 90, 0));
            customerScript.movementScript.holdingTray = true;
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
