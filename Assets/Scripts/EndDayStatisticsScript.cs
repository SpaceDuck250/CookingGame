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
    }

    private void OnDestroy()
    {
        DaySystemManager.OnDayEnd -= OnDayEnd;

    }

    public void OnDayEnd(PlayerDailyStats playerStats)
    {
        currentDayText.text = "Day " + playerStats.day.ToString() + " Complete!";

        float profit = (float)(playerStats.totalMoneyEndOfDay - playerStats.totalMoneyStartOfDay);
        moneyEarnedText.text = "Total Profit: " + profit.ToString() + "$";
        tipsEarnedText.text = "Tips Earned: " + playerStats.tipEarned.ToString() + "$";

        shuttersStatsPanel.SetActive(true);

    }
}
