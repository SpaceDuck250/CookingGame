using UnityEngine;

public class ServeCustomerTask : TutorialTask
{
    public MealChecker mealChecker;

    private void Start()
    {
        mealChecker.OnMealOrderFulfilled += OnCorrectMealServed;
    }

    private void OnDestroy()
    {
        mealChecker.OnMealOrderFulfilled += OnCorrectMealServed;
    }

    private void OnCorrectMealServed()
    {
        CompleteTask();
    }
}
