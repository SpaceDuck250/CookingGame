using UnityEngine;
using TMPro;
using System;

public class ShopIncreaserScript : MonoBehaviour
{
    public static int buyAmount = 0;
    public int changeAmount = 1;

    public static Action<int> OnChangeBuyAmount;

    public ShopItemShowerScript shopSideScript;

    private void Start()
    {
        ShopItemScript.OnSelectFoodItemInShop += ResetBuyAmount;
    }

    private void OnDestroy()
    {
        ShopItemScript.OnSelectFoodItemInShop -= ResetBuyAmount;
    }

    public void ResetBuyAmount(FoodData food)
    {
        buyAmount = 0;
        OnChangeBuyAmount?.Invoke(buyAmount);

        shopSideScript.EditAmountText(buyAmount);
    }

    public void Increment()
    {
        buyAmount += changeAmount;
        if (buyAmount < 0)
        {
            buyAmount = 0;
        }
        OnChangeBuyAmount?.Invoke(buyAmount);

        shopSideScript.EditAmountText(buyAmount);
    }
}
