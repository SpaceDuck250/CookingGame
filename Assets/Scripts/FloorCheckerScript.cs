using UnityEngine;
using System.Collections.Generic;
using System;

public class FloorManager : MonoBehaviour
{
    public bool foodOnFloor = false;

    public List<GameObject> foodOnFloorList = new List<GameObject>();

    public Action<GameObject> OnFoodHitThisFloor;
    public static Action<GameObject> OnFoodPickupFromFloor;


    private void Start()
    {
        OnFoodHitThisFloor += AddFoodToList;
        OnFoodPickupFromFloor += TryRemoveFoodFromList;
    }

    private void OnDestroy()
    {
        OnFoodHitThisFloor -= AddFoodToList;
        OnFoodPickupFromFloor += TryRemoveFoodFromList;
    }

    public void AddFoodToList(GameObject foodToAdd)
    {
        foodOnFloorList.Add(foodToAdd);

        foodOnFloor = foodOnFloorList.Count > 0 ? true : false;
    }

    public void TryRemoveFoodFromList(GameObject foodToRemove)
    {
        if (foodOnFloorList.Contains(foodToRemove))
        {
            foodOnFloorList.Remove(foodToRemove);

            foodOnFloor = foodOnFloorList.Count > 0 ? true : false;

        }
    }


}
