using UnityEngine;
using System;

public class ColdRoomTempManager : MonoBehaviour
{
    public static int currentColdRoomTemperature;

    public int minTemp = -1, maxTemp = 22;

    public static Action<int> OnColdRoomTempChange;

    private void Start()
    {
        currentColdRoomTemperature = 22;
    }


}
