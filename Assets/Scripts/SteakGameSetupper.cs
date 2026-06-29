using UnityEngine;

public class SteakGameSetupper : MonoBehaviour
{
    public CookingInputOutputScript cookingInputOutput;
    public SteakFlipperScript flipperScript;

    private void Start()
    {
        cookingInputOutput.OnCookingStart += OnCookingGameStart;
    }

    private void OnDestroy()
    {
        cookingInputOutput.OnCookingStart -= OnCookingGameStart;
    }

    private void OnCookingGameStart(FoodData foodCooked)
    {
        flipperScript.steakHeld = CookingInputOutputScript.SpawnDisplayFoodInPosition(foodCooked, flipperScript.flipObject.transform, flipperScript.localPositionOffset, false);

        flipperScript.SetTopAndBottom();
    }
}
