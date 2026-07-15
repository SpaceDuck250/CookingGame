using UnityEngine;
using System;

public class HealthInspectorManager : MonoBehaviour
{
    public static HealthInspectorManager instance;

    public static event Action<string, int> OnStallReported;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ReportStall(string reportingCustomerName, int finalScore)
    {
        Debug.Log("[Health Inspector] " + reportingCustomerName + " reported the Stall Final score was " + finalScore);

        OnStallReported?.Invoke(reportingCustomerName, finalScore);

    }
}