using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteTutorialCanvas : MonoBehaviour
{
    public string mainGameName;
    public void TransportToMainGame()
    {
        SceneManager.LoadScene(mainGameName);
    }
}
