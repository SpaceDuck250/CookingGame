using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MealData", menuName = "Scriptable Objects/MealData")]
public class NewScriptableObjectScript : ScriptableObject
{
    public int mealID;
    public string mealName;
    public List<FoodData> foodIngredients = new List<FoodData>();
    public float mealPrice;
}
