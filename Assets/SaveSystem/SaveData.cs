using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveData 
{
    public float moneyAmount = 100;

    public float[] playerPos = new float[3];

    public List<FoodSaveData> foodIdList = new List<FoodSaveData>();
    public List<FoodBoxSaveData> foodBoxList = new List<FoodBoxSaveData>();
}

[Serializable]
public class FoodSaveData
{
    public FoodSaveData(int foodId, float[] pos)
    {
        this.pos[0] = pos[0];
        this.pos[1] = pos[1];
        this.pos[2] = pos[2];

        this.foodId = foodId;

    }

    public int foodId;
    public float[] pos = new float[3];
}

[Serializable]
public class FoodBoxSaveData
{
    public FoodBoxSaveData(int boxId, int amountStored)
    {
        this.boxId = boxId;
        this.amountStored = amountStored;
    }

    public int boxId;
    public int amountStored;
}