using UnityEngine;
using System;

public class CarrotCutter : Interactable
{
    public bool canCut;

    public GameObject cutObj;
    public FoodData cutFoodData;

    public CookingInputOutputScript cookingInputOutput;

    public Transform foodSpawnArea;
    public Vector3 spawnOffset;

    public int chops;
    public int requiredChops;

    public event Action<int, int> OnChopped;

    private void Start()
    {
        cookingInputOutput.OnCookingStart += OnCookingGameStart;
    }

    private void OnDestroy()
    {
        cookingInputOutput.OnCookingStart -= OnCookingGameStart;
    }

    private void OnCookingGameStart(FoodData foodToCut)
    {
        canCut = true;

        cutObj = CookingInputOutputScript.SpawnDisplayFoodInPosition(foodToCut, foodSpawnArea, spawnOffset, false);

        cutFoodData = foodToCut;

        chops = 0;
        requiredChops = cutFoodData.chopsRequired;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (!canCut || playerHand.currentFoodHeldObj != null)
        {
            return;
        }

        Chop();
    }

    public void Chop()
    {
        chops++;

        OnChopped?.Invoke(chops, requiredChops);

        if (chops >= requiredChops)
        {
            // DO whatever

            // It will spawn a pickupable obj depending on the recipe used
            Vector3 choppedFoodSpawnPosition = foodSpawnArea.position;
            Transform parent = foodSpawnArea;

            GameObject displayOut = CookingInputOutputScript.SpawnDisplayFoodInPosition(cookingInputOutput.currentRecipeUsed.outputFood, parent, spawnOffset, false);

            ICookStation cookStation = cookingInputOutput.GetComponent<ICookStation>();
            cookStation.CallFoodSuccessEvent(choppedFoodSpawnPosition, displayOut, foodSpawnArea);
                
            //OnCookingSuccess?.Invoke(choppedFoodSpawnPosition, displayOut, foodSpawnArea);

            Destroy(cutObj);

            chops = 0;
            canCut = false;
            cutFoodData = null;
        }
    }
}
