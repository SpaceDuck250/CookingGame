using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpecialRecipe", menuName = "Scriptable Objects/SpecialRecipe")]
public class SpecialRecipe : ScriptableObject
{
    public string recipeName;

    public Sprite recipeSprite;

    public List<FoodData> foodsNeededForRecipe = new List<FoodData>();

    public FoodData outputFood;
}
