using UnityEngine;

public class ShowOnlyIfFoodHas : MonoBehaviour
{
    public CookingInputOutputScript inputOutputScript;
    public ClickPreviewScript clickPreviewScript;
    

    private void Start()
    {
        inputOutputScript.OnCookingStart += ShowControl;
        inputOutputScript.OnFoodTakenOutOfCookingStation += HideControl;

        clickPreviewScript.canShow = false;
    }


    private void OnDestroy()
    {
        inputOutputScript.OnCookingStart -= ShowControl;
        inputOutputScript.OnFoodTakenOutOfCookingStation -= HideControl;

    }

    public void ShowControl(FoodData inputFood)
    {
        clickPreviewScript.canShow = true;
        clickPreviewScript.OnPlayerEnterRange(PlayerHandScript.instance.gameObject);
    }

    private void HideControl()
    {
        clickPreviewScript.canShow = false;
        clickPreviewScript.OnPlayerExitRange();
    }
}
