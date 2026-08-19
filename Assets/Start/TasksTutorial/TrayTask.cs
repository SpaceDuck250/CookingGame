using UnityEngine;

public class TrayTask : TutorialTask
{
    public TrayRackScript trayRackScript;

    private void Start()
    {
        trayRackScript.OnTrayTakenOut += OnTrayTakenOut;
    }

    private void OnTrayTakenOut()
    {
        CompleteTask();
    }
}
