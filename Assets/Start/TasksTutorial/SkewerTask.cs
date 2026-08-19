using UnityEngine;

public class SkewerTask : TutorialTask
{
    public CookingInputOutputScript cookingInputOutput;

    private void Start()
    {
        cookingInputOutput.OnCookingStart += OnFoodInput;
    }

    private void OnFoodInput(FoodData foodData)
    {
        CompleteTask();
    }
}
