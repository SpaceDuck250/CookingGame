using System;
using System.Collections.Generic;
using UnityEngine;

// The logic for hitting floor is in holdablefoodscript
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
        if (foodOnFloorList.Contains(foodToAdd))
        {
            return;
        }

        foodOnFloorList.Add(foodToAdd);

        foodOnFloor = foodOnFloorList.Count > 0 ? true : false;
    }

    public void TryRemoveFoodFromList(GameObject foodToRemove)
    {
        print(foodToRemove);
        if (foodOnFloorList.Contains(foodToRemove))
        {
            foodOnFloorList.Remove(foodToRemove);

            foodOnFloor = foodOnFloorList.Count > 0 ? true : false;

        }

        foodOnFloorList.RemoveAll(n => n == null || !n);
    }


}
