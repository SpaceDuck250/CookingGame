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

    public ShopCostCalculator shopCostScript;
    public ShopItemShowerScript shopSideScript;

    public static Action<FoodData, int> OnSucessfullyBoughtFood;

    public int maxBuyAmount = 5;

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

        PlayerStateManager.UnPauseGame();
    }

    public void OpenShop()
    {
        shopObj.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerStateManager.PauseGame();
    }

    public void TryBuy()
    {
        if (!shopCostScript.canAfford)
        {
            return;
        }

        if (ShopIncreaserScript.buyAmount == 0)
        {
            return;
        }

        if (ShopIncreaserScript.buyAmount > maxBuyAmount)
        {
            return;
        }

        float totalCost = -1 * shopCostScript.totalCost;
        MoneyManager.instance.ChangeMoneyAmount((decimal)totalCost);

        OnSucessfullyBoughtFood?.Invoke(shopSideScript.currentSelectedFood, ShopIncreaserScript.buyAmount);

    }
}
