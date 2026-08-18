using Customer;
using UnityEngine;

public class PlatterAreaScript : MonoBehaviour
{
    public CustomerMovementScript movementScript;

    public GameObject platterPrefab;
    public Transform spawnPoint;

    public TrayRackScript trayRackScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            movementScript = other.gameObject.GetComponent<CustomerMovementScript>();

            // remove customer's platter child (not the parent hand object) and update flags
            if (movementScript != null && movementScript.mealChecker != null && movementScript.mealChecker.customerHand != null)
            {
                Transform hand = movementScript.mealChecker.customerHand;
                Transform childToDestroy = null;

                // Search the Platter child that has a PlatterScript
                for (int i = 0; i < hand.childCount; i++)
                {
                    Transform ch = hand.GetChild(i);
                    if (ch.GetComponent<PlatterScript>() != null)
                    {
                        childToDestroy = ch;
                        break;
                    }
                }

                // The child tagged "Platter"
                if (childToDestroy == null)
                {
                    for (int i = 0; i < hand.childCount; i++)
                    {
                        Transform ch = hand.GetChild(i);
                        if (ch.CompareTag("Platter"))
                        {
                            childToDestroy = ch;
                            break;
                        }
                    }
                }

                if (childToDestroy == null && hand.childCount > 0)
                {
                    childToDestroy = hand.GetChild(0);
                }

                if (childToDestroy != null)
                {
                    Destroy(childToDestroy.gameObject);
                    // clear platter reference in MealChecker so code doesn't hold a stale reference
                    movementScript.mealChecker.platterHeld = null;
                }
            }

            if (movementScript != null)
            {
                movementScript.holdingTray = false;
            }

            // spawn platter visual
            //GameObject newPlatter = Instantiate(platterPrefab, spawnPoint.position, Quaternion.identity);
            //newPlatter.transform.localScale = new Vector3(2, 2, 2);
            TryReturnTray();

            // Tell the customer's state machine to leave the map
            CustomerStateMachine csm = other.gameObject.GetComponent<CustomerStateMachine>();
            if (csm != null)
            {
                csm.OnCustomerChangeState?.Invoke(CustomerState.LeavingMap);
            }

            // move movementScript to exit immediately
            // if (movementScript != null)
            //{
            //     movementScript.OnNewDestinationChange?.Invoke(CustomerSpawnerScript.instance.exitTransform);
            // }
        }
    }

    public void TryReturnTray()
    {
        trayRackScript.ReturnTray(PlayerHandScript.instance);
    }
}
