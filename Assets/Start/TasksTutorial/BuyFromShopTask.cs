using UnityEngine;

public class BuyFromShopTask : TutorialTask
{
    public FoodData foodDataRequired;

    private void Start()
    {
        ShopScript.OnSucessfullyBoughtFood += OnFoodBought;
    }

    private void OnDestroy()
    {
        ShopScript.OnSucessfullyBoughtFood -= OnFoodBought;

    }

    public void OnFoodBought(FoodData foodData, int amount)
    {
        if (foodData == foodDataRequired)
        {
            CompleteTask();
        }
    }
}
