using UnityEngine;
using System.Collections.Generic;

public enum MealType
{
    Chicken,
    Beef,
    Fish,
    Egg,
    Vegetable,
    Dessert,
    Drink
}

[CreateAssetMenu(fileName = "MealData", menuName = "Scriptable Objects/MealData")]
public class MealData : ScriptableObject
{
    public int mealID;
    public string mealName;

    public MealType mealType;

    // When food is served check if all food matches the food ingredients
    public List<FoodData> foodIngredients = new List<FoodData>();
    public float mealPrice;
}
