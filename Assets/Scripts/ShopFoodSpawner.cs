using UnityEngine;
using System.Collections.Generic;
using Delivery;
using System.Collections;

public class ShopFoodSpawner : MonoBehaviour
{
    // Add limitations later
    public Queue<FoodDeliveryData> deliveryList = new Queue<FoodDeliveryData>();

    public Queue<TruckDeliveryScript> deliveryTruckQueue = new Queue<TruckDeliveryScript>();

    public Transform spawnPoint;

    public float waitTimer;
    public float waitTimeUntilTruckComes;

    public float timeBetweenBoxSpawns = 1f;

    public GameObject deliveryBoxPrefab;

    public GameObject deliveryTruckPrefab;
    public Transform truckStartPoint;

    public static bool truckAlreadyInScene = false;

    private void Start()
    {
        ShopScript.OnSucessfullyBoughtFood += AddFoodToList;
        ShopScript.OnShopClose += OnShopClose;

        TruckDeliveryScript.OnTruckLeftScene += SendOutNewTruck;
    }

    private void OnDestroy()
    {
        ShopScript.OnSucessfullyBoughtFood -= AddFoodToList;
        ShopScript.OnShopClose -= OnShopClose;

        TruckDeliveryScript.OnTruckLeftScene -= SendOutNewTruck;

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

        //GameObject newTruck = Instantiate(deliveryTruckPrefab, truckStartPoint.position, truckStartPoint.rotation);
        //TruckDeliveryScript truckScript = newTruck.GetComponent<TruckDeliveryScript>();

        //Queue<FoodDeliveryData> copiedQueue = new Queue<FoodDeliveryData>(deliveryList);
        //truckScript.SetupDelivery(this, spawnPoint);
        AddNewTruckToQueue();

        deliveryList.Clear();
    }

    public void AddNewTruckToQueue()
    {
        GameObject newTruck = Instantiate(deliveryTruckPrefab, truckStartPoint.position, truckStartPoint.rotation);
        TruckDeliveryScript truckScript = newTruck.GetComponent<TruckDeliveryScript>();

        Queue<FoodDeliveryData> copiedQueue = new Queue<FoodDeliveryData>(deliveryList);
        truckScript.SetupDelivery(this, spawnPoint);

        newTruck.SetActive(false);

        deliveryTruckQueue.Enqueue(truckScript);

        TrySpawnFirstTruck();
    }

    public void SendOutNewTruck()
    {
        if (deliveryTruckQueue.Count == 0 || truckAlreadyInScene)
        {
            return;
        }

        TruckDeliveryScript truck = deliveryTruckQueue.Dequeue();
        truck.gameObject.SetActive(true);

        truckAlreadyInScene = true;
    }

    public void TrySpawnFirstTruck()
    {
        if (truckAlreadyInScene)
        {
            return;
        }

        SendOutNewTruck();
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
