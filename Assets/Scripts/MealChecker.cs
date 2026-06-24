using UnityEngine;
using System.Collections.Generic;

public class MealChecker : MonoBehaviour
{
    public List<FoodData> inputFoodDataList = new List<FoodData>();

    public MealData mealToCheck;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            print(CheckIfMealMatchesOrder());
        }
    }

    public bool CheckIfMealMatchesOrder()
    {
        foreach (FoodData foodIngredient in mealToCheck.foodIngredients)
        {
            if (!inputFoodDataList.Contains(foodIngredient))
            {
                return false;
            }
        }

        int correctIngredientsCount = mealToCheck.foodIngredients.Count;

        if (correctIngredientsCount == inputFoodDataList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
