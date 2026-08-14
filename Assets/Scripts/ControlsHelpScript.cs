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

        PlayerHandScript.OnHoldSomething += HideControlsOnScreen;
    }

    private void OnDestroy()
    {
        OnShowControlsHelp -= DisplayControlsOnScreen;
        OnHideControlsHelp -= HideControlsOnScreen;

        PlayerHandScript.OnHoldSomething -= HideControlsOnScreen;



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
