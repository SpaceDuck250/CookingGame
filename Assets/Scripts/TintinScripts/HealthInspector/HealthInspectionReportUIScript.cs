using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthInspectionReportUIScript : MonoBehaviour
{
    public GameObject reportPanel;

    public Transform violationListContainer;
    public GameObject violationItemTemplate;

    public TextMeshProUGUI resultHeaderText;
    public TextMeshProUGUI totalFineText;

    public Color passedColor = Color.green;
    public Color failedColor = Color.red;

    private void OnEnable()
    {
        HealthInspectorAIScript.OnInspectionReport += ShowReport;
    }

    private void OnDisable()
    {
        HealthInspectorAIScript.OnInspectionReport -= ShowReport;
    }

    private void ShowReport(Dictionary<string, int> violationTally, decimal totalFine, bool passed)
    {
        ClearContainer();

        foreach (KeyValuePair<string, int> violation in violationTally)
        {
            GameObject newItem = Instantiate(violationItemTemplate, violationListContainer);
            newItem.GetComponent<HealthInspectionReportItemScript>().SetupItem(violation.Key, violation.Value);
        }

        if (passed)
        {
            resultHeaderText.text = "Inspection Passed";
            resultHeaderText.color = passedColor;
            totalFineText.text = "$0";
        }
        else
        {
            resultHeaderText.text = "Inspection Failed";
            resultHeaderText.color = failedColor;
            totalFineText.text = "-$" + totalFine;
        }

        reportPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Hook this up to a "Close" button's OnClick
    public void CloseReport()
    {
        reportPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ClearContainer()
    {
        foreach (Transform child in violationListContainer)
        {
            Destroy(child.gameObject);
        }
    }
}