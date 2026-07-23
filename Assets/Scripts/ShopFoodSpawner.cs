using UnityEngine;
using System.Collections.Generic;
using Delivery;

public class ShopFoodSpawner : MonoBehaviour
{
   

    // Add limitations later
    public Queue<FoodDeliveryData> deliveryList = new Queue<FoodDeliveryData>();

    public Transform spawnPoint;

    public float waitTimer;
    public float waitTime;

    public GameObject deliveryBoxPrefab;

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
        if (deliveryList.Count == 0)
        {
            return;
        }

        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            waitTimer = 0;
            PackageAndDeliver();
        }

    }

    public void AddFoodToList(FoodData newFood, int amount)
    {

        FoodDeliveryData newDelivery = new FoodDeliveryData { food = newFood, amount = amount};
        deliveryList.Enqueue(newDelivery);
    }

    public void PackageAndDeliver()
    {
        FoodDeliveryData boxToSpawn = deliveryList.Dequeue();
        GameObject newDeliveryBox = Instantiate(deliveryBoxPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);

        DeliveryBoxScript deliveryBoxScript = newDeliveryBox.GetComponent<DeliveryBoxScript>();
        deliveryBoxScript.SetupDeliveryData(boxToSpawn);
        //Instantiate(foodToSpawn.foodModel, spawnPoint.position, Quaternion.identity, spawnPoint);
    }
}

namespace Delivery
{
    public class FoodDeliveryData
    {
        public FoodData food;
        public int amount;
    }
}
