using UnityEngine;
using System.Collections.Generic;

public class ShopFoodSpawner : MonoBehaviour
{
    // Add limitations later
    public Queue<FoodData> foodsToSpawn = new Queue<FoodData>();

    public Transform spawnPoint;

    public float waitTimer;
    public float waitTime;

    private void Start()
    {
        ShopScript.OnSucessfullyBoughtFood += AddFoodToList;
    }

    private void OnDestroy()
    {
        ShopScript.OnSucessfullyBoughtFood -= AddFoodToList;
    }

    private void Update()
    {
        if (foodsToSpawn.Count == 0)
        {
            return;
        }

        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            waitTimer = 0;
            SpawnNewFood();
        }

    }

    public void AddFoodToList(FoodData newFood, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            foodsToSpawn.Enqueue(newFood);
        }
    }

    public void SpawnNewFood()
    {
        FoodData foodToSpawn = foodsToSpawn.Dequeue();

        Instantiate(foodToSpawn.foodModel, spawnPoint.position, Quaternion.identity, spawnPoint);
    }
}
