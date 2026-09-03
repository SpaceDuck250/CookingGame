using UnityEngine;

public class FoodBoxSaveable : MonoBehaviour, ISaveable
{
    public BrownFoodBox foodBox;

    private void Start()
    {
        foodBox = GetComponent<BrownFoodBox>();

        SaveLoadManager.OnSaveGame += SaveSelf;
    }

    private void OnDestroy()
    {
        SaveLoadManager.OnSaveGame -= SaveSelf;
    }

    public void SaveSelf()
    {
        int foodBoxId = SaveConverter.MapItemToId<BrownFoodBox>(foodBox, SaveConverter.instance.FoodBoxToIDMap);

        SaveLoadManager.gameData.foodBoxList.Add(new FoodBoxSaveData(foodBoxId, foodBox.foodStoredCount));
    }
}
