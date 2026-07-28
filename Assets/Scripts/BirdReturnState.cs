using Delivery;
using UnityEngine;

public class BirdReturnState : BirdState
{
    public Transform foodGivingPoint;

    public int maxFoodGive;

    public GameObject deliveryBoxPrefab;

    public GameObject spawnedBox;

    public override void SetupState(BirdAIManager manager, BirdMovementScript movementScript)
    {
        stateManager = manager;
        birdMovementScript = movementScript;

        SpawnFoodBox();
    }

    public override void DoAction()
    {
        birdMovementScript.FlyToPoint(foodGivingPoint);

        if (birdMovementScript.CheckIfCloseEnough())
        {
            spawnedBox.GetComponent<Rigidbody>().useGravity = true;
            spawnedBox.GetComponent<HoldableFoodScript>().canPickUp = true;


            spawnedBox.transform.parent = null;

            spawnedBox = null;

            stateManager.TransitionToNewState(transitionStates[0]);
        }

    }

    public void SpawnFoodBox()
    {
        Destroy(stateManager.foodSpawnParent.GetChild(0).gameObject);

        FoodDeliveryData randomDeliveryData = new FoodDeliveryData { food = stateManager.searchFood, amount = Random.Range(1, maxFoodGive) };

        spawnedBox = Instantiate(deliveryBoxPrefab, stateManager.foodSpawnParent.position, Quaternion.identity, stateManager.foodSpawnParent);
        DeliveryBoxScript deliveryBoxScript = spawnedBox.GetComponent<DeliveryBoxScript>();
        
        deliveryBoxScript.SetupDeliveryData(randomDeliveryData);

        spawnedBox.GetComponent<Rigidbody>().useGravity = false;
        spawnedBox.GetComponent<HoldableFoodScript>().canPickUp = false;

        stateManager.searchFood = null;
    }


}
