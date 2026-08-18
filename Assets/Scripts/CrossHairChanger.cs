using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrossHairChanger : MonoBehaviour
{
    public Sprite grabSymbol;

    public Sprite tryGrabSymbol;

    public Image crosshairImage;
    public RectTransform crossHairRect;

    public float originalScaleAmount = 0.12f;
    public float scaleAmount;

    public bool grabShown = false;

    private void Start()
    {
        ControlsHelpScript.OnShowControlsHelp += ShowTryGrabCrossHair;
        ControlsHelpScript.OnHideControlsHelp += RevertBackToNormalCrossHair;

        PlayerHandScript.OnHoldSomething += ShowGrabCrossHair;
        PlayerHandScript.OnStopHoldSomething += RevertBackToNormalCrossHair;
    }

    private void OnDestroy()
    {
        ControlsHelpScript.OnShowControlsHelp -= ShowTryGrabCrossHair;
        ControlsHelpScript.OnHideControlsHelp -= RevertBackToNormalCrossHair;

        PlayerHandScript.OnHoldSomething -= ShowGrabCrossHair;
        PlayerHandScript.OnStopHoldSomething -= RevertBackToNormalCrossHair;



    }

    public void ShowTryGrabCrossHair(string controlsText)
    {
        if (PlayerHandScript.instance.currentFoodHeldObj != null)
        {
            ShowGrabCrossHair();
            return;
        }

        if (controlsText != "Right Click")
        {
            return;
        }

        crosshairImage.sprite = tryGrabSymbol;

        crossHairRect.transform.localScale = Vector3.one * scaleAmount;
    }

    public void TryHideCrossHair(bool hideAll)
    {
        if (!hideAll)
        {
            return;
        }

        RevertBackToNormalCrossHair();
    }

    public void RevertBackToNormalCrossHair()
    {

        crosshairImage.sprite = null;

        crossHairRect.transform.localScale = Vector3.one * originalScaleAmount;
    }

    public void ShowGrabCrossHair()
    {
        crosshairImage.sprite = grabSymbol;

        crossHairRect.transform.localScale = Vector3.one * scaleAmount;

    }

}
