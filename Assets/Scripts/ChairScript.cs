using UnityEngine;

public class ChairScript : MonoBehaviour
{
    public CustomerMovementScript heldCustomer;

    public float seatTime;

    private void Start()
    {
        seatTime = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.gameObject.tag != "Customer" || heldCustomer != null)
        {
            return;
        }

        print("hit");

        CustomerMovementScript movementScript = other.gameObject.GetComponent<CustomerMovementScript>();

        if (movementScript.tableTransform.GetChild(0) == transform && movementScript.orderDone)
        {
            heldCustomer = movementScript;
            //CustomerSpawnerScript.OnCustomerSeated?.Invoke(heldCustomer);
            print("go to exit sooner");

            Invoke("LeaveSeat", seatTime);
        }

        
    }

    private void LeaveSeat()
    {
        heldCustomer.OnNewDestinationChange?.Invoke(CustomerSpawnerScript.instance.exitTransform);
        heldCustomer = null;
    }
}
