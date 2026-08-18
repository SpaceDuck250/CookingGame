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

    public GameObject deliveryTruckPrefab;
    public Transform truckStartPoint;

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

        GameObject newTruck = Instantiate(deliveryTruckPrefab, truckStartPoint.position, truckStartPoint.rotation);
        TruckDeliveryScript truckScript = newTruck.GetComponent<TruckDeliveryScript>();
        truckScript.SetupDelivery(boxToSpawn, spawnPoint);
    }
}

namespace Delivery
{
    public struct FoodDeliveryData
    {
        public FoodData food;
        public int amount;
    }
}
