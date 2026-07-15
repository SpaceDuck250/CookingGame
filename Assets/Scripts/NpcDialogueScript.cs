using System;
using TMPro;
using UnityEngine;
using Customer;
using System.Linq;
using Pair;
using System.Collections.Generic;

public class NpcDialogueScript : MonoBehaviour
{
    public static Action<CustomerData, MealData, CustomerMood> OnTalkToCustomer;
    public static Action OnEndTalkToCustomer;
    public static Action<CustomerData> OnOrderMetTalk;

    public CustomerData heldCustomerData;

    public GameObject dialogueObject;
    public TextMeshProUGUI dialogueTextComponent;

    public static bool conversationOpen = false;

    public SlowTyper slowTyper;

    // Add slowtalk later

    private void Start()
    {
        OnTalkToCustomer += TalkToCustomer;
        OnEndTalkToCustomer += StopTalkToCustomer;
        OnOrderMetTalk += OnOrderMetTalkFunction;

    }

    private void OnDestroy()
    {
        OnTalkToCustomer -= TalkToCustomer;
        OnEndTalkToCustomer -= StopTalkToCustomer;
        OnOrderMetTalk -= OnOrderMetTalkFunction;
    }

    public void WriteNewText(string name, string newText)
    {
        conversationOpen = true;
        slowTyper.StartWritingSlowly(name, newText);
        dialogueObject.SetActive(true);

    }

    public void TalkToCustomer(CustomerData newCustomer, MealData pickedMeal, CustomerMood mood)
    {
        heldCustomerData = newCustomer;

        string randomLineFromCustomer = PickRandomLine(newCustomer, mood);

        WriteNewText(GetName(newCustomer), randomLineFromCustomer);

    }

    public void StopTalkToCustomer()
    {
        conversationOpen = false;

        dialogueObject.SetActive(false);
    }

    public void OnOrderMetTalkFunction(CustomerData customer)
    {
        heldCustomerData = customer;
        WriteNewText(GetName(customer) , "Thank you that is the correct meal!");

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
        return customer.name + ": ";
    }
}
