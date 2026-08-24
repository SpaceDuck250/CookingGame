using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/RecipeData")]
public class RecipeData : ScriptableObject
{
    // This is for machinery only
    public FoodData inputFood;
    public FoodData outputFood;

    public FoodData failedOutputFood;

    // For recipe book
    public CookingStation cookingStationUsed;

    public string recipeName;
}

public enum CookingStation
{
    Cut,
    Pan,
    Skewer,
    Vending
}