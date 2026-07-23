using UnityEngine;
using Delivery;

public class DeliveryBoxScript : Interactable
{
    public FoodDeliveryData deliveryData;

    public Transform spawnPoint;

    public float upOffsetValue;

    public void SetupDeliveryData(FoodDeliveryData deliveryData)
    {
        this.deliveryData = deliveryData;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        SpawnAllFoodsInside();
        Destroy(gameObject);
    }

    public void SpawnAllFoodsInside()
    {
        for (int i = 0; i < deliveryData.amount; i++)
        {
            Vector3 upOffset = i * Vector3.up * upOffsetValue;
            Instantiate(deliveryData.food.foodModel, spawnPoint.position + upOffset, Quaternion.identity);
        }
    }
}
