using UnityEngine;

public class ChairScript : MonoBehaviour
{
    public CustomerMovementScript heldCustomer;
    public Vector3 originalPosition;

    public float seatTime;

    public Vector3 upOffset;

    private void Start()
    {
        seatTime = 2;
        upOffset = Vector3.up * 1.2f;
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

        if (movementScript.tableTransform != null && movementScript.tableTransform.GetChild(0) == transform && movementScript.orderDone)
        {
            heldCustomer = movementScript;
            heldCustomer.sitting = true;
            //CustomerSpawnerScript.OnCustomerSeated?.Invoke(heldCustomer);
            print("go to exit sooner");
            SeatCustomer();
            Invoke("LeaveSeat", seatTime);
        }

        
    }

    private void LeaveSeat()
    {
        heldCustomer.transform.position = originalPosition;
        heldCustomer.agent.enabled = true;

        heldCustomer.OnNewDestinationChange?.Invoke(CustomerSpawnerScript.instance.exitTransform);
        heldCustomer.sitting = false;
        heldCustomer.tableTransform = null;

        heldCustomer = null;

    }

    private void SeatCustomer()
    {
        originalPosition = heldCustomer.transform.position;
        heldCustomer.agent.enabled = false;
        heldCustomer.gameObject.transform.position = transform.position + upOffset;

        heldCustomer.gameObject.GetComponent<CustomerAnimator>().Sit();
    }

}
