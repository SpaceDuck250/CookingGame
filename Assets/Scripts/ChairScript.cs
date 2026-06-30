using UnityEngine;

public class ChairScript : MonoBehaviour
{
    public CustomerMovementScript heldCustomer;

    public float seatTime;

    private void OnCollisionEnter(Collision other)
    {

        print(other);
        if (other.gameObject.tag != "Customer" || heldCustomer != null)
        {
            return;
        }

        print("hit");

        CustomerMovementScript movementScript = other.gameObject.GetComponent<CustomerMovementScript>();

        if (movementScript.tableTransform != transform)
        {
            return;
        }

        heldCustomer = movementScript;
        //CustomerSpawnerScript.OnCustomerSeated?.Invoke(heldCustomer);

        Invoke("LeaveSeat", seatTime);
    }

    private void LeaveSeat()
    {
        heldCustomer.OnNewDestinationChange?.Invoke(CustomerSpawnerScript.instance.exitTransform);
        heldCustomer = null;
    }
}
