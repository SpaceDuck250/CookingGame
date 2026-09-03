using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameScript : MonoBehaviour
{
    public static bool gamePaused = false;
    public string mainMenuName;
    public string mainGameName;

    public UiScreensManager uiScreensManager;

    //public SlowTyper slowTyper;
    public static bool uiAlreadyOverlayed;

    public GameObject pauseObj;

    private void Start()
    {
        mainGameName = "MainGameFIXED";
    }

    private void Update()
    {
        if (uiAlreadyOverlayed)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        ShowMouse(true);

        pauseObj.SetActive(true);

        Time.timeScale = 0;
        gamePaused = true;
    }

    public void UnPauseGame()
    {
        ShowMouse(false);

        pauseObj.SetActive(false);

        Time.timeScale = 1;
        gamePaused = false;
    }

    public void GoToMainMenu()
    {
        TrySaveScene();

        UnPauseGame();

        ShowMouse(true);
        SceneManager.LoadScene(mainMenuName);

    }

    public void QuitGame()
    {
        Debug.Log("Left game");
        Application.Quit();
    }

    public static void ShowMouse(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void TrySaveScene()
    {
        print(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == mainGameName)
        {
            SaveLoadManager.instance.BeginSavingAllData();
        }
    }

    public void ClearSave()
    {
        SaveLoadManager.instance.ClearAllData();
        GoToMainMenu();
    }
}
