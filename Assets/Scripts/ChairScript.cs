using UnityEngine;
using Customer;

public class ChairScript : MonoBehaviour
{
    public CustomerStateMachine heldCustomer;
    public Vector3 originalPosition;

    public float seatTime;

    public Vector3 upOffset;

    public GameObject tableParent;

    public Vector3 foodPlaceOffset;
    private Vector3 originalPos;

    private void Start()
    {
        seatTime = 6;
        upOffset = Vector3.up * 0.15f;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.gameObject.tag != "Customer" || heldCustomer != null)
        {
            return;
        }

        print("hit");

        //CustomerMovementScript movementScript = other.gameObject.GetComponent<CustomerMovementScript>();
        CustomerStateMachine customer = other.gameObject.GetComponent<CustomerStateMachine>();

        if (customer.seatPoint != null && customer.seatPoint.GetChild(0) == transform && customer.orderDone)
        {
            heldCustomer = customer;
            heldCustomer.sitting = true;

            SeatCustomer();
            Invoke("LeaveSeat", seatTime);
        } 
    }

    private void LeaveSeat()
    {
        heldCustomer.transform.position = originalPosition;
        heldCustomer.agent.enabled = true;

        // Be more nuanced later
        //heldCustomer.movementScript.OnNewDestinationChange?.Invoke(CustomerSpawnerScript.instance.platterPoint);
        heldCustomer.OnCustomerChangeState?.Invoke(CustomerState.ReturningTray);
        heldCustomer.sitting = false;
        heldCustomer.seatPoint = null;

        //heldCustomer.mealChecker.customerHand.localPosition = new Vector3(0, 2.141f, 1.292f);

        //Vector3 originalHeldPosition = new Vector3(0, 2.141f, 1.292f);
        Vector3 originalHeldPosition = heldCustomer.normalTrayLocalPosition;
        SetPlatterPosition(heldCustomer.mealChecker.customerHand, originalHeldPosition);

        PlatterScript platter = heldCustomer.mealChecker.platterHeld;
        ClearTray(platter);

        heldCustomer = null;
    }

    private void SeatCustomer()
    {
        originalPosition = heldCustomer.transform.position;
        heldCustomer.agent.enabled = false;

        Vector3 forwardOffset = (tableParent.transform.position - transform.position).normalized * heldCustomer.forwardOffset;

        heldCustomer.gameObject.transform.position = transform.position + forwardOffset;
        heldCustomer.gameObject.transform.position = new Vector3(heldCustomer.transform.position.x, heldCustomer.upOffsetChair, heldCustomer.transform.position.z);

        Vector3 rotateVector = (tableParent.transform.position - transform.position).normalized;
        float rotateAngle = Mathf.Atan2(rotateVector.x, rotateVector.z) * Mathf.Rad2Deg;

        heldCustomer.gameObject.transform.rotation = Quaternion.Euler(0, rotateAngle, 0);

        //Vector3 tablePosition = new Vector3(0, 1.83f, 1.292f);
        Vector3 tablePosition = heldCustomer.seatedTrayLocalPosition;
        SetPlatterPosition(heldCustomer.mealChecker.customerHand, tablePosition);

        heldCustomer.OnCustomerChangeState?.Invoke(CustomerState.Seated);
    }

    public void SetPlatterPosition(Transform platter, Vector3 newLocalPosition)
    {
        platter.localPosition = newLocalPosition;
    }

    public void ClearTray(PlatterScript platterScript)
    {
        platterScript.ClearAllInPlatter();
    }

}
