using UnityEngine;

public class FoodCookingTask : TutorialTask
{
    public CookingInputOutputScript cookingInputOutput;

    public bool inputFood = false;

    public FoodData correctOutputFoodNeeded;

    private void Start()
    {
        cookingInputOutput.OnCookingStart += OnFoodInput;
        cookingInputOutput.OnFoodTakenOutOfCookingStation += CheckIfCorrectOutputFoodTakenOut;

    }

    private void OnDestroy()
    {
        cookingInputOutput.OnCookingStart -= OnFoodInput;
        cookingInputOutput.OnFoodTakenOutOfCookingStation -= CheckIfCorrectOutputFoodTakenOut;
    }

    private void OnFoodInput(FoodData foodData)
    {
        inputFood = true;

        CompleteTask();

        completed = false;
    }

    private void CheckIfCorrectOutputFoodTakenOut()
    {
        if (PlayerHandScript.instance.currentFoodHeld == correctOutputFoodNeeded && inputFood)
        {
            taskId++;
            CompleteTask();
        }
    }
}

