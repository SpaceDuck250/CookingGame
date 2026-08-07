using UnityEngine;

public class TutorialBusScript : MonoBehaviour
{
    public TutorialManagerScript tutorialManager;

    public void SetPlayerAsActive()
    {
        tutorialManager.ActivatePlayer();
        tutorialManager.SetPlayerCamOn();
    }
}
