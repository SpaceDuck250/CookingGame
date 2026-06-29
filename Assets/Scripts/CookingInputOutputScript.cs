using UnityEngine;
using System.Collections.Generic;
using System;

public class CookingInputOutputScript : Interactable
{
    public List<RecipeData> recipeStored = new List<RecipeData>();

    public FoodData input;

    public Action<FoodData> OnCookingStart;
    public Action<Vector3, GameObject, Transform> OnCookingSuccess;
    public Action<Vector3, GameObject, Transform> OnCookingFail;

    public Action<bool> OnFoodInputCorrect;

    public RecipeData currentRecipeUsed;

    // Invisaible and can contain food;
    public GameObject invisiblePickupObject;

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

    public void TryPutFood(PlayerHandScript playerHand)
    {
        currentRecipeUsed = FindRecipeFromInput(playerHand.currentFoodHeld);
        if (currentRecipeUsed == null)
        {
            return;
        }

        OnCookingStart?.Invoke(currentRecipeUsed.inputFood);
        playerHand.currentFoodHeld = null;
        Destroy(playerHand.currentFoodHeldObj);
    }

    public GameObject SpawnPickupableOutputFood(Vector3 spawnPosition, GameObject deleteObject, Transform parent, bool success = true)
    {
        GameObject pickupFood = Instantiate(invisiblePickupObject, spawnPosition, Quaternion.identity);

        HoldableFoodScript holdScript = pickupFood.GetComponent<HoldableFoodScript>();
        if (success)
        {
            holdScript.foodData = currentRecipeUsed.outputFood;
        }
        else
        {
            holdScript.foodData = currentRecipeUsed.failedOutputFood;
        }

        holdScript.objectToDelete = deleteObject;

        pickupFood.transform.parent = parent;

        return pickupFood;
    }

    // Only for display
    public static GameObject SpawnDisplayFoodInPosition(FoodData foodData, Transform parent, Vector3 localPositionOffset, bool canPickUp)
    {
        GameObject newDisplayFood = Instantiate(foodData.foodModel, parent.position, Quaternion.identity);

        newDisplayFood.transform.SetParent(parent.transform, true);

        newDisplayFood.GetComponent<Rigidbody>().isKinematic = true;
        newDisplayFood.GetComponent<Collider>().isTrigger = true;

        newDisplayFood.transform.localPosition = localPositionOffset;

        if (!canPickUp)
        {
            Destroy(newDisplayFood.GetComponent<Collider>());
        }

        PlayerHandScript.instance.currentFoodHeld = null;
        Destroy(PlayerHandScript.instance.currentFoodHeldObj);

        return newDisplayFood;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        TryPutFood(playerHand);
    }


}
