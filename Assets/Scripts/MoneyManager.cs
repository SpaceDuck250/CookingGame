using UnityEngine;
using System;
using Customer;

public class MoneyManager : MonoBehaviour
{
    // Depends on the meal's price and tip depends on the customer's mood + customer's tip range
    public static Action<MealData, CustomerMood, CustomerData> OnPayForOrder;
    public static Action<decimal, decimal, decimal> OnMoneyChanged;

    public decimal PlayerMoneyAmount;

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
        decimal tipAmount = CalculateTip(currentMood, customer);
        decimal foodPayAmount = (decimal)mealToPayFor.mealPrice;

        decimal totalPayAmount = foodPayAmount + tipAmount;

        PlayerMoneyAmount += totalPayAmount;

        decimal totalMoneyAmount = PlayerMoneyAmount;
        decimal earnedAmount = foodPayAmount;

        OnMoneyChanged?.Invoke(totalMoneyAmount, earnedAmount, tipAmount);
    }

    // This is for testing
    public decimal CalculateTip(CustomerMood currentMood, CustomerData customer)
    {
        float randomTipAmount = UnityEngine.Random.Range(0f, customer.tipRange);
        // Round to 2dp
        decimal roundedTipAmount = Math.Round((decimal)randomTipAmount, 2);

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
            return roundedTipAmount;
        }
        else
        {
            return 0;
        }
    }
}
