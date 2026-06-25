using UnityEngine;
using System.Collections.Generic;
using System;

public class CookingInputOutputScript : Interactable
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

    public void SpawnPickupableOutputFood(Vector3 spawnPosition, GameObject deleteObject)
    {
        GameObject pickupFood = Instantiate(invisiblePickupObject, spawnPosition, Quaternion.identity);

        HoldableFoodScript holdScript = pickupFood.GetComponent<HoldableFoodScript>();
        holdScript.foodData = currentRecipeUsed.outputFood;

        holdScript.objectToDelete = deleteObject;
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
