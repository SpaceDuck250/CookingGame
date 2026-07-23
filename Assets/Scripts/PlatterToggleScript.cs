using UnityEngine;
using TMPro;

public class PlatterToggleScript : Interactable
{
    public enum PlatterMode
    {
        Edit,
        Finished
    }

    public PlatterMode currentMode;

    public GameObject clickObject;
    public PlatterScript platterScript;

    public TextMeshProUGUI toggleText;

    public ScreenControlPreview controlPreview;

    private void Start()
    {
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (currentMode == PlatterMode.Edit)
        {
            currentMode = PlatterMode.Finished;
            platterScript.MakeAllFoodPickupable(false, "Default");
            toggleText.text = "Finished";
        }
        else
        {
            currentMode = PlatterMode.Edit;
            platterScript.MakeAllFoodPickupable(true, "Food");
            toggleText.text = "Edit Mode";

        }
    }

}
