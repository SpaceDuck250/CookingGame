using UnityEngine;

public class HoldableFoodScript : MonoBehaviour
{
    public FoodData foodData;
    public GameObject objectToDelete;

    public PlatterScript platterIn;
    public bool CarryType = false;

    public GameObject cookingStationIn;

    public Vector3 originalScale;
    public bool changeScaleOnHand = false;
    public bool changeRotationOnHand = false;
    public float pickupScaleModifier;

    public Vector3 holdOffset = Vector3.zero;
    public Quaternion rotationOffset;

    public bool canPickUp = true;

    public float platterScaleModifier = 1;
    public bool changeScaleOnPlatter = false;

    public bool doFloorChecks = true;


    public void Start()
    {
        if (objectToDelete == null)
        {
            objectToDelete = gameObject;
        }

        originalScale = transform.localScale;
        pickupScaleModifier = !changeScaleOnHand ? 1 : pickupScaleModifier;
        platterScaleModifier = !changeScaleOnPlatter ? 1 : platterScaleModifier;
        rotationOffset = !changeRotationOnHand ? Quaternion.identity : rotationOffset;
    }
    
    public void DeleteObjectToDelete()
    {
        if (objectToDelete != gameObject)
        {
            Destroy(objectToDelete);
            Destroy(gameObject);
        }
        else
        {
            Destroy(objectToDelete);
        }
    }

    // For floor hitting

    private void OnTriggerEnter(Collider other)
    {
        if (!doFloorChecks)
        {
            return;
        }

        if (other.gameObject.tag == "Floor")
        {
            other.gameObject.GetComponent<FloorManager>().OnFoodHitThisFloor?.Invoke(gameObject);
            print("Hit floor");
        }
    }

    private void OnDestroy()
    {
        if (!doFloorChecks)
        {
            return;
        }

        FloorManager.OnFoodPickupFromFloor?.Invoke(gameObject);
        print("Left floor");
    }

}
