using UnityEngine;

public class LookControlsScript : MonoBehaviour, ILookable
{
    public string customControlsText;

    public bool doAltTextIfHoldingFood = false;
    public string altText;

    public void DoLookEffect()
    {
        ShowControls(true);
    }

    public void StopLookEffect()
    {
        ShowControls(false);

    }


    public void ShowControls(bool value)
    {
        if (doAltTextIfHoldingFood && PlayerHandScript.instance.currentFoodHeld != null)
        {
            ControlsHelpScript.ShowControls(value, altText);

            return;
        }
        else if (PlayerHandScript.instance.currentFoodHeld != null)
        {

            return;
        }

        ControlsHelpScript.ShowControls(value, customControlsText);
    }
}
