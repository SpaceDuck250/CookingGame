using UnityEngine;

public class RatSpawnerScript : MonoBehaviour
{
    public GameObject ratPrefab;

    public float chanceOfSpawning = 0;

    public float spawnTimer;
    public float waitTime = 20;

    public Transform ratContainer;
    public Transform spawnPoint;
    public float spawnOffsetRange;

    public FloorManager coldRoomFloorManager;

    public int daysUntilCanSpawn;

    private void Update()
    {
        if (DaySystemManager.dayCounter < daysUntilCanSpawn)
        {
            return;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waitTime)
        {
            spawnTimer = 0;
            CheckIfNeedToDespawnRat();
            TrySpawningRat();
        }
    }

    public void CheckIfNeedToDespawnRat()
    {
        if (ratContainer.childCount == 0)
        {
            return;
        }

        if (coldRoomFloorManager.foodOnFloorList.Count == 0)
        {
            Destroy(ratContainer.transform.GetChild(0).gameObject);
        }
    }

    public void TrySpawningRat()
    {
        if (ratContainer.childCount == 1)
        {
            return;
        }

        int foodOnFloorAmount = coldRoomFloorManager.foodOnFloorList.Count;

        float baseSpawnChance = CalculateBaseSpawnrateFromFoodOnFloor(foodOnFloorAmount);
        if (baseSpawnChance == 0)
        {
            return;
        }

        float extraSpawnChance = CalculateExtraSpawnrateFromDaysPassed(TimeCycleScript.daysPassed);

        chanceOfSpawning = baseSpawnChance + extraSpawnChance;
        chanceOfSpawning = Mathf.Clamp(chanceOfSpawning, 0, 1f);

        if (Random.value < chanceOfSpawning)
        {
            SpawnRat();
        }
    }

    public void SpawnRat()
    {
        GameObject newRat = Instantiate(ratPrefab, spawnPoint.position, Quaternion.identity, ratContainer);

        float offsetX = Random.Range(-spawnOffsetRange, spawnOffsetRange);
        float offsetZ = Random.Range(-spawnOffsetRange, spawnOffsetRange);

        newRat.transform.position += new Vector3(offsetX, 0, offsetZ);

    }


    public float CalculateBaseSpawnrateFromFoodOnFloor(int foodOnFloorAmount)
    {
        if (foodOnFloorAmount == 0)
        {
            return 0;
        }

        if (foodOnFloorAmount > 10) // greater than 10 merry christmas everybody
        {
            return 1f;
        }
        else if (foodOnFloorAmount > 4) // between 5 and 10 tingwei
        {
            return 0.5f;
        }
        else // between 1 and 4 jovens dancing
        {
            return 0.1f;
        }
    }

    public float CalculateExtraSpawnrateFromDaysPassed(int daysPassed)
    {
        int interval = 10;

        float extraSpawnFloat = (daysPassed - daysUntilCanSpawn / interval) * 0.05f;

        return extraSpawnFloat;
    }
}
