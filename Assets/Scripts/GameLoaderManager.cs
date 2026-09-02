using UnityEngine;

public class GameLoaderManager : MonoBehaviour
{
    public GameObject player;
    public MoneyManager moneyManager;

    private void Start()
    {
        SaveLoadManager.OnLoadSave += LoadEverythingToGame;
    }

    private void OnDestroy()
    {
        SaveLoadManager.OnLoadSave -= LoadEverythingToGame;

    }

    private void LoadEverythingToGame()
    {
        LoadPlayer();
    }

    public void LoadPlayer()
    {
        moneyManager.SetMoney((decimal)SaveLoadManager.gameData.moneyAmount);

        player.transform.position = new Vector3(SaveLoadManager.gameData.playerPos[0], SaveLoadManager.gameData.playerPos[1], SaveLoadManager.gameData.playerPos[2]);
    }
}
