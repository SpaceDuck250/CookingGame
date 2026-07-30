using UnityEngine;
using System.Collections.Generic;
using System;

public class FrierInteractScript : Interactable
{
    public class HeatLevel
    {
        public string name;
        public Color displayColor;
        public float speedIncreaser;
    }

    public List<HeatLevel> heatLevelList = new List<HeatLevel>();
    private int tempIndex = 0;
    public HeatLevel currentHeatLevel;

    public Action<HeatLevel> OnChangeHeatLevel;

    public Action<GameObject> OnFry;
    public Action OnFryEnd;

    public CookingInputOutputScript inputOutputScript;

    public GameObject foodHeld;
    public GameObject pan;
    public Transform spawn;
    public Vector3 spawnOffset;

    public bool cooking = false;

    public float downScaleAmount = 1;

    private void Start()
    {
        SetupHeatLevels();

        inputOutputScript.OnCookingStart += OnCookingGameStart;
    }

    private void OnDestroy()
    {
        inputOutputScript.OnCookingStart -= OnCookingGameStart;
    }

    private void SetupHeatLevels()
    {
        heatLevelList.Add(new HeatLevel { name = "Off", displayColor = Color.white, speedIncreaser = 0 });
        heatLevelList.Add(new HeatLevel { name = "Warm", displayColor = Color.orange, speedIncreaser = 1 });
        heatLevelList.Add(new HeatLevel { name = "Hot", displayColor = Color.red, speedIncreaser = 1.5f });
        heatLevelList.Add(new HeatLevel { name = "Blazing", displayColor = Color.purple, speedIncreaser = 2.5f });

        currentHeatLevel = heatLevelList[0];
    }

    private void OnCookingGameStart(FoodData food)
    {
        foodHeld = CookingInputOutputScript.SpawnDisplayFoodInPosition(food, spawn, spawnOffset, false, true, downScaleAmount);
        CheckIfCooking();
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeldObj != null)
        {
            return;
        }

        ChangeTemp();
    }

    public void CheckIfCooking()
    {
        if (currentHeatLevel.name == "Off")
        {
            cooking = false;
            OnFryEnd?.Invoke();
        }
        else
        {
            cooking = true;

            OnFry?.Invoke(foodHeld);
            
        }
    }

    public void ChangeTemp()
    {
        tempIndex++;
        if (tempIndex > heatLevelList.Count - 1)
        {
            tempIndex = 0;
        }

        currentHeatLevel = heatLevelList[tempIndex];
        OnChangeHeatLevel?.Invoke(currentHeatLevel);

        CheckIfCooking();
    }
}
