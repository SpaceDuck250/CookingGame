using UnityEngine;

public class NormalMorning : HawkerEvent
{

    // Add all the customers that are ABLE to spawn in this event in here
    public override void SetCustomersList()
    {
        customersList.Add(CustomerSpawnerScript.customerDictionary["JovenVariant"]);
        customersList.Add(CustomerSpawnerScript.customerDictionary["PandaVariant"]);
    }

    public override void StartEvent(CustomerSpawnerScript customerSpawner)
    {
            
    }

    public override void ClearEvent()
    {
        
    }
}
