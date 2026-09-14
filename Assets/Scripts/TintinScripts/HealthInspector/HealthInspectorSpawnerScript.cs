using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

public class HealthInspectorSpawnerScript : MonoBehaviour
{
    public static event Action OnInspectorSpawned;

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
            inspectionInProgress = false;
            return;
        }

        GameObject newInspector = Instantiate(chosenPrefab, spawnPoint.position, spawnPoint.rotation);
        HealthInspectorAIScript inspectorAI = newInspector.GetComponent<HealthInspectorAIScript>();

        if (inspectorAI == null)
        {
            Destroy(newInspector);
            inspectionInProgress = false;
            return;
        }


        inspectorAI.BeginInspection(exitPoint, inspectionPoints);
        OnInspectorSpawned?.Invoke();
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