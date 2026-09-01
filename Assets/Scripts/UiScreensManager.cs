using UnityEngine;
using System.Collections.Generic;

public class UiScreensManager : MonoBehaviour
{
    public List<GameObject> uiScreenList = new List<GameObject>();

    public bool CheckIfAllAreInactive()
    {
        foreach (GameObject uiScreen in uiScreenList)
        {
            if (uiScreen.activeSelf)
            {
                return false;
            }
        }

        return true;
    }
}