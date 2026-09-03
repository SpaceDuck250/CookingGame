using UnityEngine;

public class PlayerSaveable : MonoBehaviour, ISaveable
{

    private void Start()
    {
        SaveLoadManager.OnSaveGame += SaveSelf;
    }

    private void OnDestroy()
    {
        SaveLoadManager.OnSaveGame -= SaveSelf;

    }

    public void SaveSelf()
    {
        float[] positionArray = new float[3];
        positionArray[0] = transform.position.x;
        positionArray[1] = transform.position.y;
        positionArray[2] = transform.position.z;


        SaveLoadManager.gameData.playerPos = positionArray;

        SaveLoadManager.gameData.moneyAmount = (float)MoneyManager.playerMoneyAmount;
    }
}
