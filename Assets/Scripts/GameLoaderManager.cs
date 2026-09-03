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
        LoadAllFoods();
        LoadAllFoodBox();
    }

    public void LoadPlayer()
    {
        moneyManager.SetMoney((decimal)SaveLoadManager.gameData.moneyAmount);

        player.transform.position = new Vector3(SaveLoadManager.gameData.playerPos[0], SaveLoadManager.gameData.playerPos[1], SaveLoadManager.gameData.playerPos[2]);
    }

    public void LoadAllFoods()
    {
        if (SaveLoadManager.gameData.foodIdList.Count == 0)
        {
            return;
        }

        foreach (FoodSaveData foodSave in SaveLoadManager.gameData.foodIdList)
        {
            GameObject actualFoodObj = SaveConverter.MapIDToItem<GameObject>(foodSave.foodId, SaveConverter.instance.FoodToIDMap);
            if (actualFoodObj == null)
            {
                return;
            }

            GameObject spawnedFoodObj = Instantiate(actualFoodObj, null);

            spawnedFoodObj.transform.position = new Vector3(foodSave.pos[0], foodSave.pos[1], foodSave.pos[2]);
        }
    }

    public void LoadAllFoodBox()
    {
        if (SaveLoadManager.gameData.foodBoxList.Count == 0)
        {
            return;
        }

        foreach (FoodBoxSaveData foodBoxData in SaveLoadManager.gameData.foodBoxList)
        {
            BrownFoodBox foodBox = SaveConverter.MapIDToItem<BrownFoodBox>(foodBoxData.boxId, SaveConverter.instance.FoodBoxToIDMap);

            if (foodBox == null)
            {
                return;
            }

            foodBox.foodStoredCount = foodBoxData.amountStored;

            foodBox.OnFoodAmountChangedInBox?.Invoke(foodBox.foodStoredCount, foodBox.maxFoodCount);
        }
    }
}
