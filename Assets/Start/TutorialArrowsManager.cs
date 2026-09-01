using UnityEngine;
using System;
using System.Collections.Generic;

public class TutorialArrowsManager : MonoBehaviour
{
    public static Action<int> OnTutorialTaskComplete;

    public static Action<TaskData> OnShowTaskUI;

    public static Action OnAllTasksComplete;

    public static int currentTaskId = 0;
    public TaskData currentTask;

    public List<GameObject> tutorialArrowList = new List<GameObject>();
    public List<TaskData> taskDataList = new List<TaskData>();

    public bool allTasksFinished = false;

    private void Start()
    {
        foreach (GameObject arrow in tutorialArrowList)
        {
            arrow.SetActive(false);
        }
        //OnTutorialTaskComplete += CompleteTutorialTask;

        //ShowNewArrow(0);
        //currentTask = taskDataList[0];
        //OnShowTaskUI?.Invoke(currentTask);
    }

    private void OnDestroy()
    {
        OnTutorialTaskComplete -= CompleteTutorialTask;

    }

    public void Setup()
    {
        OnTutorialTaskComplete += CompleteTutorialTask;

        ShowNewArrow(0);
        currentTask = taskDataList[0];
        OnShowTaskUI?.Invoke(currentTask);
    }

    // Can have more ui like side bar later
    public void CompleteTutorialTask(int taskID)
    {
        if (taskID >= tutorialArrowList.Count - 2)
        {
            allTasksFinished = true;
            OnAllTasksComplete?.Invoke();
        }

        if (taskID != currentTaskId || taskID >= tutorialArrowList.Count - 1)
        {
            return;
        }

        currentTaskId++;

        currentTask = currentTaskId <= taskDataList.Count - 1 ? taskDataList[currentTaskId] : null;
        ShowNewArrow(currentTaskId);

        OnShowTaskUI?.Invoke(currentTask);
    }

    private void ShowNewArrow(int index)
    {
        foreach (GameObject arrow in tutorialArrowList)
        {
            if (arrow != null)
            {
                arrow.SetActive(false);
            }
        }

        if (tutorialArrowList[index] != null)
        {
            tutorialArrowList[index].SetActive(true);

        }
    }
}
