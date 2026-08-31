using UnityEngine;
using Delivery;
using System.Collections.Generic;
using System.Collections;
using System;

public class TruckDeliveryScript : MonoBehaviour
{
    public Animator truckAnimator;

    public GameObject deliveryBoxPrefab;

    public Queue<FoodDeliveryData> foodDeliveryList = new Queue<FoodDeliveryData>();
    public Transform spawnPoint;

    public float timeBetweenBoxSpawns = 0.4f;

    public ShopFoodSpawner foodSpawner;

    public static Action OnTruckLeftScene;
    public static Action OnTruckArrived;
    public SFXBank truckBank;
   
    public void SetupDelivery(ShopFoodSpawner foodSpawner, Transform spawnPoint)
    {
        this.foodSpawner = foodSpawner;
        deliveryBoxPrefab = foodSpawner.deliveryBoxPrefab;

        while (foodSpawner.deliveryList.Count > 0)
        {
            FoodDeliveryData deliveryData = foodSpawner.deliveryList.Dequeue();

            foodDeliveryList.Enqueue(deliveryData);
        }

        this.spawnPoint = spawnPoint;
    }

    public void DropBoxes()
    {
        GeneralSoundManager.instance.PlaySoundEffect(truckBank, "truck_arrive", transform.position);
        StartCoroutine(SpawnAllBoxesWithWaitTime());
    }

    public IEnumerator SpawnAllBoxesWithWaitTime()
    {
        while (foodDeliveryList.Count > 0)
        {
            FoodDeliveryData deliveryData = foodDeliveryList.Dequeue();
            print(deliveryData + " deliver");
            SpawnFoodBox(deliveryData, spawnPoint);

            yield return new WaitForSeconds(timeBetweenBoxSpawns);
        }
    }

    public void SpawnFoodBox(FoodDeliveryData deliveryData, Transform spawnPoint)
    {
        GameObject newDeliveryBox = Instantiate(deliveryBoxPrefab, spawnPoint.position, Quaternion.identity);

        DeliveryBoxScript deliveryBoxScript = newDeliveryBox.GetComponent<DeliveryBoxScript>();
        deliveryBoxScript.SetupDeliveryData(deliveryData);
    }


    public void OnTruckLeft()
    {
        ShopFoodSpawner.truckAlreadyInScene = false;

        GeneralSoundManager.instance.PlaySoundEffect(truckBank, "truck_leave", transform.position);

        OnTruckLeftScene?.Invoke();
        Destroy(gameObject);
    }
}