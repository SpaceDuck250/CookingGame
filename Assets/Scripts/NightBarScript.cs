using UnityEngine;
using UnityEngine.UI;

public class NightBarScript : MonoBehaviour
{
    public Image nightBarImage;

    private void Start()
    {
        DaySystemManager.OnDayEnd += OnDayEnd;
        DaySystemManager.OnNightTimerRun += RunBar;
    }

    private void OnDestroy()
    {
        DaySystemManager.OnDayEnd -= OnDayEnd;
        DaySystemManager.OnNightTimerRun -= RunBar;


    }

    public void OnDayEnd(PlayerDailyStats stats)
    {
        SetupNightBar();
    }

    public void SetupNightBar()
    {
        nightBarImage.fillAmount = 1;
        nightBarImage.gameObject.SetActive(true);
    }

    public void RunBar(float timer, float maxTime)
    {
        nightBarImage.fillAmount = timer / maxTime;
        if (timer < 0.1f)
        {
            nightBarImage.gameObject.SetActive(false);
        }
    }
}
