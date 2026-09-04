using UnityEngine;

public class FoodOnTrayTask : TutorialTask
{
    public PlatterScript platterScript;

    public FoodData correctFoodData;

    public GameObject laptop;
    //public HoldableFoodScript holdableFoodScript;

    private void Start()
    {
        platterScript.OnFoodPlacedOnPlatter += OnFoodPlaced;
    }

    private void OnDestroy()
    {
        platterScript.OnFoodPlacedOnPlatter += OnFoodPlaced;
    }

    public void OnFoodPlaced(FoodData foodData)
    {
        if (foodData != correctFoodData)
        {
            return;
        }

        //holdableFoodScript.enabled = true;
        CompleteTask();
        laptop.layer = LayerMask.NameToLayer("Clickable");
    }

}
