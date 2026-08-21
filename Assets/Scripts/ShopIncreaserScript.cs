using UnityEngine;
using TMPro;
using System;

public class ShopIncreaserScript : MonoBehaviour
{
    public static int buyAmount = 0;
    public int changeAmount = 1;

    public static int maxBuyAmount = 5;

    public static Action<int> OnChangeBuyAmount;

    public ShopItemShowerScript shopSideScript;

    private void Start()
    {
        ShopItemScript.OnSelectFoodItemInShop += ResetBuyAmount;
        ShopScript.OnSucessfullyBoughtFood += Reset;
    }

    private void OnDestroy()
    {
        ShopItemScript.OnSelectFoodItemInShop -= ResetBuyAmount;
        ShopScript.OnSucessfullyBoughtFood -= Reset;

    }

    public void ResetBuyAmount(FoodData food)
    {
        buyAmount = 0;
        OnChangeBuyAmount?.Invoke(buyAmount);

        shopSideScript.EditAmountText(buyAmount);
    }

    public void Reset(FoodData food, int amount)
    {
        ResetBuyAmount(food);
    }

    public void Increment()
    {
        if (buyAmount + changeAmount > maxBuyAmount)
        {
            buyAmount = maxBuyAmount;
            return;
        }

        buyAmount += changeAmount;
        if (buyAmount < 0)
        {
            buyAmount = 0;
        }
        OnChangeBuyAmount?.Invoke(buyAmount);

        shopSideScript.EditAmountText(buyAmount);
    }
}
