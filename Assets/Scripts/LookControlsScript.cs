using UnityEngine;

public class LookControlsScript : MonoBehaviour, ILookable
{
    public string customControlsText;

    public bool doAltTextIfHoldingFood = false;
    public string altText;

    public GameObject self;

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
        // FUCK PLATTERS
        if (PlayerHandScript.instance.currentFoodHeldObj != null && PlayerHandScript.instance.currentFoodHeldObj.tag == "Platter")
        {
            ControlsHelpScript.ShowControls(value, "Right Click (Drop)");

            return;
        }

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
