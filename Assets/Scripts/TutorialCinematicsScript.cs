using UnityEngine;

public class TutorialCinematicsScript : MonoBehaviour
{
    public Camera playerCam;
    public Camera busCam;
    public Camera deliveryCam;
    public Camera endTutorialCamera;

    //public Camera currentCam;

    public TutorialArrowsManager tutorialArrowsManager;
    public GameObject actualCanvas;
    public GameObject tutorialEndCanvas;

    private void Start()
    {
        //CustomerInteractScript.OnEndInteractWithCustomer += SwitchToEndCamera;
        TutorialArrowsManager.OnAllTasksComplete += PlayEndScreen;
    }

    private void OnDestroy()
    {

        //CustomerInteractScript.OnEndInteractWithCustomer -= SwitchToEndCamera;
        TutorialArrowsManager.OnAllTasksComplete -= PlayEndScreen;

    }

    private void PlayEndScreen()
    {
        PlayerHandScript.instance.FreezePlayer(true, null);
        actualCanvas.SetActive(false);
        //SetNewCamera(endTutorialCamera);

        Invoke("ShowEndAnim", 0.5f);
    }

    public void ShowEndAnim()
    {
        tutorialEndCanvas.SetActive(true);
    }


    public static void SetNewCamera(Camera newCam)
    {
        //currentCam = newCam;
        //currentCam.gameObject.SetActive(true);
        newCam.gameObject.SetActive(true);

        Camera[] allCameraList = Camera.allCameras;

        foreach (Camera cam in allCameraList)
        {
            if (cam != newCam)
            {
                cam.gameObject.SetActive(false);
            }
        }
    }


}
