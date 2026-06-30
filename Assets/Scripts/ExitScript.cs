using UnityEngine;

public class ExitScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            CustomerMovementScript movementScript = other.gameObject.GetComponent<CustomerMovementScript>();

            CustomerSpawnerScript.OnCustomerExit?.Invoke(movementScript);
            Destroy(other.gameObject);
        }
    }
}
