using UnityEngine;

public class HoldableFoodScript : MonoBehaviour
{
    public FoodData foodData;
    public GameObject objectToDelete;

    public PlatterScript platterIn;
    public bool CarryType = false;

<<<<<<< HEAD
=======
    public CookingInputOutputScript cookingStationIn;

>>>>>>> origin/newestAlex
    public Vector3 holdOffset;

    public void Start()
    {
        if (objectToDelete == null)
        {
            objectToDelete = gameObject;
        }
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
