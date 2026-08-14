using System;
using UnityEngine;

public class PlayerHandScript : MonoBehaviour
{
    public FoodData currentFoodHeld = null;
    public GameObject currentFoodHeldObj = null;
    public Camera cam;

    public float maxRange;
    public LayerMask foodLayer;
    public LayerMask interactableLayer;

    public Transform heldContainer;

    public float throwStrength;
    public float spinStrength;

    public Interactable currentInteractable;

    public static PlayerHandScript instance;

    public static Action OnHoldSomething;
    public static Action OnStopHoldSomething;

    public PlayerMovement playerMove;
    public TurnScript playerTurn;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
                if (TryHoldingFoodObj())
                {
                    return;
                }
                TryInteractWithInteractable();
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

    private bool TryHoldingFoodObj()
    {
        if (currentFoodHeld != null || currentFoodHeldObj != null)
        {
            return false;
        }

        int layerMask = ~(1 << 10);

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange, layerMask, QueryTriggerInteraction.Collide))
        {
            HoldableFoodScript holdableFoodScript = hit.collider.gameObject.GetComponentInParent<HoldableFoodScript>();
            if (holdableFoodScript == null)
            {
                return false;
            }

            if (!holdableFoodScript.canPickUp)
            {
                return false;
            }

            if (!holdableFoodScript.CarryType)
            {
                FoodData foodData = holdableFoodScript.foodData;
                SwitchFoodItem(foodData, hit.collider.gameObject);
            }
            else
            {
                SwitchFoodItem(null, hit.collider.gameObject);
            }

            BringFoodToHand(holdableFoodScript);
            OnHoldSomething?.Invoke();
            //NpcDialogueScript.OnHideDialogue?.Invoke();

            return true;
        }

        return false;
    }

    private void SwitchFoodItem(FoodData newFoodItem, GameObject newFoodObj)
    {
        currentFoodHeld = newFoodItem;
        currentFoodHeldObj = newFoodObj;
    }

    private void BringFoodToHand(HoldableFoodScript holdableScript, bool deleteWhenPickedUp = true)
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

        if (holdableScript.cookingStationIn != null)
        {
            //holdableScript.cookingStationIn.OnFoodTakenOutOfCookingStation?.Invoke();
            ICookStation cookStation = holdableScript.cookingStationIn.GetComponent<ICookStation>();

            cookStation.CallFoodTakenOutEvent();
        }

        currentFoodHeldObj = Instantiate(holdableScript.foodData.foodModel, transform.position, Quaternion.identity, heldContainer);

        //currentFoodHeldObj = holdableScript.gameObject;
        currentFoodHeldObj.transform.SetParent(heldContainer.transform, true);

        if (!holdableScript.changeRotationOnHand)
        {
            currentFoodHeldObj.transform.localRotation = holdableScript.foodData.foodModel.transform.rotation;
        }
        else
        {
            currentFoodHeldObj.transform.localRotation = holdableScript.rotationOffset;
        }

        Rigidbody rb = currentFoodHeldObj.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        currentFoodHeldObj.transform.localPosition = Vector3.zero;
        currentFoodHeldObj.transform.localPosition += currentFoodHeldObj.GetComponent<HoldableFoodScript>().holdOffset;

        currentFoodHeldObj.GetComponent<Collider>().isTrigger = true;

        if (deleteWhenPickedUp)
        {
            holdableScript.DeleteObjectToDelete();
        }

        currentFoodHeldObj.layer = LayerMask.NameToLayer("IgnoreLayer");
    }

    private void CarryInstead(GameObject objectToCarry)
    {
        currentFoodHeldObj = objectToCarry;
        currentFoodHeldObj.layer = LayerMask.NameToLayer("IgnoreLayer");
        currentFoodHeldObj.transform.SetParent(heldContainer.transform, true);

        Rigidbody rb = currentFoodHeldObj.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        currentFoodHeldObj.GetComponent<Collider>().isTrigger = true;

        currentFoodHeldObj.transform.localPosition = Vector3.zero;
        currentFoodHeldObj.transform.localRotation = Quaternion.identity;
        currentFoodHeldObj.transform.localPosition += currentFoodHeldObj.GetComponent<HoldableFoodScript>().holdOffset;




        HoldableFoodScript holdScript = currentFoodHeldObj.GetComponent<HoldableFoodScript>();
        ScaleObject(currentFoodHeldObj, holdScript.originalScale * holdScript.pickupScaleModifier);

        //if (objectToCarry.tag == "Platter")
        //{
        //    InteractAreaScript interactArea = currentFoodHeldObj.transform.GetChild(0).GetComponent<InteractAreaScript>();
        //    interactArea.HideDisplay();
        //}
        

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


        //Vector3 throwForce = cam.transform.forward * throwStrength;
        //rb.AddForce(throwForce, ForceMode.Impulse);

        //rb.AddTorque(Vector3.up * spinStrength, ForceMode.Impulse);

        ScaleObject(currentFoodHeldObj, currentFoodHeldObj.GetComponent<HoldableFoodScript>().originalScale);

        if (currentFoodHeldObj.tag == "Platter")
        {
            //currentFoodHeldObj.transform.GetChild(0).GetComponent<InteractAreaScript>().active = true;
        }

        currentFoodHeldObj.layer = LayerMask.NameToLayer("Default");


        currentFoodHeldObj = null;

        OnStopHoldSomething?.Invoke();


    }

    private void TryInteractWithInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxRange, interactableLayer))
        {
            //print(hit.collider);

            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }

    public void ClearFoodFromHand()
    {
        currentFoodHeld = null;
        if (currentFoodHeldObj != null)
        {
            Destroy(currentFoodHeldObj.gameObject);
        }

        OnStopHoldSomething?.Invoke();
    }

    public void TransferPlatterToCustomer(Transform newParent, Quaternion newRotation)
    {
        HoldableFoodScript holdScript = currentFoodHeldObj.GetComponent<HoldableFoodScript>();

        // Summarize into a function
        holdScript.canPickUp = false;

        PlatterScript platterScript = holdScript.transform.GetChild(0).GetComponent<PlatterScript>();
        if (platterScript != null)
        {
            foreach (Transform placeArea in platterScript.placeAreasArray)
            {
                if (placeArea.childCount > 0)
                {
                    placeArea.GetChild(0).GetComponent<HoldableFoodScript>().canPickUp = false;
                }
            }
        }

        ScaleObject(currentFoodHeldObj, holdScript.originalScale * holdScript.pickupScaleModifier * 0.6f);

        currentFoodHeld = null;
        if (currentFoodHeldObj != null)
        {
            currentFoodHeldObj.transform.SetParent(newParent);
            currentFoodHeldObj.transform.localPosition = Vector3.zero;
            currentFoodHeldObj.transform.localRotation = newRotation;

            currentFoodHeldObj = null;
        }
    }

    // For food box
    public void BringFoodDirectlyToHand(FoodData food)
    {
        HoldableFoodScript holdableFoodScript = food.foodModel.gameObject.GetComponent<HoldableFoodScript>();

        SwitchFoodItem(food, null);


        BringFoodToHand(holdableFoodScript, false);
        OnHoldSomething?.Invoke();
    }

    public void ScaleObject(GameObject obj, Vector3 scaleAmount)
    {
        obj.transform.localScale = scaleAmount;
    }

    public void FreezePlayer(bool value, Transform rotateToTransform)
    {
        playerMove.canMove = !value;
        playerTurn.canTurn = !value;

        if (value == true)
        {
            playerMove.rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            playerMove.rb.constraints = RigidbodyConstraints.FreezeRotation;
        }


    }
}
