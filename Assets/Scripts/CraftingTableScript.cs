using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

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

    public Transform returnPoint;
    public bool busyReturning = false;

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

        craftingMode = false;

    }

    public void OutputFoodResult()
    {
        foodInputList.Clear();

        GameObject newFoodObj = CookingInputOutputScript.SpawnDisplayFoodInPosition(outputFood, spawnParent, Vector3.zero, true, false, downScaleAmount);
        newFoodObj.GetComponent<Rigidbody>().isKinematic = false;
        newFoodObj.GetComponent<Collider>().isTrigger = false;

        OnOuputDispensed?.Invoke();
    }

    public void CycleThroughRecipeList(int amount)
    {
        if (craftingMode)
        {
            return;
        }

        currentCycleIndex += amount;
        if (currentCycleIndex >= specialRecipeList.Count)
        {
            currentCycleIndex = 0;
        }
        else if (currentCycleIndex < 0)
        { 
            currentCycleIndex = specialRecipeList.Count - 1;
        }

        currentRecipeUsed = specialRecipeList[currentCycleIndex];

        OnCycleThroughRecipe?.Invoke(currentRecipeUsed);
    }

    public IEnumerator ReturnAllInputFoodBack()
    {
        busyReturning = true;
        float waitTime = 0.5f;
        foreach (FoodData food in foodInputList)
        {
            GameObject newFoodObj = CookingInputOutputScript.SpawnDisplayFoodInPosition(food, returnPoint, Vector3.zero, true, false, downScaleAmount);
            newFoodObj.GetComponent<Rigidbody>().isKinematic = false;
            newFoodObj.GetComponent<Collider>().isTrigger = false;

            yield return new WaitForSeconds(waitTime);
        }

        foodInputList.Clear();
        craftingMode = false;
        busyReturning = false;


    }

}
