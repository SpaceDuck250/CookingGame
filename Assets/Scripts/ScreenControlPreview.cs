using UnityEngine;

public class ScreenControlPreview : MonoBehaviour
{
    public InteractAreaScript interactArea;

    public string controlsText;

    public static PlatterToggleScript theOneInUse;
    public PlatterToggleScript ownPlatterToggleScript;

    private void Awake()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        interactArea.OnPlayerEnterRange -= OnPlayerEnterRange;
        interactArea.OnPlayerExitRange -= OnPlayerExitRange;
    }

    public void Subscribe()
    {
        interactArea.OnPlayerEnterRange += OnPlayerEnterRange;
        interactArea.OnPlayerExitRange += OnPlayerExitRange;
    }

    public void OnPlayerEnterRange(GameObject player)
    {
        ControlsTextScript.OnChangeControlsText?.Invoke(controlsText);

    }
    private void OnPlayerExitRange()
    {
        ControlsTextScript.OnChangeControlsText?.Invoke(string.Empty);
    }

}
