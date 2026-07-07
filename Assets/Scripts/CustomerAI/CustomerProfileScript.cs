using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Customer Profile")]
public class CustomerProfileScript : ScriptableObject
{
    // Name of unique customer profile
    public string customerName;

    // Meal preferences for this customer profile, can be multiple meals
    public MealData[] favoriteMeal;
    public int mealRequired = 1;

    // Time the customer will wait before leaving
    public float maxWaitTime = 30f;

    // Payment amounts
    public int fullPayment = 20;
    public int wrongOrderPayment = 8;
    public int timeoutPayment = 0;

    // Personallity traits
    public bool onlyOrdersChickenMeals;
}
