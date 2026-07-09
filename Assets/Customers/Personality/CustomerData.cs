using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Scriptable Objects/CustomerData")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public List<MealData> possibleMealOrders = new List<MealData>();

    public float waitTime;

    public float tipRange;

    public enum PersonalityTrait
    { 
        OnlyOrderChickenMeal, 
        OnlyOrderRawMeal
    }

}
