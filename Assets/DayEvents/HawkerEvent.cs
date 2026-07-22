using UnityEngine;
using System.Collections.Generic;

public abstract class HawkerEvent : MonoBehaviour
{
    public string eventName;
    public List<GameObject> customersList = new List<GameObject>();
    public float duration;

    public abstract void SetCustomersList();

    public abstract void StartEvent(CustomerSpawnerScript customerSpawner);

    // Does the opposite of start event
    public abstract void ClearEvent();

}
