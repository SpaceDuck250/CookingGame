using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopItemShowerScript : MonoBehaviour
{
    public GameObject displayObj;
    public Image displayImage;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI displayCost;
    public TextMeshProUGUI amountText;

    public FoodData currentSelectedFood;

    public static Action<float, int, FoodData> OnTryBuyFood;
    public static Action<float, int, FoodData> OnSucessfullyBoughtFood;

    private void Start()
    {
        ShopItemScript.OnSelectFoodItemInShop += DisplayItem;
    }

    private void OnDestroy()
    {
        ShopItemScript.OnSelectFoodItemInShop -= DisplayItem;

    }

    public void DisplayItem(FoodData foodData)
    {
        displayObj.SetActive(true);
        currentSelectedFood = foodData;

        if (foodData.foodSprite != null)
        {
            displayImage.sprite = foodData.foodSprite;
        }

        displayName.text = foodData.foodName;

        float totalCost = 0;
        EditCostAmountText(totalCost);
    }

    public void EditCostAmountText(float newCost)
    {
        displayCost.text = "Total Cost: " + newCost + "$";

    }

    public void EditAmountText(int newBuyAmount)
    {
        amountText.text = "Amount: " + newBuyAmount.ToString();
    }


}
