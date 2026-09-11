using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public string gameplaySceneName = "MainGame";

    public string tutorialSceneName = "Tutorial";

    public GameObject settingsPanel;

    void Start()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnTutorialButtonPressed()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    //public void OnClearSaveButtonPressed()
    //{
    //    if (SaveLoadManager.instance != null)
    //    {
    //        SaveLoadManager.instance.ClearAllData();
    //    }
    //}

    public void OnSettingsButtonPressed()
    {
        if (settingsPanel == null)
        {
            return;
        }

        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void OnCloseSettingsButtonPressed()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void OnQuitButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}