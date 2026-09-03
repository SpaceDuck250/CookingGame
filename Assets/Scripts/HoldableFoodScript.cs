using UnityEngine;

public class HoldableFoodScript : MonoBehaviour, ISaveable
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

    // Fuck the trays
    public GameObject anotherThingToChangeLayer;


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

        doFloorChecks = CarryType ? false : true; // Checks if food

        SaveLoadManager.OnSaveGame += SaveSelf;
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

    private void OnTriggerExit(Collider other)
    {
        if (!doFloorChecks)
        {
            return;
        }

        if (other.gameObject.tag == "Floor")
        {
            FloorManager.OnFoodPickupFromFloor?.Invoke(gameObject);
            print("left floor");
        }
    }

    private void OnDestroy()
    {
        SaveLoadManager.OnSaveGame -= SaveSelf;


        if (!doFloorChecks)
        {
            return;
        }

        FloorManager.OnFoodPickupFromFloor?.Invoke(gameObject);
        //print("Left floor");
    }

    public void SaveSelf()
    {
        if (CarryType)
        {
            return;
        }
        //SaveLoadManager.gameData.foodIdInMap
        int idToSave = SaveConverter.MapItemToId<GameObject>(foodData.foodModel, SaveConverter.instance.FoodToIDMap);

        float[] posArray = new float[3];

        posArray[0] = transform.position.x;
        posArray[1] = transform.position.y;
        posArray[2] = transform.position.z;

        SaveLoadManager.gameData.foodIdList.Add(new FoodSaveData(idToSave, posArray));
    }

}
