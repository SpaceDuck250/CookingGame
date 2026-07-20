using UnityEngine;
using TMPro;

public class ShopCostCalculator : MonoBehaviour
{
    public bool canAfford = false;
    public ShopItemShowerScript shopSideScript;

    public TextMeshProUGUI costAmountText;

    private void Start()
    {
        ShopIncreaserScript.OnChangeBuyAmount += CalculateCost;
    }

    private void OnDestroy()
    {
        ShopIncreaserScript.OnChangeBuyAmount -= CalculateCost;

    }

    public void CalculateCost(int buyAmount)
    {
        float totalCost = buyAmount * shopSideScript.currentSelectedFood.costInShop;
        canAfford = totalCost < (float)MoneyManager.PlayerMoneyAmount ? false : true;

        shopSideScript.EditCostAmountText(totalCost);
    }
}
