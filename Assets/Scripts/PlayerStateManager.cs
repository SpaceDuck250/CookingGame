using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static bool paused = false;

    public static void PauseGame()
    {
        Time.timeScale = 0f;
        paused = true;
    }

    public static void UnPauseGame()
    {
        Time.timeScale = 1f;
        paused = false;
    }
}
