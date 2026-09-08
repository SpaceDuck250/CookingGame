using UnityEngine;

public class ChopParticleScript : MonoBehaviour
{
    public CarrotCutter cutterScript;

    public GameObject chopParticlePrefab;
    public Transform particleSpawnPoint;

    private void Start()
    {
        if (cutterScript == null)
        {
            return;
        }

        cutterScript.OnChopped += OnChopped;
    }

    private void OnDestroy()
    {
        if (cutterScript == null)
        {
            return;
        }

        cutterScript.OnChopped -= OnChopped;
    }

    private void OnChopped(int currentChops, int requiredChops)
    {
        SpawnBurst();
    }

    private void SpawnBurst()
    {
        if (chopParticlePrefab == null)
        {
            return;
        }

        Vector3 spawnPos = particleSpawnPoint != null ? particleSpawnPoint.position : transform.position;
        Instantiate(chopParticlePrefab, spawnPos, chopParticlePrefab.transform.rotation);
    }
}