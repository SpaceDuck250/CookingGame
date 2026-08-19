using UnityEngine;
using System;
using System.Collections.Generic;

public class TutorialArrowsManager : MonoBehaviour
{
    public static Action<int> OnTutorialTaskComplete;

    public int currentTaskId = 0;

    public List<GameObject> tutorialArrowList;

    private void Start()
    {
        OnTutorialTaskComplete += CompleteTutorialTask;

        ShowNewArrow(0);
    }

    private void OnDestroy()
    {
        OnTutorialTaskComplete -= CompleteTutorialTask;

    }

    // Can have more ui like side bar later
    public void CompleteTutorialTask(int taskID)
    {
        if (taskID != currentTaskId || taskID >= tutorialArrowList.Count - 1)
        {
            return;
        }

        currentTaskId++;

        ShowNewArrow(currentTaskId);
    }

    private void ShowNewArrow(int index)
    {
        foreach (GameObject arrow in tutorialArrowList)
        {
            arrow.SetActive(false);
        }

        tutorialArrowList[index].SetActive(true);
    }
}
