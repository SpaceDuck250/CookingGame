using System.Collections;
using UnityEngine;

public class AIEventSystemScript : MonoBehaviour
{
    public static AIEventSystemScript Instance { get; private set; }

    public CustomerSpawnerScript customerSpawner;

    public HawkerEventType currentEvent = HawkerEventType.None;

    // Event parameters for Rush Hour
    public int rushHourCustomerAmount = 4;
    public float rushHourSpawnDelay = 3f;

    // Event parameters for Fussy Customer
    public int fussyCustomerAmount = 2;




}
