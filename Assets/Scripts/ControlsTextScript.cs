using UnityEngine;
using TMPro;
using System;

public class ControlsTextScript : MonoBehaviour
{
    public TextMeshProUGUI controlsTextComponent;

    public static Action<string> OnChangeControlsText;

    private void Start()
    {
        OnChangeControlsText += ChangeControlsText;
    }

    private void OnDestroy()
    {
        OnChangeControlsText -= ChangeControlsText;

    }

    public void ChangeControlsText(string newText)
    {
        controlsTextComponent.text = newText;
    }
}
