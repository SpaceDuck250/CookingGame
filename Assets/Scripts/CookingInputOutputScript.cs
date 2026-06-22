using UnityEngine;
using System.Collections.Generic;
using System;

public class CookingInputOutputScript : MonoBehaviour
{
    public List<RecipeData> recipeStored = new List<RecipeData>();

    public FoodData input;

    public Action<FoodData> OnCookingStart;
    public Action<FoodData> OnCookingEnd;

    public Action<bool> OnFoodInputCorrect;

    public RecipeData currentRecipeUsed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            print(FindRecipeFromInput(input));
        }
    }

    public RecipeData FindRecipeFromInput(FoodData foodInput)
    {
        foreach (RecipeData recipe in recipeStored)
        {
            if (recipe.inputFood == foodInput)
            {
                return recipe;
            }
        }

        return null;
    }

    public void TryPutFood(FoodData foodInput, PlayerHandScript playerHand)
    {
        currentRecipeUsed = FindRecipeFromInput(foodInput);
        if (currentRecipeUsed == null)
        {
            return;
        }

        OnCookingStart?.Invoke(currentRecipeUsed.inputFood);
        playerHand.currentFoodHeld = null;
        Destroy(playerHand.currentFoodHeldObj);

    }


}
