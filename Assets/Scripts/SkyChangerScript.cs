using UnityEngine;

public class SkyChangerScript : MonoBehaviour
{
    public Material morningSky, afternoonSky, EveningSky;

    private void OnEnable()
    {
        TimeCycleScript.OnTimeOfDayChanged += ChangeSky;
    }

    private void OnDisable()
    {
        TimeCycleScript.OnTimeOfDayChanged -= ChangeSky;
    }

    public void ChangeSky(TimeOfDay timeOfDay)
    {
        switch (timeOfDay)
        {
            case TimeOfDay.Day:
                RenderSettings.skybox = morningSky;
                break;

            case TimeOfDay.Afternoon:
                RenderSettings.skybox = afternoonSky;
                break;

            case TimeOfDay.Evening:
                RenderSettings.skybox = EveningSky;
                break;
        }
    }
}
