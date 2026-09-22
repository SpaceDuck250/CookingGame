using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteTutorialCanvas : MonoBehaviour
{
    public string mainMenuName;
    public void TransportToMainGame()
    {
        SceneManager.LoadScene(mainMenuName);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
