using UnityEngine;

public class MainMenuSetup : MonoBehaviour
{
    private void Start()
    {
        PauseGameScript.ShowMouse(true);
        Screen.SetResolution(1920, 1080, true);
    }
}
