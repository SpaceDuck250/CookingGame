using UnityEngine;

public class LookControlsScript : MonoBehaviour, ILookable
{
    public string customControlsText;

    public bool showAltTextIfHoldingFood = false;
    public bool showAltTextIfHolding = false;

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
        //if (showAltTextIfHolding && PlayerHandScript.instance.currentFoodHeldObj != null)
        //{
        //    string altText = "Right Click (drop)";
        //    ControlsHelpScript.ShowControls(value, altText);

        //}

        ControlsHelpScript.ShowControls(value, customControlsText);
    }
}
