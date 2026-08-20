using UnityEngine;

public abstract class TutorialTask : MonoBehaviour
{
    public int taskId;

    public bool completed = false;

    public virtual void CompleteTask()
    {
        if (completed)
        {
            return;
        }

        TutorialArrowsManager.OnTutorialTaskComplete?.Invoke(taskId);

        completed = true;
    }
}
