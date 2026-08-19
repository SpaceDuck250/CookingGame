using UnityEngine;

public class TaskUIManager : MonoBehaviour
{
    public TaskBarSetupper taskBarObj;

    private void Start()
    {
        TutorialArrowsManager.OnShowTaskUI += CreateNewTaskBar;
    }

    private void OnDestroy()
    {
        TutorialArrowsManager.OnShowTaskUI -= CreateNewTaskBar;
    }

    public void CreateNewTaskBar(TaskData taskData)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        TaskBarSetupper newTaskbar = Instantiate(taskBarObj, transform.position, Quaternion.identity, transform);
        newTaskbar.SetupTaskBar(taskData);
        
    }
}
