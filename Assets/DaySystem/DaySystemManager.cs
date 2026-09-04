using UnityEngine;
using System;

public class DaySystemManager : MonoBehaviour, ISaveable
{
    public static int dayCounter;

    //public int customersServedToday = 0;
    public PlayerDailyStats playerDailyStats;

    public int serveMin = 3;
    public int serveMax = 20;

    public int customerServeRequirement;

    public static Action OnDayStart;
    public static Action<PlayerDailyStats> OnDayEnd;

    public static Action<float, float> OnNightTimerRun;

    public CustomerSpawnerScript customerSpawner;

    public float nightTimer;
    public float nightTime;
    public bool isNight;

    private void Start()
    {
        SaveLoadManager.OnSaveGame += SaveSelf;

        MealChecker.OnAnyCustomerServed += CountServedCustomers;
        //dayCounter = 0;
        OnDayStart += SetupDayStart;
        MoneyManager.OnMoneyChanged += KeepTrackOfMoneyStatistics;

        OnDayEnd += OnDayEndFunction;

        customerServeRequirement = serveMin;
    }

    private void OnDestroy()
    {
        SaveLoadManager.OnSaveGame -= SaveSelf;

        MealChecker.OnAnyCustomerServed -= CountServedCustomers;

        OnDayStart -= SetupDayStart;

        MoneyManager.OnMoneyChanged -= KeepTrackOfMoneyStatistics;

        OnDayEnd -= OnDayEndFunction;

    }

    private void Update()
    {
        //// for testing and checking
        if (Input.GetKeyDown(KeyCode.C))
        {
            UpdatePlayerStats();
            OnDayEnd?.Invoke(playerDailyStats);
        }


        if (!isNight)
        {
            return;
        }

        nightTimer -= Time.deltaTime;
        if (nightTimer <= 0)
        {
            IncrementDay();
            TryIncreaseServeRequirement();

            OnDayStart?.Invoke();
        }

        OnNightTimerRun?.Invoke(nightTimer, nightTime);
    }

    public void CountServedCustomers()
    {
        playerDailyStats.customersServed++;
        if (playerDailyStats.customersServed >= customerServeRequirement)
        {
            UpdatePlayerStats();
            OnDayEnd?.Invoke(playerDailyStats);
        }
    }

    public void OnDayEndFunction(PlayerDailyStats playerStats)
    {
        customerSpawner.canSpawn = false;
        SetupNight();

        //playerDailyStats = null;
    }

    public void SetupDayStart()
    {
        // Might change later
        //if (UnityEngine.Random.value < 0.4f)
        //{
        //    SetCustomerServeRequirement(customerServeRequirement + 1);
        //}

        //dayCounter++;

        SetNewPlayerStatsForNewDay();


        customerSpawner.canSpawn = true;

        isNight = false;

    }

    public void IncrementDay()
    {
        dayCounter++;
    }

    public void TryIncreaseServeRequirement()
    {
        if (UnityEngine.Random.value < 0.4f)
        {
            SetCustomerServeRequirement(customerServeRequirement + 1);
        }
    }

    public void SetupNight()
    {
        nightTimer = nightTime;
        isNight = true;
    }

    public void SetCustomerServeRequirement(int newValue)
    {
        if (newValue < serveMin)
        {
            customerServeRequirement = serveMin;
            return;
        }
        else if (newValue > serveMax)
        {
            customerServeRequirement = serveMax;
            return;
        }

        customerServeRequirement = newValue;
    }

    public void KeepTrackOfMoneyStatistics(decimal playerTotalMoney, decimal moneyEarned, decimal tipEarned)
    {
        playerDailyStats.tipEarned += tipEarned;
    }

    public void SetNewPlayerStatsForNewDay()
    {
        playerDailyStats = new PlayerDailyStats(dayCounter, MoneyManager.playerMoneyAmount);
    }

    public void UpdatePlayerStats()
    {
        playerDailyStats.customersServed = customerServeRequirement;
        playerDailyStats.totalMoneyEndOfDay = MoneyManager.playerMoneyAmount;
    }

    public void SaveSelf()
    {
        SaveLoadManager.gameData.currentDay = dayCounter;
        SaveLoadManager.gameData.serveRequirement = customerServeRequirement;
    }

  

}

[Serializable]
public class PlayerDailyStats
{
    public PlayerDailyStats(int currentDay, decimal startMoney)
    {
        day = currentDay;
        totalMoneyStartOfDay = startMoney;
        tipEarned = 0;
    }

    public int day;
    public decimal totalMoneyStartOfDay;
    public decimal totalMoneyEndOfDay;

    public decimal moneyGained => totalMoneyEndOfDay - totalMoneyStartOfDay;

    public decimal tipEarned = 0;

    public int customersServed = 0;

}