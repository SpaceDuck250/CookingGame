using UnityEngine;

// This is a special kind of task. It can increment 3 times.
public class FoodCookingTask : TutorialTask
{
    public CookingInputOutputScript cookingInputOutput;

    public bool inputFood = false;
    public bool cooked = false;

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
        if (inputFood)
        {
            return;
        }

        if (taskId != TutorialArrowsManager.currentTaskId)
        {
            return;
        }

        if (foodData != correctInputFood)
        {
            correctFoodWasInput = false;
            return;
        }

        correctFoodWasInput = true;
        inputFood = true;

        CompleteTask();
        taskId = secondTaskId;

        completed = false;
    }

    private void OnFoodCooked(Vector3 arg1, GameObject arg2, Transform arg3)
    {
        if (cooked)
        {
            return;
        }

        if (taskId != TutorialArrowsManager.currentTaskId)
        {
            return;
        }

        if (!correctFoodWasInput)
        {
            return;
        }

        cooked = true;


        CompleteTask();
        taskId = thirdTaskId;
        completed = false;
    }


    private void CheckIfCorrectOutputFoodTakenOut()
    {
        if (taskId != TutorialArrowsManager.currentTaskId)
        {
            return;
        }

        if (!correctFoodWasInput)
        {
            return;
        }

        if (PlayerHandScript.instance.currentFoodHeld == correctOutputFoodNeeded && inputFood)
        {

            CompleteTask();
        }
    }
}

