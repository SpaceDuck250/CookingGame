using JetBrains.Annotations;
using UnityEngine;

public class ChairScript : MonoBehaviour
{
    public CustomerMovementScript heldCustomer;
    public Vector3 originalPosition;

    public float seatTime;

    public Vector3 upOffset;
    public float forwardOffsetValue;

    public GameObject tableParent;

    public Vector3 foodPlaceOffset;
    private Vector3 originalPos;

    private void Start()
    {
        seatTime = 2;
        upOffset = Vector3.up * 0.9f;
        forwardOffsetValue = 0.4f;
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

        if (movementScript.chairTransform != null && movementScript.chairTransform.GetChild(0) == transform && movementScript.orderDone)
        {
            heldCustomer = movementScript;
            heldCustomer.sitting = true;

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
        heldCustomer.chairTransform = null;

        heldCustomer.mealChecker.customerHand.localPosition = new Vector3(0, 2.141f, 1.292f);


        heldCustomer = null;
    }

    private void SeatCustomer()
    {
        originalPosition = heldCustomer.transform.position;
        heldCustomer.agent.enabled = false;

        Vector3 forwardOffset = (tableParent.transform.position - transform.position).normalized * forwardOffsetValue;

        heldCustomer.gameObject.transform.position = transform.position + upOffset + forwardOffset;

        Vector3 rotateVector = (tableParent.transform.position - transform.position).normalized;
        float rotateAngle = Mathf.Atan2(rotateVector.x, rotateVector.z) * Mathf.Rad2Deg;

        heldCustomer.gameObject.transform.rotation = Quaternion.Euler(0, rotateAngle, 0);

        heldCustomer.mealChecker.customerHand.localPosition = new Vector3(0, 0.38f, 1.292f);

        heldCustomer.gameObject.GetComponent<CustomerAnimator>().Sit();
    }

}
