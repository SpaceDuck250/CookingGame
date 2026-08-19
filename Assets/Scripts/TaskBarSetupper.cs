using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskBarSetupper : MonoBehaviour
{
    public TextMeshProUGUI taskNameText;
    public TextMeshProUGUI taskDescriptionText;

    public void SetupTaskBar(TaskData taskData)
    {
        taskNameText.text = taskData.taskName;
        taskDescriptionText.text = taskData.taskDescription;
    }
}
