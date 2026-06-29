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
    public TextMeshProUGUI dialogueText;

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

    public void TalkToCustomer(CustomerData newCustomer, MealData pickedMeal)
    {
        heldCustomerData = newCustomer;

        dialogueText.text = heldCustomerData.customerName + ": Hello I would like... " + pickedMeal.mealName;
        dialogueObject.SetActive(true);
    }

    public void StopTalkToCustomer()
    {
        dialogueObject.SetActive(false);
    }

    public void OnOrderMetTalkFunction()
    {
        dialogueText.text = heldCustomerData.customerName + ": Thank you that is the correct meal!";
        dialogueObject.SetActive(true);
    }
}
