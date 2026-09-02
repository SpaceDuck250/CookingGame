using System.Collections.Generic;

[System.Serializable]
public class SaveData 
{
    public float moneyAmount;

    public float[] playerPos = new float[3];

    public List<int> foodIdInMap = new List<int>();
}
