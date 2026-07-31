using UnityEngine;

public class CookingParticleScript : MonoBehaviour
{
    public CookingInputOutputScript inputOutputScript;

    //cooking
    public GameObject cookingParticlePrefab;
    public Transform particleSpawnPoint;

    //success/fail
    public GameObject successParticlePrefab;
    public GameObject failParticlePrefab;

    private GameObject currentCookingParticleInstance;

    private void Start()
    {
        inputOutputScript.OnCookingStart += OnCookingStart;
        inputOutputScript.OnCookingSuccess += OnCookingSuccess;
        inputOutputScript.OnCookingFail += OnCookingFail;
        inputOutputScript.OnFoodTakenOutOfCookingStation += OnFoodTakenOut;
    }

    private void OnDestroy()
    {
        inputOutputScript.OnCookingStart -= OnCookingStart;
        inputOutputScript.OnCookingSuccess -= OnCookingSuccess;
        inputOutputScript.OnCookingFail -= OnCookingFail;
        inputOutputScript.OnFoodTakenOutOfCookingStation -= OnFoodTakenOut;
    }

    private void OnCookingStart(FoodData foodData)
    {
        StartCookingParticles();
    }

    private void OnCookingSuccess(Vector3 spawnPos, GameObject displayObj, Transform parent)
    {
        StopCookingParticles();
        SpawnBurst(successParticlePrefab, spawnPos);
    }

    private void OnCookingFail(Vector3 spawnPos, GameObject displayObj, Transform parent)
    {
        StopCookingParticles();
        SpawnBurst(failParticlePrefab, spawnPos);
    }

    private void OnFoodTakenOut()
    {
        StopCookingParticles();
    }

    private void StartCookingParticles()
    {
        if (cookingParticlePrefab == null)
        {
            return;
        }

        StopCookingParticles();

        Vector3 spawnPos = particleSpawnPoint != null ? particleSpawnPoint.position : transform.position;
        currentCookingParticleInstance = Instantiate(cookingParticlePrefab, spawnPos, Quaternion.identity, particleSpawnPoint);
    }

    private void StopCookingParticles()
    {
        if (currentCookingParticleInstance == null)
        {
            return;
        }

        Destroy(currentCookingParticleInstance);
        currentCookingParticleInstance = null;
    }

    private void SpawnBurst(GameObject burstPrefab, Vector3 position)
    {
        if (burstPrefab == null)
        {
            return;
        }

        Instantiate(burstPrefab, position, Quaternion.identity);
    }
}