using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskBarSetupper : MonoBehaviour
{
    public TextMeshProUGUI taskNameText;
    public TextMeshProUGUI taskDescriptionText;

    //public float downAmount;
    //private bool goneDown = false;

    public void SetupTaskBar(TaskData taskData)
    {
        //CustomerInteractScript.OnAnyCustomerInteract += MoveDown;

        taskNameText.text = taskData.taskName;
        taskDescriptionText.text = taskData.taskDescription;
    }

    //private void OnDestroy()
    //{
    //    CustomerInteractScript.OnAnyCustomerInteract -= MoveDown;

    //}

    //public void MoveDown(CustomerStateMachine csm)
    //{
    //    if (goneDown)
    //    {
    //        return;
    //    }

    //    goneDown = true;
    //    transform.GetComponent<RectTransform>().position += Vector3.down * downAmount;
    //}
}

