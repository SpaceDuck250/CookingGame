using UnityEngine;

public class HoldableFoodScript : MonoBehaviour
{
    public FoodData foodData;
    public GameObject objectToDelete;

    public void Start()
    {
        if (objectToDelete == null)
        {
            objectToDelete = gameObject;
        }
    }
}
