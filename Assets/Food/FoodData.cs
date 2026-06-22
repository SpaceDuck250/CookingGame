using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Scriptable Objects/FoodData")]
public class FoodData : ScriptableObject
{
    public int foodID;
    public string foodName;
    public Sprite foodSprite;
    public GameObject foodModel;

    public FoodData cookedVersion;
}
