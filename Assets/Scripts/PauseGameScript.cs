using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameScript : MonoBehaviour
{
    public bool gamePaused = false;
    public string mainMenuName;

    public SlowTyper slowTyper;

    public GameObject pauseObj;

    private void Update()
    {
        if (slowTyper.inDialogue)
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
}
