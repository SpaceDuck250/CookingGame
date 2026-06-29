using UnityEngine;
using System.Collections.Generic;
using System;

public class CustomerInteractScript : Interactable
{
    public CustomerData heldCustomerData;

    public MealData currentMealOrder;

    // fix this later
    public bool talkingTo = false;

    public event Action<MealData> OnNewMealPicked;

    public MealChecker mealChecker;

    public bool orderComplete = false;
    public CustomerMovementScript movementScript;
    
    private void Start()
    {
        PickNewMeal();

        mealChecker.OnMealOrderFulfilled += OnOrderComplete;
    }

    private void OnDestroy()
    {
        mealChecker.OnMealOrderFulfilled -= OnOrderComplete;

    }

    public override void Interact(PlayerHandScript playerHand)
    {
        TryTalkToCustomer();
    }

    private void TryTalkToCustomer()
    {
        if (!NpcDialogueScript.conversationOpen)
        {
            OpenConversation();
        }
        else
        {
            CloseConversation();
        }
    }

    public void OpenConversation()
    {
        NpcDialogueScript.OnTalkToCustomer?.Invoke(heldCustomerData, currentMealOrder);
    }

    public void CloseConversation()
    {
        NpcDialogueScript.OnEndTalkToCustomer?.Invoke();
        if (orderComplete)
        {
            movementScript.OnNewDestinationChange?.Invoke(movementScript.tableTransform);
        }
    }

    public void PickNewMeal()
    {
        currentMealOrder = PickRandomMeal();
    }

    public MealData PickRandomMeal()
    {
        int randomIndex = UnityEngine.Random.Range(0, heldCustomerData.possibleMealOrders.Count);

        MealData randomMeal = heldCustomerData.possibleMealOrders[randomIndex];

        return randomMeal;
    }

    public void OnOrderComplete()
    {
        orderComplete = true;
    }
}
