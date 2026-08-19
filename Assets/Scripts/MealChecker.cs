using UnityEngine;
using System.Collections.Generic;
using System;
using Customer;
using Category;
using System.Linq;

public class MealChecker : MonoBehaviour
{
    public List<FoodData> inputFoodDataList = new List<FoodData>();

    public MealData mealToCheck = null;

    public CustomerInteractScript customerScript;

    public event Action OnMealOrderFulfilled;
    public event Action OnWrongOrderServed;

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
            //print("correct");
            PlayerHandScript.OnStopHoldSomething?.Invoke();

            // print("correct");
            OnMealOrderFulfilled?.Invoke();
            NpcDialogueScript.OnOrderMetDialogue?.Invoke(customerScript.heldCustomerData);
            stateMachine.OnCustomerChangeState?.Invoke(CustomerState.PayingForFood);

            playerHand.TransferPlatterToCustomer(customerHand, Quaternion.identity * Quaternion.Euler(0, 90 + 90, 0));


            customerScript.movementScript.holdingTray = true;
        }
        else
        {

            OnWrongOrderServed?.Invoke();

            customerScript.OnInteractWithCustomer?.Invoke();

            bool servedBurntFood = CheckIfMealContainsCookType(CookAmount.Burnt);
            NpcDialogueScript.OnWrongMealServedDialogue?.Invoke(stateMachine.profile, servedBurntFood);
        }

    }

    public bool CheckIfMealMatchesOrder()
    {
        return inputFoodDataList.Select(f => f.foodName).OrderBy(name => name).SequenceEqual(
        mealToCheck.foodIngredients.Select(f => f.foodName).OrderBy(name => name));
    }

    public bool CheckIfMealContainsCookType(CookAmount cookedAmountToCheckFor)
    {
        bool has = inputFoodDataList.Any(n => n.cookedAmount == cookedAmountToCheckFor);
        return has;
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
