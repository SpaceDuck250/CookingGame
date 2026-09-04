using System;
using TMPro;
using UnityEngine;
using Customer;
using System.Linq;
using Pair;
using System.Collections.Generic;

public class NpcDialogueScript : MonoBehaviour
{
    public static Action<CustomerData, MealData, CustomerMood> OnShowDialogue;
    public static Action OnHideDialogue;
    public static Action<CustomerData> OnOrderMetDialogue;

    public static Action<CustomerData, bool> OnWrongMealServedDialogue;

    public CustomerData heldCustomerData;

    public GameObject dialogueObject;
    public TextMeshProUGUI dialogueTextComponent;

    public static bool conversationOpen = false;

    public SlowTyper slowTyper;

    public CustomerStateMachine currentCustomer;

    // Add slowtalk later

    private void Start()
    {
        OnShowDialogue += TalkToCustomer;
        OnHideDialogue += StopTalkToCustomer;
        OnOrderMetDialogue += OnOrderMetTalkFunction;

        OnWrongMealServedDialogue += TalkWrongMeal;

    }

    private void OnDestroy()
    {
        OnShowDialogue -= TalkToCustomer;
        OnHideDialogue -= StopTalkToCustomer;
        OnOrderMetDialogue -= OnOrderMetTalkFunction;

        OnWrongMealServedDialogue -= TalkWrongMeal;

    }

    public void WriteNewText(string name, string newText, Sprite customerSprite = null)
    {
        conversationOpen = true;
        slowTyper.StartWritingSlowly(name, newText, transform, customerSprite);
        dialogueObject.SetActive(true);

    }

    public void TalkToCustomer(CustomerData newCustomer, MealData pickedMeal, CustomerMood mood)
    {
        //PauseGameScript.uiAlreadyOverlayed = true;

        heldCustomerData = newCustomer;

        string randomLineFromCustomer = PickRandomLine(newCustomer, mood);

        WriteNewText(GetName(newCustomer), randomLineFromCustomer, newCustomer.customerSprite);

    }

    public void StopTalkToCustomer()
    {
        //PauseGameScript.uiAlreadyOverlayed = false;

        conversationOpen = false;

        //dialogueObject.SetActive(false);
        slowTyper.CloseDialogue();

        CustomerInteractScript.OnEndInteractWithCustomer?.Invoke();
    }

    public void OnOrderMetTalkFunction(CustomerData customer)
    {
        heldCustomerData = customer;
        WriteNewText(GetName(customer), "Thank you that is the correct meal!", customer.customerSprite);

    }

    public void TalkWrongMeal(CustomerData customer, bool burntFood)
    {
        if (conversationOpen)
        {
            StopTalkToCustomer();
            CustomerInteractScript.OnEndInteractWithCustomer?.Invoke();

            return;
        }

        heldCustomerData = customer;

        // Make sure they arent null
        string lineToShow = burntFood ? customer.burntFoodDialogueLine : customer.wrongFoodDialogueLine;
        WriteNewText(GetName(customer), lineToShow, customer.customerSprite);
    }

    public string PickRandomLine(CustomerData customer, CustomerMood currentMood)
    {
        if (customer.dialogueLines.Count == 0)
        {
            return "This customer has no lines, please add some clown";
        }

        List<LineMoodPair> moodMatchedLines = customer.dialogueLines.Where(n => n.moodType == currentMood).ToList<LineMoodPair>();

        if (moodMatchedLines.Count == 0)
        {
            return "This customer is missing lines when it is in this mood.." + currentMood.ToString();
        }

        int randomInt = UnityEngine.Random.Range(0, moodMatchedLines.Count);

        string randomLine = moodMatchedLines[randomInt].dialogueLine;

        return randomLine;
    }

    public string GetName(CustomerData customer)
    {
        return customer.customerName + ": ";
    }
}