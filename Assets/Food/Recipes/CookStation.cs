using System;
using UnityEngine;

// Only for the simple recipe station and the special recipe station dont use elsewhere
public interface ICookStation 
{
    public event Action<FoodData> OnCookingStart;
    public event Action<Vector3, GameObject, Transform> OnCookingSuccess;
    public event Action<Vector3, GameObject, Transform> OnCookingFail;

    public event Action OnFoodTakenOutOfCookingStation;

    public void CallFoodSuccessEvent(Vector3 spawnPos, GameObject displayObj, Transform parent);

    public void CallFoodFailEvent(Vector3 spawnPos, GameObject displayObj, Transform parent);

    public void CallFoodTakenOutEvent();





}
