using UnityEngine;
using Delivery;
using System.Collections.Generic;
using System.Collections;

public class TruckDeliveryScript : MonoBehaviour
{
    public Animator truckAnimator;

    public GameObject deliveryBoxPrefab;

    public Queue<FoodDeliveryData> foodDeliveryList = new Queue<FoodDeliveryData>();
    public Transform spawnPoint;

    public float timeBetweenBoxSpawns = 0.4f;

    public void SetupDelivery(ShopFoodSpawner foodSpawner, Transform spawnPoint)
    {
        while (foodSpawner.deliveryList.Count > 0)
        {
            FoodDeliveryData deliveryData = foodSpawner.deliveryList.Dequeue();

            foodDeliveryList.Enqueue(deliveryData);
        }

        this.spawnPoint = spawnPoint;
    }

    public void DropBoxes()
    {
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
        Destroy(gameObject);
    }
}