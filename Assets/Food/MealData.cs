using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MealData", menuName = "Scriptable Objects/MealData")]
public class NewScriptableObjectScript : ScriptableObject
{
    public int mealID;
    public string mealName;

    // When food is served check if all food matches the food ingredients
    public List<FoodData> foodIngredients = new List<FoodData>();
    public float mealPrice;
}
