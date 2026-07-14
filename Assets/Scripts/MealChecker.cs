using UnityEngine;
using System.Collections.Generic;
using System;
using Customer;

public class MealChecker : MonoBehaviour
{
    public List<FoodData> inputFoodDataList = new List<FoodData>();

    public MealData mealToCheck = null;

    public CustomerInteractScript customerScript;

    public event Action OnMealOrderFulfilled;
    public event Action OnMealOrderIncorrect;  

    public Transform customerHand;

    public PlatterScript platterHeld;

    public CustomerStateMachine stateMachine;

    private void Start()
    {
        SetMeal();
    }

    public void CheckOrder(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeldObj.tag != "Platter")
        {
            return;
        }
         
        PlatterGiverScript platterGiver = playerHand.currentFoodHeldObj.GetComponent<PlatterGiverScript>();
        inputFoodDataList = platterGiver.GiveFoodDataListFromPlatter();
        platterHeld = platterGiver.platterScript;



        if (CheckIfMealMatchesOrder())
        {
            print("correct");
            OnMealOrderFulfilled?.Invoke();
            NpcDialogueScript.OnOrderMetTalk?.Invoke(customerScript.heldCustomerData);
            stateMachine.OnCustomerChangeState?.Invoke(CustomerState.PayingForFood);    

            playerHand.TransferPlatterToCustomer(customerHand, Quaternion.identity * Quaternion.Euler(0, 90 + 90, 0));
            customerScript.movementScript.holdingTray = true;
        }
        else
        {
            print("meal doesnt match order");
            OnMealOrderIncorrect?.Invoke();
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

    private MealData ChooseOrder()
    {

        if (customerScript == null)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, customerScript.heldCustomerData.possibleMealOrders.Count);

        MealData randomMeal = customerScript.heldCustomerData.possibleMealOrders[randomIndex];

        return randomMeal;
    }

    public void SetMeal()
    {
        mealToCheck = ChooseOrder();
    }

}
