using UnityEngine;
using System.Collections.Generic;
using System;
using Customer;
using UnityEditorInternal;

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

    public bool talkedTo = false;

    public bool talkingTo = false;
    
    private void Start()
    {
        //PickNewMeal();

        mealChecker.OnMealOrderFulfilled += OnOrderComplete;
        OnEndInteractWithCustomer += NotTalking;
        OnInteractWithCustomer += Talking;
    }

    private void OnDestroy()
    {
        mealChecker.OnMealOrderFulfilled -= OnOrderComplete;
        OnEndInteractWithCustomer -= NotTalking;
        OnInteractWithCustomer -= Talking;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (finishedInteract || !movementScript.agent.isStopped)
        {
            return;
        }

        if (movementScript.agent.isStopped)
        {
            RotateToPlayer();
        }

        if (CheckIfHoldingFood(playerHand) && talkedTo)
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
        talkedTo = true;

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

        NpcDialogueScript.OnTalkToCustomer?.Invoke(heldCustomerData, mealChecker.mealToCheck, customerStateMachine.currentMood);
    }

    public void CloseConversation()
    {
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

    public void NotTalking()
    {
        talkingTo = false;
    }

    public void Talking()
    {
        talkingTo = true;
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
