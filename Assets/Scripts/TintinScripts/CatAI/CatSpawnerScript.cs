using UnityEngine;
using System.Collections;

public class CatSpawnerScript : MonoBehaviour
{
    public GameObject catPrefab;

    public Transform spawnPoint;
    public Transform waitPoint;
    public Transform exitPoint;

    public Transform wanderPointsParent;

    public float minArrivalDelay = 60f;
    public float maxArrivalDelay = 180f;

    public KeyCode testSpawnKey = KeyCode.None;

    private bool catPresent = false;

    private void Start()
    {
        CatAIScript.OnCatLeft += OnCatLeft;
        StartCoroutine(ArrivalLoop());
    }

    private void OnDestroy()
    {
        CatAIScript.OnCatLeft -= OnCatLeft;
    }

    private void Update()
    {
        if (testSpawnKey != KeyCode.None && Input.GetKeyDown(testSpawnKey) && !catPresent)
        {
            SpawnCat();
        }
    }

    private IEnumerator ArrivalLoop()
    {
        while (true)
        {
            float delay = Random.Range(minArrivalDelay, maxArrivalDelay);
            yield return new WaitForSeconds(delay);

            if (!catPresent)
            {
                SpawnCat();
            }
        }
    }

    private void SpawnCat()
    {
        if (catPrefab == null || spawnPoint == null || waitPoint == null || exitPoint == null)
        {
            Debug.LogWarning("[Cat] Missing prefab or points - cannot spawn.");
            return;
        }

        catPresent = true;

        GameObject newCat = Instantiate(catPrefab, spawnPoint.position, spawnPoint.rotation);
        CatAIScript catAI = newCat.GetComponent<CatAIScript>();

        if (catAI == null)
        {
            Debug.LogWarning("[Cat] Cat prefab has no CatAIScript component.");
            Destroy(newCat);
            catPresent = false;
            return;
        }

        catAI.BeginVisit(waitPoint, exitPoint, wanderPointsParent);
    }

    private void OnCatLeft()
    {
        catPresent = false;
    }
}