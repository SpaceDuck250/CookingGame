using UnityEngine;
using Delivery;

public class DeliveryBoxScript : Interactable
{
    public FoodDeliveryData deliveryData;

    public Transform spawnPoint;

    public float upOffsetValue;

    public int tapsNeeded = 5;
    public int tapsMade = 0;

    public Animator boxAnimator;

    public void SetupDeliveryData(FoodDeliveryData deliveryData)
    {
        this.deliveryData = deliveryData;
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeldObj != null)
        {
            return;
        }

        tapsMade++;
        if (tapsMade == tapsNeeded)
        {
            SpawnAllFoodsInside();
            Destroy(gameObject);
        }

        boxAnimator.SetTrigger("Tap");

       
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
