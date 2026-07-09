using UnityEngine;

public class ExitScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            //CustomerMovementScript movementScript = other.gameObject.GetComponent<CustomerMovementScript>();
            CustomerStateMachine customer = other.GetComponent<CustomerStateMachine>();

            CustomerSpawnerScript.OnCustomerExit?.Invoke(customer);
            Destroy(other.gameObject);
        }
    }
}
