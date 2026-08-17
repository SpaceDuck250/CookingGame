using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("Scene Names")]
    [Tooltip("Name of the scene to load when Play is pressed (must be added to Build Settings)")]
    public string gameplaySceneName = "MainGame";
    [Tooltip("Name of the scene to load when Tutorial is pressed (must be added to Build Settings)")]
    public string tutorialSceneName = "Tutorial";

    [Header("Settings Panel")]
    [Tooltip("Panel/overlay GameObject that gets shown/hidden, no scene change")]
    public GameObject settingsPanel;

    void Start()
    {
        // Make sure the settings panel starts closed
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // Hook this up to the Play button's OnClick
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Hook this up to the Tutorial button's OnClick
    public void OnTutorialButtonPressed()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    // Hook this up to the Settings button's OnClick
    public void OnSettingsButtonPressed()
    {
        if (settingsPanel == null)
        {
            Debug.LogWarning("MainMenuScript: settingsPanel is not assigned in the Inspector.");
            return;
        }

        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    // Hook this up to a Close/Back button inside the settings panel itself
    public void OnCloseSettingsButtonPressed()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // Hook this up to the Quit button's OnClick
    public void OnQuitButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}