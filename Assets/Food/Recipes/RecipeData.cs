using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/RecipeData")]
public class RecipeData : ScriptableObject
{
    // This is for machinery only
    public FoodData inputFood;
    public FoodData outputFood;
}
