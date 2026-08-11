using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RatSpawnerScript : MonoBehaviour
{
    public GameObject ratPrefab;

    public float chanceOfSpawning = 0;

    public float spawnTimer;
    public float waitTime = 20;

    public Transform ratContainer;
    public Transform spawnPoint;

    public FloorManager coldRoomFloorManager;

    public int daysUntilCanSpawn;

    private void Update()
    {
        if (ratContainer.childCount >= 1 || TimeCycleScript.daysPassed < daysUntilCanSpawn)
        {
            return;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waitTime)
        {
            spawnTimer = 0;

            TrySpawningRat();
        }
    }

    public void TrySpawningRat()
    {
        int foodOnFloorAmount = coldRoomFloorManager.foodOnFloorList.Count;

        float baseSpawnChance = CalculateBaseSpawnrateFromFoodOnFloor(foodOnFloorAmount);
        if (baseSpawnChance == 0)
        {
            return;
        }

        float extraSpawnChance = CalculateExtraSpawnrateFromDaysPassed(TimeCycleScript.daysPassed);

        chanceOfSpawning = baseSpawnChance + extraSpawnChance;
        chanceOfSpawning = Mathf.Clamp(chanceOfSpawning, 0, 0.9f);

        if (Random.value < chanceOfSpawning)
        {
            SpawnRat();
        }
    }

    public void SpawnRat()
    {
        GameObject newRat = Instantiate(ratPrefab, spawnPoint.position, Quaternion.identity, ratContainer);
    }


    public float CalculateBaseSpawnrateFromFoodOnFloor(int foodOnFloorAmount)
    {
        if (foodOnFloorAmount == 0)
        {
            return 0;
        }

        if (foodOnFloorAmount > 10) // greater than 10 merry christmas everybody
        {
            return 0.4f;
        }
        else if (foodOnFloorAmount > 4) // between 5 and 10 tingwei
        {
            return 0.2f;
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
