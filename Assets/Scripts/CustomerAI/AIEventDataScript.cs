using System.Collections.Generic;
using UnityEngine;

public class AIEventDataScript : MonoBehaviour
{
    public float servedDishWindow = 10f;

    public readonly Queue<float> servedDishTimes = new Queue<float>();

    public int foodLyingAround;

    // Resets old dish times and returns the number of dishes served within the time window
    public int DishesServedAtOnce
    {
        get
        {
            RemoveExpiredDishTimes();
            return servedDishTimes.Count;
        }
    }

    // Amount of food lying around for the Inspector event
    public int FoodLyingAround
    {
        get
        {
            return foodLyingAround;
        }
    }

    // Call this whenever the player serves one dish
    public void RecordDishServed()
    {
        servedDishTimes.Enqueue(Time.time);
        RemoveExpiredDishTimes();
    }

    // Clears the dish burst after the Fussy Customer event activates
    public void ClearServedDishData()
    {
        servedDishTimes.Clear();
    }

    // Call this when food starts lying around
    public void AddFoodLyingAround()
    {
        foodLyingAround++;
    }

    // Call this when lying food is collected, served, or destroyed
    public void RemoveFoodLyingAround()
    {
        foodLyingAround = Mathf.Max(0, foodLyingAround - 1);
    }

    // Useful when another manager already knows the exact food amount
    public void SetFoodLyingAround(int amount)
    {
        foodLyingAround = Mathf.Max(0, amount);
    }

    // Removes old dish times that are outside of the servedDishWindow
    private void RemoveExpiredDishTimes()
    {
        float oldestAllowedTime = Time.time - servedDishWindow;

        while (servedDishTimes.Count > 0 && servedDishTimes.Peek() < oldestAllowedTime)
        {
            servedDishTimes.Dequeue();
        }
    }
}
