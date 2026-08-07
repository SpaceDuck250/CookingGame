using System;
using UnityEngine;

public class TimeCycleScript : MonoBehaviour
{
    public static TimeCycleScript Instance { get; private set; }
    public static event Action<TimeOfDay> OnTimeOfDayChanged;

    // Time Cycle
    public float timePeriodDuration = 200f;
    public static TimeOfDay currentTimeOfDay = TimeOfDay.Day;
    public float timePeriodTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timePeriodTimer = 0f;

        // The starting period
        OnTimeOfDayChanged?.Invoke(currentTimeOfDay);
    }

    private void Update()
    {
        timePeriodTimer += Time.deltaTime;

        // Change the time period after set seconds
        if (timePeriodTimer >= timePeriodDuration)
        {
            timePeriodTimer = 0f;

            ChangeToNextTimePeriod();
        }
    }

    // State machine for the period of times
    private void ChangeToNextTimePeriod()
    {
        switch (currentTimeOfDay)
        {
            case TimeOfDay.Day:
                SetTimeOfDay(TimeOfDay.Afternoon);
                break;

            case TimeOfDay.Afternoon:
                SetTimeOfDay(TimeOfDay.Evening);
                break;

            case TimeOfDay.Evening:
                SetTimeOfDay(TimeOfDay.Day);
                break;
        }
    }

    // The setting the time of day
    private void SetTimeOfDay(TimeOfDay newTimeOfDay)
    {
        currentTimeOfDay = newTimeOfDay;

        Debug.Log("Time changed to: " + currentTimeOfDay);

        OnTimeOfDayChanged?.Invoke(currentTimeOfDay);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
