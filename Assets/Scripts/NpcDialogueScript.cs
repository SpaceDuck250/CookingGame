using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcDialogueScript : MonoBehaviour
{
    public static Action<CustomerData, MealData> OnTalkToCustomer;
    public static Action OnEndTalkToCustomer;
    public static Action OnOrderMetTalk;

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

        WriteNewText(heldCustomerData.customerName + ": Hello I would like... " + pickedMeal.mealName);
    }

    public void StopTalkToCustomer()
    {
        conversationOpen = false;

        dialogueObject.SetActive(false);
    }

    public void OnOrderMetTalkFunction()
    {
        WriteNewText(heldCustomerData.customerName + ": Thank you that is the correct meal!");
    }
}
