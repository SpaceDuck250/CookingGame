using UnityEngine;

public class ManualSkyChanger : MonoBehaviour
{
    public Material nightSky;
    public Material daySky;
    public Light directionalLight;
    public float nightIntensity;
    public float dayIntensity;

    private void Start()
    {
        DaySystemManager.OnDayEnd += OnDayEnd;
        DaySystemManager.OnDayStart += OnDayStart;

        daySky = RenderSettings.skybox;
    }

    private void OnDestroy()
    {
        DaySystemManager.OnDayEnd -= OnDayEnd;
        DaySystemManager.OnDayStart -= OnDayStart;


    }

    public void OnDayEnd(PlayerDailyStats stats)
    {
        RenderSettings.skybox = nightSky;

        directionalLight.intensity = nightIntensity;
    }

    public void OnDayStart()
    {
        RenderSettings.skybox = daySky;

        directionalLight.intensity = dayIntensity;
    }
}
