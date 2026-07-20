using UnityEngine;
using System.Collections.Generic;

public class ShopScript : MonoBehaviour
{
    // Only raw ingredients
    public List<FoodData> soldFoodList = new List<FoodData>();

    public GameObject uiItemPrefab;
    public Transform spawnParent;

    private void Start()
    {
        SpawnAllFoodsUI();
    }

    public void SpawnAllFoodsUI()
    {
        foreach (FoodData food in soldFoodList)
        {
            GameObject newUiFoodObj = Instantiate(uiItemPrefab, spawnParent);
            ShopItemScript newShopItem = newUiFoodObj.GetComponent<ShopItemScript>();

            newShopItem.SetItem(food.foodName, food.costInShop, food.foodSprite, food);
        }
    }
}
