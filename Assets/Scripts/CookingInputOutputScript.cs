using UnityEngine;
using System.Collections.Generic;
using System;

public class CookingInputOutputScript : Interactable
{
    public List<RecipeData> recipeStored = new List<RecipeData>();

    public FoodData input;

    public Action<FoodData> OnCookingStart;
<<<<<<< HEAD
    public Action<Vector3, GameObject> OnCookingEnd;
=======
    public Action<Vector3, GameObject, Transform> OnCookingSuccess;
    public Action<Vector3, GameObject, Transform> OnCookingFail;

    public Action OnFoodTakenOutOfCookingStation;
>>>>>>> origin/newestAlex

    public Action<bool> OnFoodInputCorrect;

    public RecipeData currentRecipeUsed;

    // Invisaible and can contain food;
    public GameObject invisiblePickupObject;

<<<<<<< HEAD
    private void Start()
    {
        OnCookingEnd += SpawnPickupableOutputFood;
=======
    public bool hasFood = false;

    private void Start()
    {
        OnFoodTakenOutOfCookingStation += TakeFoodOut;
>>>>>>> origin/newestAlex
    }

    private void OnDestroy()
    {
<<<<<<< HEAD
        OnCookingEnd -= SpawnPickupableOutputFood;

    }

    private void Update()
    {
=======
        OnFoodTakenOutOfCookingStation -= TakeFoodOut;
>>>>>>> origin/newestAlex

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
<<<<<<< HEAD
=======
        if (hasFood)
        {
            return;
        }

        if (playerHand.currentFoodHeldObj == null && playerHand.currentFoodHeld == null)
        {
            return;
        }

>>>>>>> origin/newestAlex
        currentRecipeUsed = FindRecipeFromInput(playerHand.currentFoodHeld);
        if (currentRecipeUsed == null)
        {
            return;
        }

        OnCookingStart?.Invoke(currentRecipeUsed.inputFood);
        playerHand.currentFoodHeld = null;
        Destroy(playerHand.currentFoodHeldObj);
<<<<<<< HEAD
    }

    public void SpawnPickupableOutputFood(Vector3 spawnPosition, GameObject deleteObject)
=======

        hasFood = true;
    }

    public GameObject SpawnPickupableOutputFood(Vector3 spawnPosition, GameObject deleteObject, Transform parent, bool success = true)
>>>>>>> origin/newestAlex
    {
        GameObject pickupFood = Instantiate(invisiblePickupObject, spawnPosition, Quaternion.identity);

        HoldableFoodScript holdScript = pickupFood.GetComponent<HoldableFoodScript>();
<<<<<<< HEAD
        holdScript.foodData = currentRecipeUsed.outputFood;

        holdScript.objectToDelete = deleteObject;
=======
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

        holdScript.cookingStationIn = this;

        //pickupFoodStore = pickupFood;
        //spawnedPickupFood = true;

        return pickupFood;
>>>>>>> origin/newestAlex
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
<<<<<<< HEAD
    }

=======

    }

    public void TakeFoodOut()
    {
        hasFood = false;
    }
>>>>>>> origin/newestAlex

}
