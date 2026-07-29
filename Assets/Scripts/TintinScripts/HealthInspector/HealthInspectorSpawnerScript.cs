using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthInspectorSpawnerScript : MonoBehaviour
{
    public GameObject inspectorPrefab;

    public Transform spawnPoint;
    public Transform exitPoint;

    public List<HealthInspectionPointScript> inspectionPoints = new List<HealthInspectionPointScript>();

    public float minArrivalDelay = 30f;
    public float maxArrivalDelay = 90f;

    private bool inspectionInProgress = false;

    private void Start()
    {
        HealthInspectorManager.OnStallReported += OnStallReported;
        HealthInspectorAIScript.OnInspectionComplete += OnInspectionComplete;
    }

    private void OnDestroy()
    {
        HealthInspectorManager.OnStallReported -= OnStallReported;
        HealthInspectorAIScript.OnInspectionComplete -= OnInspectionComplete;
    }

    private void OnStallReported(string reportingCustomerName, int finalScore)
    {
        if (inspectionInProgress)
        {
            return;
        }

        inspectionInProgress = true;

        float delay = Random.Range(minArrivalDelay, maxArrivalDelay);
        Debug.Log("[Health Inspector] Report received from " + reportingCustomerName + ". Inspector arriving in " + delay.ToString("F0") + "s.");

        StartCoroutine(SpawnAfterDelay(delay));
    }

    private IEnumerator SpawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnInspector();
    }

    private void SpawnInspector()
    {
        if (inspectorPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("[Health Inspector] Missing inspectorPrefab or spawnPoint - cannot spawn.");
            inspectionInProgress = false;
            return;
        }

        GameObject newInspector = Instantiate(inspectorPrefab, spawnPoint.position, spawnPoint.rotation);
        HealthInspectorAIScript inspectorAI = newInspector.GetComponent<HealthInspectorAIScript>();

        if (inspectorAI == null)
        {
            Debug.LogWarning("[Health Inspector] Inspector prefab has no HealthInspectorAIScript component.");
            Destroy(newInspector);
            inspectionInProgress = false;
            return;
        }

        inspectorAI.BeginInspection(exitPoint, inspectionPoints);
    }

    private void OnInspectionComplete()
    {
        inspectionInProgress = false;
    }
}