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

    public void WriteNewText(string newText)
    {
        conversationOpen = true;
        dialogueTextComponent.text = newText;
        dialogueObject.SetActive(true);

    }

    public void TalkToCustomer(CustomerData newCustomer, MealData pickedMeal)
    {
        heldCustomerData = newCustomer;

        string randomLineFromCustomer = PickRandomLine(newCustomer);

        WriteNewText(randomLineFromCustomer);
    }

    public void StopTalkToCustomer()
    {
        conversationOpen = false;

        dialogueObject.SetActive(false);
    }

    public void OnOrderMetTalkFunction(CustomerData customer)
    {
        heldCustomerData = customer;
        WriteNewText(heldCustomerData.customerName + ": Thank you that is the correct meal!");
    }

    public string PickRandomLine(CustomerData customer)
    {
        if (customer.possibleDialogueLines.Count == 0)
        {
            return "This customer has no lines, please add some clown";
        }

        int randomInt = UnityEngine.Random.Range(0, customer.possibleDialogueLines.Count);

        string randomLine = customer.possibleDialogueLines[randomInt];

        randomLine = customer.customerName + ": " + randomLine;

        return randomLine;
    }
}
