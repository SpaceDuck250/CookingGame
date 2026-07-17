using UnityEngine;
using System.Collections.Generic;
using Customer;
using System;
using Pair;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Scriptable Objects/CustomerData")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public List<MealData> possibleMealOrders = new List<MealData>();

    //public List<string> normalDialogueLines = new List<string>();
    
    public List<LineMoodPair> dialogueLines = new List<LineMoodPair>();

    // Special cases
    public string wrongFoodDialogueLine;
    public string burntFoodDialogueLine;


    public float waitTimeUntilAngry;
    public float waitTimeUntilReallyAngry;

    public float fastEnoughTime;

    public float tipRange;

    public enum PersonalityTrait
    { 
        OnlyOrderChickenMeal, 
        OnlyOrderRawMeal
    }

}

namespace Pair
{
    [Serializable]
    public struct LineMoodPair
    {
        public string dialogueLine;
        public CustomerMood moodType;
    }
}