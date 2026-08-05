using UnityEngine;

public class TimeLightingScript : MonoBehaviour
{
    public Light directionalLight;

    public float dayIntensity = 1f;
    public float afternoonIntensity = 0.7f;
    public float eveningIntensity = 0.3f;

    private void Start()
    {
        if (TimeCycleScript.Instance != null)
        {
            HandleTimeOfDayChanged(TimeCycleScript.Instance.currentTimeOfDay);
        }
    }

    private void OnEnable()
    {
        TimeCycleScript.OnTimeOfDayChanged += HandleTimeOfDayChanged;
    }

    private void OnDisable()
    {
        TimeCycleScript.OnTimeOfDayChanged -= HandleTimeOfDayChanged;
    }

    // Handles the time of the day for the lightings
    private void HandleTimeOfDayChanged(TimeOfDay timeOfDay)
    {
        if (directionalLight == null)
        {
            return;
        }

        switch (timeOfDay)
        {
            case TimeOfDay.Day:
                directionalLight.intensity = dayIntensity;
                break;

            case TimeOfDay.Afternoon:
                directionalLight.intensity = afternoonIntensity;
                break;

            case TimeOfDay.Evening:
                directionalLight.intensity = eveningIntensity;
                break;
        }

        Debug.Log("Lighting changed for: " + timeOfDay);
    }
}
