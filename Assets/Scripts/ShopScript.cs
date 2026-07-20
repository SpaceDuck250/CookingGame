using UnityEngine;
using System.Collections.Generic;
using System;

public class ShopScript : MonoBehaviour
{
    // Only raw ingredients
    public List<FoodData> soldFoodList = new List<FoodData>();

    public GameObject uiItemPrefab;
    public Transform spawnParent;

    public GameObject shopObj;

    public static Action OnShopOpen;
    public static Action OnShopClose;


    private void Start()
    {
        OnShopOpen += OpenShop;

        SpawnAllFoodsUI();
    }

    private void OnDestroy()
    {
        OnShopOpen -= OpenShop;
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

    public void CloseShop()
    {
        shopObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenShop()
    {
        shopObj.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
