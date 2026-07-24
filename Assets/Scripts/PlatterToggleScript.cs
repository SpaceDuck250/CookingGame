using UnityEngine;
using TMPro;

public class PlatterToggleScript : Interactable
{
    public enum PlatterMode
    {
        Edit,
        Finished
    }

    public PlatterMode currentMode = PlatterMode.Edit;

    public GameObject clickObject;
    public PlatterScript platterScript;

    public TextMeshProUGUI toggleText;

    public PlatterLook lookScript;

    public HoldableFoodScript holdScript;

    public override void Interact(PlayerHandScript playerHand)
    {
        if (PlayerLooker.currentLookComponent != lookScript)
        {
            return;
        }

        if (currentMode == PlatterMode.Edit)
        {
            currentMode = PlatterMode.Finished;
            platterScript.MakeAllFoodPickupable(false, "Default");
            toggleText.text = "Finished";

            holdScript.canPickUp = true;
        }
        else
        {
            currentMode = PlatterMode.Edit;
            platterScript.MakeAllFoodPickupable(true, "Food");
            toggleText.text = "Edit Mode";

            holdScript.canPickUp = false;
        }
    }

}
