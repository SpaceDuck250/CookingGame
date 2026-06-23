using UnityEngine;
using System.Collections.Generic;
using System;

public class CookingInputOutputScript : MonoBehaviour
{
    public List<RecipeData> recipeStored = new List<RecipeData>();

    public FoodData input;

    public Action<FoodData> OnCookingStart;
    public Action<Vector3, GameObject> OnCookingEnd;

    public Action<bool> OnFoodInputCorrect;

    public RecipeData currentRecipeUsed;

    // Invisaible and can contain food;
    public GameObject invisiblePickupObject;

    private void Start()
    {
        OnCookingEnd += SpawnPickupableOutputFood;
    }

    private void OnDestroy()
    {
        OnCookingEnd -= SpawnPickupableOutputFood;

    }

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

    public void SpawnPickupableOutputFood(Vector3 spawnPosition, GameObject deleteObject)
    {
        GameObject pickupFood = Instantiate(invisiblePickupObject, spawnPosition, Quaternion.identity);

        HoldableFoodScript holdScript = pickupFood.GetComponent<HoldableFoodScript>();
        holdScript.foodData = currentRecipeUsed.outputFood;

        holdScript.objectToDelete = deleteObject;
    }


}
