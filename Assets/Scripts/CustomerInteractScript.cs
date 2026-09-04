using UnityEngine;
using System.Collections.Generic;
using System;
using Customer;
using JetBrains.Annotations;

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

    // For UI;
    public static Action<CustomerStateMachine> OnAnyCustomerInteract;

    public bool finishedInteract = false;

    public CustomerStateMachine customerStateMachine;

    public bool talkedTo = false;

    public bool talkingTo = false;

    public static Action<CustomerInteractScript> OnCheckIfNeedToLeave;

    public TalkRangeScript talkRange;

    private void Start()
    {
        //PickNewMeal();

        mealChecker.OnMealOrderFulfilled += OnOrderComplete;
        OnEndInteractWithCustomer += NotTalking;
        OnInteractWithCustomer += Talking;

        customerStateMachine.OnCustomerMoodChange += OnCustomerMoodChange;

        OnCheckIfNeedToLeave += TryLeaveSelf;
    }

    private void OnDestroy()
    {
        mealChecker.OnMealOrderFulfilled -= OnOrderComplete;
        OnEndInteractWithCustomer -= NotTalking;
        OnInteractWithCustomer -= Talking;

        customerStateMachine.OnCustomerMoodChange -= OnCustomerMoodChange;

        OnCheckIfNeedToLeave -= TryLeaveSelf;


    }

    public override void Interact(PlayerHandScript playerHand)
    {

        if (finishedInteract || !movementScript.agent.isStopped || !talkRange.inRange)
        {
            return;
        }

        //if (movementScript.agent.isStopped)
        //{
        //    //RotateToPlayer();
        //    //RotateTo(playerHand.gameObject);
        //}

        if (CheckIfHoldingFood(playerHand) && talkedTo)
        {
            //CloseConversation();
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
            // Check this specific customer when the player talks to them
            // Normal customers will not have AuntMerryCustomerScript
            AuntMerryCustomerScript auntMerryScript = GetComponent<AuntMerryCustomerScript>();

            if (auntMerryScript == null)
            {
                auntMerryScript = GetComponentInParent<AuntMerryCustomerScript>();
            }

            if (auntMerryScript == null)
            {
                auntMerryScript = GetComponentInChildren<AuntMerryCustomerScript>();
            }

            // Only Aunt Merry will enter this section.
            if (auntMerryScript != null)
            {
                auntMerryScript.TryChangeMindWhenTalkedTo();
            }

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
        if (customerStateMachine != CustomerStateMachine.customerTakingOrder && CustomerStateMachine.customerTakingOrder != null)
        {
            return;
        }

        OnInteractWithCustomer?.Invoke();
        OnAnyCustomerInteract?.Invoke(customerStateMachine);

        NpcDialogueScript.OnShowDialogue?.Invoke(heldCustomerData, mealChecker.mealToCheck, customerStateMachine.currentMood);

        //OnCheckIfNeedToLeave?.Invoke(this);

    }

    public void CloseConversation()
    {
        //CancelInvoke("CloseConversation");

        //RotateTo(CustomerSpawnerScript.instance.mainCounterPoint.gameObject);
        OnEndInteractWithCustomer?.Invoke();

        NpcDialogueScript.OnHideDialogue?.Invoke();
        if (orderComplete && !finishedInteract)
        {

            customerStateMachine.OnCustomerChangeState(CustomerState.WalkingToSeat);
            CustomerSpawnerScript.OnCustomerLeftQueue?.Invoke(customerStateMachine);

            finishedInteract = true;
        }

        //OnCheckIfNeedToLeave?.Invoke(this);
    }

    public void NotTalking()
    {
        talkingTo = false;
    }

    public void Talking()
    {
        talkingTo = true;
    }

    public void OnCustomerMoodChange(CustomerMood mood)
    {
        if (mood == CustomerMood.ReallyAngry)
        {
            finishedInteract = true;
        }
    }

    public void RotateTo(GameObject obj)
    {
        Vector3 rotateVector = (obj.transform.position - transform.position).normalized;
        float rotateAngle = Mathf.Atan2(rotateVector.x, rotateVector.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, rotateAngle, 0);
    }

    public void OnOrderComplete()
    {
        orderComplete = true;
        OnInteractWithCustomer?.Invoke();
    }

    // Comes from others
    public void TryLeaveSelf(CustomerInteractScript interactScript)
    {
        if (interactScript != this)
        {
            if (orderComplete && !finishedInteract)
            {
                // Be more nuanced later
                customerStateMachine.OnCustomerChangeState(CustomerState.WalkingToSeat);
                CustomerSpawnerScript.OnCustomerLeftQueue?.Invoke(customerStateMachine);

                finishedInteract = true;
            }
        }
    }
}
