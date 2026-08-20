using UnityEngine;

// This is a special kind of task. It can increment 3 times.
public class FoodCookingTask : TutorialTask
{
    public CookingInputOutputScript cookingInputOutput;

    public bool inputFood = false;

    public FoodData correctOutputFoodNeeded;
    public FoodData correctInputFood;

    private int firstTaskId, secondTaskId, thirdTaskId;

    public bool correctFoodWasInput = false;

    private void Start()
    {
        cookingInputOutput.OnCookingStart += OnFoodInput;
        cookingInputOutput.OnCookingSuccess += OnFoodCooked;
        cookingInputOutput.OnFoodTakenOutOfCookingStation += CheckIfCorrectOutputFoodTakenOut;

        firstTaskId = taskId;
        secondTaskId = taskId + 1;
        thirdTaskId = taskId + 2;

    }
    private void OnDestroy()
    {
        cookingInputOutput.OnCookingStart -= OnFoodInput;
        cookingInputOutput.OnCookingSuccess -= OnFoodCooked;
        cookingInputOutput.OnFoodTakenOutOfCookingStation -= CheckIfCorrectOutputFoodTakenOut;
    }

    private void OnFoodInput(FoodData foodData)
    {
        if (foodData != correctInputFood)
        {
            correctFoodWasInput = false;
            return;
        }

        correctFoodWasInput = true;
        inputFood = true;

        CompleteTask();

        completed = false;
    }

    private void OnFoodCooked(Vector3 arg1, GameObject arg2, Transform arg3)
    {
        if (!correctFoodWasInput)
        {
            return;
        }

        taskId = secondTaskId;
        CompleteTask();
        completed = false;
    }


    private void CheckIfCorrectOutputFoodTakenOut()
    {
        if (!correctFoodWasInput)
        {
            return;
        }

        if (PlayerHandScript.instance.currentFoodHeld == correctOutputFoodNeeded && inputFood)
        {
            taskId = thirdTaskId;
            CompleteTask();
        }
    }
}

