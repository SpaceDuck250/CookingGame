using UnityEngine;

public class LookCorrectlyScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Customer")
        {
            return;
        }

        if (!other.gameObject.GetComponent<CustomerMovementScript>().CheckIfCloseEnoughToDestination())
        {
            return;
        }

        other.gameObject.GetComponent<CustomerInteractScript>().RotateTo(CustomerSpawnerScript.instance.mainCounterPoint.gameObject);
    }
}
