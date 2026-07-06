using UnityEngine;

public class SteakGameSetupper : MonoBehaviour
{
    public CookingInputOutputScript cookingInputOutput;
    public SteakFlipperScript flipperScript;

    public bool alreadyCooking = false;

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
        if (flipperScript.steakHeld != null)
        {
            return;
        }


        flipperScript.steakHeld = CookingInputOutputScript.SpawnDisplayFoodInPosition(foodCooked, flipperScript.flipObject.transform, flipperScript.localPositionOffset, false);

        flipperScript.SetTopAndBottom();
    }


}
