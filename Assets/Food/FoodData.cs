using UnityEngine;
using Category;

[CreateAssetMenu(fileName = "FoodData", menuName = "Scriptable Objects/FoodData")]
public class FoodData : ScriptableObject
{
    public int foodID;
    public string foodName;
    public Sprite foodSprite;
    public GameObject foodModel;

    public bool usesAlternateFoodModel = false;
    public GameObject alternateFoodModel;

    public int chopsRequired;

    // Dont change this, this is a static data
    public CookAmount cookedAmount = CookAmount.Raw;

    public float costInShop;
}

namespace Category
{
    public enum CookAmount
    { 
        Raw,
        Cooked,
        Burnt
    }



}