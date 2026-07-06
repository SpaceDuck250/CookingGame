using UnityEngine;

public class HoldableFoodScript : MonoBehaviour
{
    public FoodData foodData;
    public GameObject objectToDelete;

    public PlatterScript platterIn;
    public CuttingFoodScript cuttingIn;
    public bool CarryType = false;

    public CookingInputOutputScript cookingStationIn;

    public Vector3 originalScale;
    public bool changeScaleOnHand = false;
    public float pickupScaleModifier;

    public Vector3 holdOffset;

    public void Start()
    {
        if (objectToDelete == null)
        {
            objectToDelete = gameObject;
        }

        originalScale = transform.localScale;
        pickupScaleModifier = !changeScaleOnHand ? 1 : pickupScaleModifier;

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

}
