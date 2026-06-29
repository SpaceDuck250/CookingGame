using UnityEngine;
using System.Collections.Generic;
using System;

public class CustomerScript : Interactable
{
    public CustomerData heldCustomerData;

    public MealData currentMealOrder;

    // fix this later
    public bool switchValue = false;

    public event Action<MealData> OnNewMealPicked;

    public MealChecker mealChecker;

    private void Start()
    {
        PickNewMeal();

        mealChecker.OnMealOrderFulfilled += OnMealOrderFulfilled;
    }

    private void OnDestroy()
    {
        mealChecker.OnMealOrderFulfilled -= OnMealOrderFulfilled;

    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (!switchValue)
        {
            NpcDialogueScript.OnTalkToCustomer?.Invoke(heldCustomerData, currentMealOrder);
        }
        else
        {
            NpcDialogueScript.OnEndTalkToCustomer?.Invoke();
        }

        switchValue = !switchValue;
    }

    public void PickNewMeal()
    {
        currentMealOrder = PickRandomMeal();
        //OnNewMealPicked?.Invoke(currentMealOrder);
    }

    public MealData PickRandomMeal()
    {
        int randomIndex = UnityEngine.Random.Range(0, heldCustomerData.possibleMealOrders.Count);

        MealData randomMeal = heldCustomerData.possibleMealOrders[randomIndex];

        return randomMeal;
    }

    public void OnMealOrderFulfilled()
    {
        
    }
}
