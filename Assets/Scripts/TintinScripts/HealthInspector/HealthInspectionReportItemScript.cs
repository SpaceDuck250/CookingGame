using UnityEngine;
using TMPro;

public class HealthInspectionReportItemScript : MonoBehaviour
{
    public TextMeshProUGUI violationNameText;
    public TextMeshProUGUI violationCountText;

    public void SetupItem(string violationName, int count)
    {
        violationNameText.text = violationName;
        violationCountText.text = "x" + count;
    }
}