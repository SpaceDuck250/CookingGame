using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopItemScript : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI costText;
    public Image foodImage;

    public FoodData foodDataStored;

    public static Action<FoodData> OnSelectFoodItemInShop;

    public void SetItem(string name, float cost, Sprite foodPic, FoodData foodData)
    {
        foodDataStored = foodData;
        itemName.text = name;
        costText.text = "$" + cost.ToString();

        if (foodPic != null)
        {
            foodImage.sprite = foodPic;

            RectTransform rt = foodImage.GetComponent<RectTransform>();

            rt.sizeDelta = new Vector2(80, 80);
        }
    }

    public void SelectItem()
    {
        OnSelectFoodItemInShop?.Invoke(foodDataStored);
    }

}
