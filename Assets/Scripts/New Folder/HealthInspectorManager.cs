using UnityEngine;
using System;

public class HealthInspectorManager : MonoBehaviour
{
    public static HealthInspectorManager instance;

    public static event Action<string, int> OnRestaurantReported;

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

    public static void ReportRestaurant(string reportingCustomerName, int finalScore)
    {
        Debug.Log("[Health Inspector] " + reportingCustomerName + " reported the restaurant! Final score was " + finalScore);

        OnRestaurantReported?.Invoke(reportingCustomerName, finalScore);

    }
}