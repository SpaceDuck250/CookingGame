using UnityEngine;

public class ChickenHoldTask : TutorialTask
{
    private void OnDestroy()
    {
        CompleteTask();
    }
}
