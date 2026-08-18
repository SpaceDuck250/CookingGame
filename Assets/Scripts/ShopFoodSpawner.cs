using UnityEngine;
using System.Collections.Generic;
using Delivery;
using System.Collections;

public class ShopFoodSpawner : MonoBehaviour
{
    // Add limitations later
    public Queue<FoodDeliveryData> deliveryList = new Queue<FoodDeliveryData>();

    public Transform spawnPoint;

    public float waitTimer;
    public float waitTimeUntilTruckComes;

    public float timeBetweenBoxSpawns = 1f;

    public GameObject deliveryBoxPrefab;

    public GameObject deliveryTruckPrefab;
    public Transform truckStartPoint;

    private void Start()
    {
        ShopScript.OnSucessfullyBoughtFood += AddFoodToList;
        ShopScript.OnShopClose += OnShopClose;
    }

    private void OnDestroy()
    {
        ShopScript.OnSucessfullyBoughtFood -= AddFoodToList;
        ShopScript.OnShopClose -= OnShopClose;
    }


    public void OnShopClose()
    {
        ClearAndSendFoodToTruck();
    }

    public void AddFoodToList(FoodData newFood, int amount)
    {
        FoodDeliveryData newDelivery = new FoodDeliveryData { food = newFood, amount = amount};
        deliveryList.Enqueue(newDelivery);
    }

    public void ClearAndSendFoodToTruck()
    {
        if (deliveryList.Count == 0)
        {
            return;
        }

        GameObject newTruck = Instantiate(deliveryTruckPrefab, truckStartPoint.position, truckStartPoint.rotation);
        TruckDeliveryScript truckScript = newTruck.GetComponent<TruckDeliveryScript>();

        Queue<FoodDeliveryData> copiedQueue = new Queue<FoodDeliveryData>(deliveryList);
        truckScript.SetupDelivery(this, spawnPoint);

        deliveryList.Clear();
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
