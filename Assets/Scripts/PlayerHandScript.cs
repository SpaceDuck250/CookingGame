using UnityEngine;

public class PlayerHandScript : MonoBehaviour
{
    public FoodData currentFoodHeld;
    public GameObject currentFoodHeldObj;
    public Camera cam;

    public float maxRange;
    public LayerMask foodLayer;
    public LayerMask interactableLayer;

    public Transform heldContainer;

    public float throwStrength;
    public float spinStrength;

    public Interactable currentInteractable;

    public static PlayerHandScript instance;

    private void Awake()
    {
        instance = this;

    }

    private void Update()
    {
        CheckForFoodInputs();
    }

    public void CheckForFoodInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentFoodHeld == null && currentFoodHeldObj == null)
            {
                TryHoldingFoodObj();
            }
            else
            {
                TryInteractWithInteractable();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (currentFoodHeldObj != null)
            {
                ThrowFood();
            }
        }
    }

    private void TryHoldingFoodObj()
    {
        if (currentFoodHeld != null || currentFoodHeldObj != null)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange, foodLayer))
        {
            //SwitchFoodItem(hit.collider.gameObject.GetComponent<FoodData>());
            HoldableFoodScript holdableFoodScript = hit.collider.gameObject.GetComponent<HoldableFoodScript>();
            FoodData foodData = holdableFoodScript.foodData;

            if (!holdableFoodScript.CarryType)
            {
                SwitchFoodItem(foodData, hit.collider.gameObject);
            }
            else
            {
                SwitchFoodItem(null, hit.collider.gameObject);
            }

            BringFoodToHand(holdableFoodScript);
        }
    }

    private void SwitchFoodItem(FoodData newFoodItem, GameObject newFoodObj)
    {
        currentFoodHeld = newFoodItem;
        currentFoodHeldObj = newFoodObj;
    }

    private void BringFoodToHand(HoldableFoodScript holdableScript)
    {
        if (holdableScript.CarryType)
        {
            CarryInstead(holdableScript.gameObject);
            return;
        }

        if (holdableScript.platterIn != null)
        {
            holdableScript.platterIn.OnFoodTakenOutOfPlatter?.Invoke(holdableScript.foodData);
        }

        currentFoodHeldObj = Instantiate(holdableScript.foodData.foodModel, transform.position, Quaternion.identity, heldContainer);

        //currentFoodHeldObj = holdableScript.gameObject;
        currentFoodHeldObj.transform.SetParent(heldContainer.transform, true);


        Rigidbody rb = currentFoodHeldObj.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        currentFoodHeldObj.transform.localPosition = Vector3.zero;

        holdableScript.DeleteObjectToDelete();
    }

    private void CarryInstead(GameObject objectToCarry)
    {
        currentFoodHeldObj = objectToCarry;
        currentFoodHeldObj.transform.SetParent(heldContainer.transform, true);


        Rigidbody rb = currentFoodHeldObj.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        currentFoodHeldObj.GetComponent<Collider>().isTrigger = true;

        currentFoodHeldObj.transform.localPosition = Vector3.zero;
    }

    private void ThrowFood()
    {
        if (currentFoodHeld == null && currentFoodHeldObj == null)
        {
            return;
        }

        currentFoodHeld = null;
        

        currentFoodHeldObj.transform.parent = null;

        Rigidbody rb = currentFoodHeldObj.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        currentFoodHeldObj.GetComponent<Collider>().isTrigger = false;


        Vector3 throwForce = cam.transform.forward * throwStrength;
        rb.AddForce(throwForce, ForceMode.Impulse);

        rb.AddTorque(Vector3.up * spinStrength, ForceMode.Impulse);

        currentFoodHeldObj = null;
    }

    private void TryInteractWithInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange, interactableLayer))
        {
            print(hit.collider);

            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }
}
