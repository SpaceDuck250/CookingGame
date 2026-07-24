using UnityEngine;

public class PlatterLook : MonoBehaviour, ILookable
{
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
        if (!interactAreaScript.withinRange)
        {
            return;
        }

    }

    public void StopLookEffect()
    {
        
    }

}
