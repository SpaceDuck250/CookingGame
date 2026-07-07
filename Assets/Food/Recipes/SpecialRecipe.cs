using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpecialRecipe", menuName = "Scriptable Objects/SpecialRecipe")]

// For stuff requiring more than 1 input foods
public class SpecialRecipe : ScriptableObject
{
    public string recipeName;
    public List<FoodData> inputFoodList = new List<FoodData>();

    public FoodData foodGettingCooked;

    public FoodData sucessOutputFood;
    public FoodData failOuputFood;
}
