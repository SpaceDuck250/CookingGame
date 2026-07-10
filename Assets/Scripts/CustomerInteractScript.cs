using UnityEngine;
using System.Collections.Generic;
using System;
using Customer;

public class CustomerInteractScript : Interactable
{
    public CustomerData heldCustomerData;

    //public MealData currentMealOrder;

    public event Action<MealData> OnNewMealPicked;

    public MealChecker mealChecker;

    public bool orderComplete = false;
    public CustomerMovementScript movementScript;

    public Action OnInteractWithCustomer;
    public static Action OnEndInteractWithCustomer;

    public bool finishedInteract = false;

    public CustomerStateMachine customerStateMachine;

    public bool TalkedTo = false;
    
    private void Start()
    {
        //PickNewMeal();

        mealChecker.OnMealOrderFulfilled += OnOrderComplete;
    }

    private void OnDestroy()
    {
        mealChecker.OnMealOrderFulfilled -= OnOrderComplete;

    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (finishedInteract)
        {
            return;
        }

        if (movementScript.agent.isStopped)
        {
            RotateToPlayer();
        }

        if (CheckIfHoldingFood(playerHand) && TalkedTo)
        {
            CheckIfFoodMatchesOrder(playerHand);
        }
        else if (!CheckIfHoldingFood(playerHand))
        {
            TryTalkToCustomer();
        }
    }

    public bool CheckIfHoldingFood(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeldObj == null && playerHand.currentFoodHeld == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void CheckIfFoodMatchesOrder(PlayerHandScript playerHand)
    {
        mealChecker.CheckOrder(playerHand);
    }

    private void TryTalkToCustomer()
    {
        TalkedTo = true;

        if (!NpcDialogueScript.conversationOpen)
        {
            OpenConversation();
            customerStateMachine.OnCustomerChangeState?.Invoke(CustomerState.WaitingForFood);
        }
        else
        {
            CloseConversation();
        }
    }

    public void OpenConversation()
    {
        OnInteractWithCustomer?.Invoke();

        NpcDialogueScript.OnTalkToCustomer?.Invoke(heldCustomerData, mealChecker.mealToCheck);
    }

    public void CloseConversation()
    {
        print("ended" + gameObject);
        OnEndInteractWithCustomer?.Invoke();

        NpcDialogueScript.OnEndTalkToCustomer?.Invoke();
        if (orderComplete)
        {
            // Be more nuanced later
            customerStateMachine.OnCustomerChangeState(CustomerState.WalkingToSeat);
            CustomerSpawnerScript.OnCustomerLeftQueue?.Invoke(customerStateMachine);

            finishedInteract = true;
        }
    }

    public void RotateToPlayer()
    {
        Vector3 rotateVector = (PlayerHandScript.instance.transform.position - transform.position).normalized;
        float rotateAngle = Mathf.Atan2(rotateVector.x, rotateVector.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, rotateAngle, 0);
    }

    public void OnOrderComplete()
    {
        orderComplete = true;
    }
}
