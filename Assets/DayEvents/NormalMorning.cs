using UnityEngine;

public class NormalMorning : HawkerEvent
{

    // Add all the customers that are ABLE to spawn in this event in here
    public override void SetCustomersList()
    {
        // The number is the percentage chance of that customer spawning for that event (It should add up to 1)

        customersList.Add(CustomerSpawnerScript.customerDictionary["JovenVariant"], 0.3f);
        customersList.Add(CustomerSpawnerScript.customerDictionary["PandaVariant"], 0.7f);
    }

    public override void StartEvent(CustomerSpawnerScript customerSpawner)
    {
        customerSpawner.maxCustomers = 5;
        customerSpawner.spawnInterval = 5;
    }

}
