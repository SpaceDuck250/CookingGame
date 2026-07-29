using UnityEngine;

public class FrierParticleScript : MonoBehaviour
{
    public FrierInteractScript frierScript;

    public GameObject smokeParticlePrefab;
    public Transform particleSpawnPoint;

    public float baseScale = 1f;
    public float scalePerHeatLevel = 0.5f; 

    private GameObject currentSmokeInstance;

    private void Start()
    {
        frierScript.OnFry += OnFry;
        frierScript.OnFryEnd += OnFryEnd;
        frierScript.OnChangeHeatLevel += OnChangeHeatLevel;
    }

    private void OnDestroy()
    {
        frierScript.OnFry -= OnFry;
        frierScript.OnFryEnd -= OnFryEnd;
        frierScript.OnChangeHeatLevel -= OnChangeHeatLevel;
    }

    private void OnFry(GameObject foodBeingFried)
    {
        StartSmoke();
        UpdateSmokeScale(frierScript.currentHeatLevel);
    }

    private void OnFryEnd()
    {
        StopSmoke();
    }

    private void OnChangeHeatLevel(FrierInteractScript.HeatLevel heatLevel)
    {
        if (heatLevel.name == "Off")
        {
            StopSmoke();
            return;
        }

        if (currentSmokeInstance == null && frierScript.cooking)
        {
            StartSmoke();
        }

        UpdateSmokeScale(heatLevel);
    }

    private void StartSmoke()
    {
        if (smokeParticlePrefab == null || currentSmokeInstance != null)
        {
            return;
        }

        Vector3 spawnPos = particleSpawnPoint != null ? particleSpawnPoint.position : transform.position;

        currentSmokeInstance = Instantiate(smokeParticlePrefab, spawnPos, smokeParticlePrefab.transform.rotation, particleSpawnPoint);
    }

    private void StopSmoke()
    {
        if (currentSmokeInstance == null)
        {
            return;
        }

        Destroy(currentSmokeInstance);
        currentSmokeInstance = null;
    }

    private void UpdateSmokeScale(FrierInteractScript.HeatLevel heatLevel)
    {
        if (currentSmokeInstance == null || heatLevel == null)
        {
            return;
        }

        float targetScale = baseScale + (heatLevel.speedIncreaser * scalePerHeatLevel);
        currentSmokeInstance.transform.localScale = Vector3.one * targetScale;
    }
}