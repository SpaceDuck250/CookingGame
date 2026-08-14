using UnityEngine;
using UnityEngine.UI;

public class PlatterLook : MonoBehaviour, ILookable
{
    public PlatterToggleScript togglerScript;
    public InteractAreaScript interactAreaScript;

    public GameObject lockObj;
    public Image lockImage;

    public Sprite lockedSprite, unlockedSprite;

    public void DoLookEffect()
    {
        //if (interactAreaScript.withinRange)
        //{
        //    lockImage.sprite = togglerScript.currentMode == PlatterToggleScript.PlatterMode.Edit ? unlockedSprite : lockedSprite;
        //    lockObj.SetActive(true);

        //    GeneralControlsTextScript.OnControlsTextChanged?.Invoke("E to Switch Mode \n Current Mode: " + togglerScript.currentMode.ToString());
        //}
    }

    public void StopLookEffect()
    {
        //lockObj.SetActive(false);
        //GeneralControlsTextScript.OnControlsTextChanged?.Invoke("");
    }
}
