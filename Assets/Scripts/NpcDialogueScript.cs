using System;
using TMPro;
using UnityEngine;

public class NpcDialogueScript : MonoBehaviour
{
    public static Action<CustomerData, MealData> OnTalkToCustomer;
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

    public void TalkToCustomer(CustomerData newCustomer, MealData pickedMeal)
    {
        heldCustomerData = newCustomer;

        string randomLineFromCustomer = PickRandomLine(newCustomer);

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

    public string PickRandomLine(CustomerData customer)
    {
        if (customer.possibleDialogueLines.Count == 0)
        {
            return "This customer has no lines, please add some clown";
        }

        int randomInt = UnityEngine.Random.Range(0, customer.possibleDialogueLines.Count);

        string randomLine = customer.possibleDialogueLines[randomInt];

        return randomLine;
    }

    public string GetName(CustomerData customer)
    {
        return customer.name + ": ";
    }
}
