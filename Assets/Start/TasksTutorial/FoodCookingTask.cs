using UnityEngine;

// This is a special kind of task. It can increment 3 times.
public class FoodCookingTask : TutorialTask
{
    public CookingInputOutputScript cookingInputOutput;

    public bool inputFood = false;

    public FoodData correctOutputFoodNeeded;

    private void Start()
    {
        cookingInputOutput.OnCookingStart += OnFoodInput;
        cookingInputOutput.OnCookingSuccess += OnFoodCooked;
        cookingInputOutput.OnFoodTakenOutOfCookingStation += CheckIfCorrectOutputFoodTakenOut;

    }
    private void OnDestroy()
    {
        cookingInputOutput.OnCookingStart -= OnFoodInput;
        cookingInputOutput.OnCookingSuccess -= OnFoodCooked;
        cookingInputOutput.OnFoodTakenOutOfCookingStation -= CheckIfCorrectOutputFoodTakenOut;
    }

    private void OnFoodInput(FoodData foodData)
    {
        inputFood = true;

        CompleteTask();

        completed = false;
    }

    private void OnFoodCooked(Vector3 arg1, GameObject arg2, Transform arg3)
    {
        taskId++;
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

