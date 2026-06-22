using UnityEngine;

public class PlayerHandScript : MonoBehaviour
{
    public FoodData currentFoodHeld;
    public GameObject currentFoodHeldObj;
    public Camera cam;

    public float maxRange;
    public LayerMask foodLayer;
    public LayerMask cookingStationLayer;

    public Transform heldContainer;

    public float throwStrength;
    public float spinStrength;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentFoodHeld == null)
            {
                TryHoldingFoodObj();
            }
            else
            {
                print("l");
                TryToInteractWithCookingStation();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (currentFoodHeldObj != null)
            {
                ThrowFood();
            }
            else
            {
                TryToTakeFoodFromStation();
            }
        }
    }

    private void TryHoldingFoodObj()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange, foodLayer))
        {
            //SwitchFoodItem(hit.collider.gameObject.GetComponent<FoodData>());
            HoldableFoodScript holdableFoodScript = hit.collider.gameObject.GetComponent<HoldableFoodScript>();
            FoodData foodData = holdableFoodScript.foodData;

            SwitchFoodItem(foodData, hit.collider.gameObject);
            BringFoodToHand(hit.collider.gameObject.transform);
        }
    }

    private void SwitchFoodItem(FoodData newFoodItem, GameObject newFoodObj)
    {
        currentFoodHeld = newFoodItem;
        currentFoodHeldObj = newFoodObj;
    }

    private void BringFoodToHand(Transform foodTransform)
    {
        Rigidbody rb = currentFoodHeldObj.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        

        foodTransform.parent = heldContainer;
        foodTransform.localPosition = Vector3.zero;


    }

    private void ThrowFood()
    {
        if (currentFoodHeld == null)
        {
            return;
        }

        currentFoodHeld = null;
        

        currentFoodHeldObj.transform.parent = null;

        Rigidbody rb = currentFoodHeldObj.GetComponent<Rigidbody>();
        rb.isKinematic = false;



        Vector3 throwForce = cam.transform.forward * throwStrength;
        rb.AddForce(throwForce, ForceMode.Impulse);

        rb.AddTorque(Vector3.up * spinStrength, ForceMode.Impulse);

        currentFoodHeldObj = null;
    }

    private void TryToInteractWithCookingStation()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange))
        {
            if (hit.collider.gameObject.tag != "CookingStation")
            {
                return;
            }

            print(hit.collider.gameObject);

            CookingInputOutputScript inputOutputScript = hit.collider.gameObject.GetComponent<CookingInputOutputScript>();
            inputOutputScript.TryPutFood(currentFoodHeld, this);
        }
    }

    private void TryToTakeFoodFromStation()
    {

    }
}
