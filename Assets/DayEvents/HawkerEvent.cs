using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public abstract class HawkerEvent : MonoBehaviour
{
    public string eventName;
    //public List<GameObject> customersList = new List<GameObject>();
    public Dictionary<GameObject, float> customersList = new Dictionary<GameObject, float>();

    public abstract void SetCustomersList();

    public abstract void StartEvent(CustomerSpawnerScript customerSpawner);

}
