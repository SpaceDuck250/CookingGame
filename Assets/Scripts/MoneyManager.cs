using UnityEngine;
using System;
using Customer;


public class MoneyManager : MonoBehaviour
{
    // Depends on the meal's price and tip depends on the customer's mood + customer's tip range
    public static Action<MealData, CustomerMood, CustomerData> OnPayForOrder;
    public static Action<float> OnMoneyChanged;

    public float PlayerMoneyAmount;

    private void Start()
    {
        OnPayForOrder += PayForOrder;
    }

    private void OnDestroy()
    {
        OnPayForOrder -= PayForOrder;
    }

    public void PayForOrder(MealData mealToPayFor, CustomerMood currentMood, CustomerData customer)
    {
        float tipAmount = CalculateTip(currentMood, customer);
        float foodPayAmount = mealToPayFor.mealPrice;

        float totalPayAmount = foodPayAmount + tipAmount;

        PlayerMoneyAmount += totalPayAmount;

        OnMoneyChanged?.Invoke(PlayerMoneyAmount);
    }

    // This is for testing
    public float CalculateTip(CustomerMood currentMood, CustomerData customer)
    {
        float randomTipAmount = UnityEngine.Random.Range(0f, customer.tipRange);
        // Round to 2dp
        randomTipAmount = Mathf.Round(randomTipAmount * 100) / 100;

        float tipChance = 0;
        if (currentMood == CustomerMood.Normal)
        {
            tipChance = 0.4f;
        }
        else if (currentMood == CustomerMood.Happy)
        {
            tipChance = 0.8f;
        }
        else if (currentMood == CustomerMood.Angry)
        {
            tipChance = 0f;
        }

        if (UnityEngine.Random.value < tipChance)
        {
            return randomTipAmount;
        }
        else
        {
            return 0;
        }
    }
}
