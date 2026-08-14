using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthInspectorSpawnerScript : MonoBehaviour
{
    // Chill and Strict prefab variants go here - one is picked at random each time an inspector is spawned
    public List<GameObject> inspectorPrefabs = new List<GameObject>();

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
        GameObject chosenPrefab = PickRandomInspectorPrefab();

        if (chosenPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("[Health Inspector] Missing inspectorPrefabs or spawnPoint - cannot spawn.");
            inspectionInProgress = false;
            return;
        }

        GameObject newInspector = Instantiate(chosenPrefab, spawnPoint.position, spawnPoint.rotation);
        HealthInspectorAIScript inspectorAI = newInspector.GetComponent<HealthInspectorAIScript>();

        if (inspectorAI == null)
        {
            Debug.LogWarning("[Health Inspector] Inspector prefab has no HealthInspectorAIScript component.");
            Destroy(newInspector);
            inspectionInProgress = false;
            return;
        }

        Debug.Log("[Health Inspector] Spawning " + chosenPrefab.name + " (strictness: " + inspectorAI.strictness + ")");

        inspectorAI.BeginInspection(exitPoint, inspectionPoints);
    }

    private GameObject PickRandomInspectorPrefab()
    {
        if (inspectorPrefabs == null || inspectorPrefabs.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, inspectorPrefabs.Count);
        return inspectorPrefabs[randomIndex];
    }

    private void OnInspectionComplete()
    {
        inspectionInProgress = false;
    }
}