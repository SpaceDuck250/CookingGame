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

    public bool onLookObj = false;

    private void Start()
    {
        OnShowControlsHelp += DisplayControlsOnScreen;
        OnHideControlsHelp += HideControlsOnScreen;

        //PlayerHandScript.OnHoldSomething += ShowHoldText;

        PlayerHandScript.OnStopHoldSomething += HideControlsOnScreen;
    }

    private void OnDestroy()
    {
        OnShowControlsHelp -= DisplayControlsOnScreen;
        OnHideControlsHelp -= HideControlsOnScreen;

        //PlayerHandScript.OnHoldSomething -= ShowHoldText;

        PlayerHandScript.OnStopHoldSomething -= HideControlsOnScreen;
    }

    private void Update()
    {
        //print(PlayerLooker.currentLookComponent);


        if (onLookObj)
        {
            return;
        }


        //if (PlayerLooker.currentLookComponent == null)
        //{
        //    DisplayControlsOnScreen("");

        //}

        if (PlayerHandScript.instance.currentFoodHeldObj != null)
        {
            DisplayControlsOnScreen("Right Click (Drop)");
        }
        else if (PlayerLooker.currentLookComponent == null)
        {
            DisplayControlsOnScreen("");
        }
    }

    public void DisplayControlsOnScreen(string controlText)
    {
        onLookObj = true;

        controlsHelpTextComponent.text = controlText;
        controlsHelpObj.SetActive(true);
    }

    public void HideControlsOnScreen()
    {
        onLookObj = false;

        controlsHelpObj?.SetActive(false);
    }

    //public void ShowHoldText()
    //{
    //    controlsHelpObj?.SetActive(true);
    //    controlsHelpTextComponent.text = "Right Click (Drop)";
    //}

    public static void ShowControls(bool show, string controlText = "")
    {
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
