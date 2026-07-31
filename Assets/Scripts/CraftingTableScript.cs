using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class CraftingTableScript : Interactable
{
    public List<FoodData> foodInputList = new List<FoodData>();

    public List<SpecialRecipe> specialRecipeList = new List<SpecialRecipe>();
    public int currentCycleIndex = 0;

    public SpecialRecipe currentRecipeUsed;

    public FoodData outputFood;

    public bool craftingMode = false;

    public event Action OnRecipeReady;
    public event Action OnOuputDispensed;
    public event Action<SpecialRecipe> OnCycleThroughRecipe;

    public Transform spawnParent;
    public float downScaleAmount = 1f;

    private void Start()
    {
        CycleThroughRecipeList();
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeld == null)
        {
            return;
        }

        TryInputFood(playerHand);
    }

    public void TryInputFood(PlayerHandScript playerHand)
    {
        FoodData inputFood = playerHand.currentFoodHeld;

        if (foodInputList.Count >= 3)
        {
            return;
        }

        foreach (FoodData foodNeeded in currentRecipeUsed.foodsNeededForRecipe)
        {
            if (inputFood == foodNeeded)
            {
                craftingMode = true;

                foodInputList.Add(inputFood);
                playerHand.ClearFoodFromHand();

                if (CheckIfRecipeMet())
                {
                    FinishRecipe();
                }
            }
        }

    }

    public bool CheckIfRecipeMet()
    {
        // Basically just checks if both arrays are the same if ordered

        return foodInputList.Select(f => f.foodName).OrderBy(name => name).SequenceEqual(
        currentRecipeUsed.foodsNeededForRecipe.Select(f => f.foodName).OrderBy(name => name));
    }

    public void FinishRecipe()
    {
        outputFood = currentRecipeUsed.outputFood;

        OutputFoodResult();

    }

    public void OutputFoodResult()
    {
        foodInputList.Clear();

        GameObject newFoodObj = CookingInputOutputScript.SpawnDisplayFoodInPosition(outputFood, spawnParent, Vector3.zero, true, false, downScaleAmount);
        newFoodObj.GetComponent<Rigidbody>().isKinematic = false;
        newFoodObj.GetComponent<Collider>().isTrigger = false;
    }

    public void CycleThroughRecipeList()
    {
        if (craftingMode)
        {
            return;
        }

        currentCycleIndex++;
        if (currentCycleIndex >= specialRecipeList.Count)
        {
            currentCycleIndex = 0;
        }

        currentRecipeUsed = specialRecipeList[currentCycleIndex];

        OnCycleThroughRecipe?.Invoke(currentRecipeUsed);
    }


}
