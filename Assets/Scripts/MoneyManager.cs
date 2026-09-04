using UnityEngine;
using System;
using Customer;

public class MoneyManager : MonoBehaviour
{
    // Depends on the meal's price and tip depends on the customer's mood + customer's tip range
    public static Action<MealData, CustomerMood, CustomerData> OnPayForOrder;
    public static Action<decimal, decimal, decimal> OnMoneyChanged;

    public static decimal playerMoneyAmount;

    //public static decimal moneyStartAmount = 100;

    public static MoneyManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {


        SetMoney(moneyStartAmount);
        //moneyStartAmount = 100;
        //ChangeMoneyAmount(moneyStartAmount);
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

        playerMoneyAmount += totalPayAmount;

        decimal totalMoneyAmount = playerMoneyAmount;
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

    public void ChangeMoneyAmount(decimal change)
    {
        playerMoneyAmount += (decimal)change;

        //print("LP");

        OnMoneyChanged?.Invoke(playerMoneyAmount, (decimal)change, 0);

    }

    public void SetMoney(decimal newAmount)
    {
        playerMoneyAmount = (decimal)newAmount;

        OnMoneyChanged?.Invoke(playerMoneyAmount, (decimal)newAmount, 0);
    }
}
