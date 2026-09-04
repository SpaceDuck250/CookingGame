using UnityEngine;
using TMPro;

public class EndDayStatisticsScript : MonoBehaviour
{
    public GameObject shuttersStatsPanel;
    public TextMeshProUGUI moneyEarnedText;
    public TextMeshProUGUI currentDayText;
    public TextMeshProUGUI tipsEarnedText;

    private void Start()
    {
        DaySystemManager.OnDayEnd += OnDayEnd;
        DaySystemManager.OnDayStart += OnDayStart;
    }

    private void OnDestroy()
    {
        DaySystemManager.OnDayEnd -= OnDayEnd;
        DaySystemManager.OnDayStart -= OnDayStart;


    }

    public void OnDayEnd(PlayerDailyStats playerStats)
    {
        currentDayText.text = "Day " + playerStats.day.ToString() + " Complete!";

        float profit = (float)playerStats.moneyGained;

        
        moneyEarnedText.text = "Total Profit: " + profit.ToString() + "$";
        tipsEarnedText.text = "Tips Earned: " + playerStats.tipEarned.ToString() + "$";

        shuttersStatsPanel.SetActive(true);

    }

    public void OnDayStart()
    {
        shuttersStatsPanel.SetActive(false);

    }
}
