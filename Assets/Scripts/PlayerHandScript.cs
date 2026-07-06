using UnityEngine;
<<<<<<< HEAD

public class PlayerHandScript : MonoBehaviour
{
    public FoodData currentFoodHeld;
    public GameObject currentFoodHeldObj;
=======
using System;

public class PlayerHandScript : MonoBehaviour
{
    public FoodData currentFoodHeld = null;
    public GameObject currentFoodHeldObj = null;
>>>>>>> origin/newestAlex
    public Camera cam;

    public float maxRange;
    public LayerMask foodLayer;
<<<<<<< HEAD
    public LayerMask cookingStationLayer;
=======
    public LayerMask interactableLayer;
>>>>>>> origin/newestAlex

    public Transform heldContainer;

    public float throwStrength;
    public float spinStrength;

    public Interactable currentInteractable;

    public static PlayerHandScript instance;

<<<<<<< HEAD
    private void Awake()
    {
        instance = this;

=======
    //public event Action<CookingInputOutputScript> OnFoodTakenOutOfCookingStation;

    private void Awake()
    {
        instance = this;
>>>>>>> origin/newestAlex
    }

    private void Update()
    {
        CheckForFoodInputs();
<<<<<<< HEAD
        //print(instance);
=======
>>>>>>> origin/newestAlex
    }

    public void CheckForFoodInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
<<<<<<< HEAD
            if (currentFoodHeld == null)
            {
                TryHoldingFoodObj();
=======
            if (currentFoodHeld == null && currentFoodHeldObj == null)
            {
                if (TryHoldingFoodObj())
                {
                    return;
                }
                TryInteractWithInteractable();
>>>>>>> origin/newestAlex
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

<<<<<<< HEAD
    private void TryHoldingFoodObj()
    {
        if (currentFoodHeld != null || currentFoodHeldObj != null)
        {
            return;
=======
    private bool TryHoldingFoodObj()
    {
        if (currentFoodHeld != null || currentFoodHeldObj != null)
        {
            return false;
>>>>>>> origin/newestAlex
        }

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange, foodLayer))
        {
<<<<<<< HEAD
            //SwitchFoodItem(hit.collider.gameObject.GetComponent<FoodData>());
=======
>>>>>>> origin/newestAlex
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
<<<<<<< HEAD
        }
=======

            return true;
        }

        return false;
>>>>>>> origin/newestAlex
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

<<<<<<< HEAD
=======
        if (holdableScript.cookingStationIn != null)
        {
            holdableScript.cookingStationIn.OnFoodTakenOutOfCookingStation?.Invoke();
        }

>>>>>>> origin/newestAlex
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
<<<<<<< HEAD
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange))
        {
=======
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange, interactableLayer))
        {
            print(hit.collider);

>>>>>>> origin/newestAlex
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }
<<<<<<< HEAD
=======

    public void ClearFoodFromHand()
    {
        currentFoodHeld = null;
        if (currentFoodHeldObj != null)
        {
            Destroy(currentFoodHeldObj.gameObject);
        }
    }
>>>>>>> origin/newestAlex
}
