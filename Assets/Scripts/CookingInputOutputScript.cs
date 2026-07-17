using UnityEngine;
using System.Collections.Generic;
using System;

public class CookingInputOutputScript : Interactable, ICookStation
{
    public List<RecipeData> recipeStored = new List<RecipeData>();

    public FoodData input;

    public event Action<FoodData> OnCookingStart;
    public event Action<Vector3, GameObject, Transform> OnCookingSuccess;
    public event Action<Vector3, GameObject, Transform> OnCookingFail;

    public event Action OnFoodTakenOutOfCookingStation;

    //public Action<bool> OnFoodInputCorrect;

    public RecipeData currentRecipeUsed;

    // Invisible and can contain food;
    public GameObject invisiblePickupObject;

    public bool hasFood = false;

    private void Start()
    {
        OnFoodTakenOutOfCookingStation += TakeFoodOut;
    }

    private void OnDestroy()
    {
        OnFoodTakenOutOfCookingStation -= TakeFoodOut;

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
        if (hasFood)
        {
            return;
        }

        if (playerHand.currentFoodHeldObj == null && playerHand.currentFoodHeld == null)
        {
            return;
        }

        currentRecipeUsed = FindRecipeFromInput(playerHand.currentFoodHeld);
        if (currentRecipeUsed == null)
        {
            return;
        }

        OnCookingStart?.Invoke(currentRecipeUsed.inputFood);
        playerHand.currentFoodHeld = null;
        Destroy(playerHand.currentFoodHeldObj);

        hasFood = true;
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

        holdScript.cookingStationIn = gameObject;

        //pickupFoodStore = pickupFood;
        //spawnedPickupFood = true;

        return pickupFood;
    }

    // Only for display
    public static GameObject SpawnDisplayFoodInPosition(FoodData foodData, Transform parent, Vector3 localPositionOffset, bool canPickUp, bool useAlternate = false)
    {
        GameObject foodToSpawn;
        if (!useAlternate)
        {
            foodToSpawn = foodData.foodModel;
        }
        else
        {
            foodToSpawn = foodData.usesAlternateFoodModel ? foodData.alternateFoodModel : foodData.foodModel;
        }

        GameObject newDisplayFood = Instantiate(foodToSpawn, parent.position, foodToSpawn.transform.rotation);

        newDisplayFood.transform.SetParent(parent.transform, true);

        newDisplayFood.GetComponent<Rigidbody>().isKinematic = true;
        newDisplayFood.GetComponent<Collider>().isTrigger = true;

        newDisplayFood.transform.localPosition = localPositionOffset;

        if (!canPickUp)
        {
            Destroy(newDisplayFood.GetComponent<Collider>());
        }

        //if (clearHand)
        //{
        //    PlayerHandScript.instance.currentFoodHeld = null;
        //    Destroy(PlayerHandScript.instance.currentFoodHeldObj);
        //}


        return newDisplayFood;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        TryPutFood(playerHand);

    }

    public void TakeFoodOut()
    {
        hasFood = false;
    }

    public void CallFoodSuccessEvent(Vector3 spawnPos, GameObject displayObj, Transform parent)
    {
        OnCookingSuccess?.Invoke(spawnPos, displayObj, parent);
    }

    public void CallFoodFailEvent(Vector3 spawnPos, GameObject displayObj, Transform parent)
    {
        OnCookingFail?.Invoke(spawnPos, displayObj, parent);
    }

    public void CallFoodTakenOutEvent()
    {
        OnFoodTakenOutOfCookingStation.Invoke();
    }

}
