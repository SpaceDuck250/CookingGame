using UnityEngine;
using Delivery;

public class TruckDeliveryScript : MonoBehaviour
{
    public Animator truckAnimator;

    public GameObject deliveryBoxPrefab;

    private FoodDeliveryData deliveryData;
    private Transform boxSpawnPoint;

    public void SetupDelivery(FoodDeliveryData data, Transform spawnPoint)
    {
        deliveryData = data;
        boxSpawnPoint = spawnPoint;
    }

    public void DropBoxes()
    {
        GameObject newDeliveryBox = Instantiate(deliveryBoxPrefab, boxSpawnPoint.position, Quaternion.identity);

        DeliveryBoxScript deliveryBoxScript = newDeliveryBox.GetComponent<DeliveryBoxScript>();
        deliveryBoxScript.SetupDeliveryData(deliveryData);
    }

    public void OnTruckLeft()
    {
        Destroy(gameObject);
    }
}