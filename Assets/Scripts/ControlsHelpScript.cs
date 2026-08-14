using UnityEngine;
using System;
using TMPro;

public class ControlsHelpScript : MonoBehaviour
{
    public static event Action<string> OnShowControlsHelp;
    public static event Action OnHideControlsHelp;


    public GameObject controlsHelpObj;
    public TextMeshProUGUI controlsHelpTextComponent;

    public bool hidden = false;

    private void Start()
    {
        OnShowControlsHelp += DisplayControlsOnScreen;
        OnHideControlsHelp += HideControlsOnScreen;

        PlayerHandScript.OnHoldSomething += ShowHoldText;

        PlayerHandScript.OnStopHoldSomething += HideControlsOnScreen;
    }

    private void OnDestroy()
    {
        OnShowControlsHelp -= DisplayControlsOnScreen;
        OnHideControlsHelp -= HideControlsOnScreen;

        PlayerHandScript.OnHoldSomething -= ShowHoldText;

        PlayerHandScript.OnStopHoldSomething -= HideControlsOnScreen;




    }

    public void DisplayControlsOnScreen(string controlText)
    {
        controlsHelpTextComponent.text = controlText;
        controlsHelpObj.SetActive(true);
    }

    public void HideControlsOnScreen()
    {
        controlsHelpObj?.SetActive(false);
    }

    public void ShowHoldText()
    {
        controlsHelpObj?.SetActive(true);
        controlsHelpTextComponent.text = "Left Click (Drop)";
    }

    public static void ShowControls(bool show, string controlText = "")
    {
        if (PlayerHandScript.instance.currentFoodHeldObj != null)
        {
            return;
        }

        if (show)
        {
            ControlsHelpScript.OnShowControlsHelp?.Invoke(controlText);
        }
        else
        {
            ControlsHelpScript.OnHideControlsHelp?.Invoke();
        }
    }
}
