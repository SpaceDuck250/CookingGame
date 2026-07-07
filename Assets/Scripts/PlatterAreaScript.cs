using UnityEngine;

public class PlatterAreaScript : MonoBehaviour
{
    public CustomerMovementScript movementScript;

    public GameObject platterPrefab;
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Customer")
        {
            movementScript = other.gameObject.GetComponent<CustomerMovementScript>();
            Destroy(movementScript.mealChecker.customerHand.gameObject);
            movementScript.holdingTray = false;

            GameObject newPlatter = Instantiate(platterPrefab, spawnPoint.position, Quaternion.identity);
            newPlatter.transform.localScale = new Vector3(2.209f, 0.14f, 1.58f);

            //float waitTime = 1.5f;
            //Invoke("MoveToExit", waitTime);
            MoveToExit();
        }
    }

    private void MoveToExit()
    {
        movementScript.OnNewDestinationChange(CustomerSpawnerScript.instance.exitTransform);

    }
}
