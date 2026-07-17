using System.Collections;
using UnityEngine;

public class AIEventSystemScript : MonoBehaviour
{
    public static AIEventSystemScript Instance { get; private set; }

    public CustomerSpawnerScript customerSpawner;

    public HawkerEventType currentEvent = HawkerEventType.None;

    // Event parameters for Rush Hour
    private int rushHourCustomerAmount = 4;
    private float rushHourSpawnDelay = 1f;

    // Event parameters for Fussy Customer
    private int fussyCustomerAmount = 2;

    private IEnumerator RushHourRoutine()
    {
        currentEvent = HawkerEventType.RushHour;
        customerSpawner.eventRushHour = true;

        int spawnedCustomers = 0;

        while (spawnedCustomers < rushHourCustomerAmount)
        {
            //bool spawned = customerSpawner.TrySpawnCustomer();

            //if (!spawned)
            //{
            //    break;
            //}

            spawnedCustomers++;

            yield return new WaitForSeconds(
                rushHourSpawnDelay);
        }

        customerSpawner.eventRushHour = false;
        currentEvent = HawkerEventType.None;
    }
}
