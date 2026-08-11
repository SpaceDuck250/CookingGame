using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RatSpawnerScript : MonoBehaviour
{
    public GameObject ratPrefab;

    public float changeOfSpawning = 0;

    public float spawnTimer;
    public float waitTime = 20;

    public Transform ratContainer;
    public Transform spawnPoint;

    public FloorManager coldRoomFloorManager;

    private void Update()
    {
        if (ratContainer.childCount >= 1)
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

        changeOfSpawning = baseSpawnChance + extraSpawnChance;

        if (Random.value < changeOfSpawning)
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

        float extraSpawnFloat = (daysPassed / interval) * 0.05f;

        return extraSpawnFloat;
    }
}
