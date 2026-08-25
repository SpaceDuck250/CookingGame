using UnityEngine;
using System;

public class DaySystemManager : MonoBehaviour
{
    public int dayCounter = 1;

    //public int customersServedToday = 0;
    public PlayerDailyStats playerDailyStats;

    public int serveMin = 3;
    public int serveMax = 20;

    public int customerServeRequirement;

    public static Action OnDayStart;
    public static Action<PlayerDailyStats> OnDayEnd;

    private void Start()
    {
        MealChecker.OnAnyCustomerServed += CountServedCustomers;
        OnDayStart += SetupDayStart;
        MoneyManager.OnMoneyChanged += KeepTrackOfMoneyStatistics;

        customerServeRequirement = serveMin;

        OnDayStart?.Invoke();
    }

    private void OnDestroy()
    {
        MealChecker.OnAnyCustomerServed -= CountServedCustomers;
        OnDayStart -= SetupDayStart;

        MoneyManager.OnMoneyChanged -= KeepTrackOfMoneyStatistics;



    }

    public void CountServedCustomers()
    {
        playerDailyStats.customersServed++;
        if (playerDailyStats.customersServed >= customerServeRequirement)
        {
            playerDailyStats.customersServed = customerServeRequirement;
            playerDailyStats.totalMoneyEndOfDay = MoneyManager.playerMoneyAmount;

            OnDayEnd?.Invoke(playerDailyStats);
            playerDailyStats = null;

            print(playerDailyStats);

            Time.timeScale = 1;
        }
    }

    public void SetupDayStart()
    {
        // Might change later
        if (UnityEngine.Random.value < 0.4f && dayCounter != 1)
        {
            SetCustomerServeRequirement(customerServeRequirement + 1);
        }

        dayCounter++;

        playerDailyStats = new PlayerDailyStats(dayCounter, MoneyManager.playerMoneyAmount);

        Time.timeScale = 1;

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

    public decimal tipEarned = 0;

    public int customersServed = 0;
    public int secondsElapsed = 0;

}