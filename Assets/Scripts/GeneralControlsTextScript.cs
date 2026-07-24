using UnityEngine;
using System;
using TMPro;

public class GeneralControlsTextScript : MonoBehaviour
{
    public static Action<string> OnControlsTextChanged;
    public TextMeshProUGUI controlsText;

    private void Start()
    {
        OnControlsTextChanged += ChangeControlsText;
    }

    private void OnDestroy()
    {
        OnControlsTextChanged -= ChangeControlsText;

    }

    public void ChangeControlsText(string newText)
    {
        controlsText.text = newText;
    }
}
