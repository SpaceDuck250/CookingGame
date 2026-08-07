using UnityEngine;
using System.Collections.Generic;

public class TutorialManagerScript : MonoBehaviour
{
    public List<GameObject> InactiveGameObjectList = new List<GameObject>();

    public GameObject playerObj;

    public List<Camera> allCamerasList = new List<Camera>();

    public Camera playerCam;

    private void Start()
    {
        foreach (GameObject obj in InactiveGameObjectList)
        {
            obj.SetActive(false);
        }
    }

    public void ActivatePlayer()
    {
        playerObj.SetActive(true);
    }

    public void SetPlayerCamOn()
    {
        SetCameraAsMain(playerCam);
    }

    public void SetCameraAsMain(Camera mainCam)
    {
        foreach (Camera camera in allCamerasList)
        {
            camera.gameObject.SetActive(false);
        }

        mainCam.gameObject.SetActive(true);

    }
}
