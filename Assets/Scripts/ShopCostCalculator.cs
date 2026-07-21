using UnityEngine;
using TMPro;

public class ShopCostCalculator : MonoBehaviour
{
    public bool canAfford = false;
    public ShopItemShowerScript shopSideScript;

    public float totalCost;

    public TextMeshProUGUI costAmountText;

    private void Start()
    {
        ShopIncreaserScript.OnChangeBuyAmount += CalculateCost;
        ShopItemScript.OnSelectFoodItemInShop += ResetValues;
    }

    private void OnDestroy()
    {
        ShopIncreaserScript.OnChangeBuyAmount -= CalculateCost;
        ShopItemScript.OnSelectFoodItemInShop -= ResetValues;


    }

    public void CalculateCost(int buyAmount)
    {
        totalCost = buyAmount * shopSideScript.currentSelectedFood.costInShop;
        canAfford = (decimal)totalCost < MoneyManager.playerMoneyAmount ? true : false;

        totalCost = (float)System.Math.Round((double)totalCost, 2);

        shopSideScript.EditCostAmountText((decimal)totalCost);
    }

    public void ResetValues(FoodData food)
    {
        canAfford = false;
        totalCost = 0;
    }
}
