using UnityEngine;

public class PlatterLook : MonoBehaviour, ILookable
{
    public GameObject controlsTextObj;
    public InteractAreaScript interactAreaScript;

    private void Start()
    {
        interactAreaScript.OnPlayerExitRange += StopLookEffect;
    }

    private void OnDestroy()
    {
        interactAreaScript.OnPlayerExitRange -= StopLookEffect;

    }

    public void DoLookEffect()
    {
        //print("platter");
        //if (!interactAreaScript.withinRange)
        //{
        //    return;
        //}
        controlsTextObj.SetActive(true);
    }

    public void StopLookEffect()
    {
        controlsTextObj.SetActive(false);
    }

}
