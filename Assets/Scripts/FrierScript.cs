using UnityEngine;
using System.Collections.Generic;
using System;

public class FrierScript : Interactable
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

    public CookingInputOutputScript inputOutputScript;

    public GameObject foodHeld;
    public GameObject pan;
    public Transform spawn;
    public Vector3 spawnOffset;

    public bool cooking = false;

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
        heatLevelList.Add(new HeatLevel { name = "Off", displayColor = Color.gray, speedIncreaser = 0 });
        heatLevelList.Add(new HeatLevel { name = "Warm", displayColor = Color.orange, speedIncreaser = 1 });
        heatLevelList.Add(new HeatLevel { name = "Hot", displayColor = Color.red, speedIncreaser = 1.5f });
        heatLevelList.Add(new HeatLevel { name = "Blazing", displayColor = Color.purple, speedIncreaser = 2.5f });

        currentHeatLevel = heatLevelList[0];
    }

    private void OnCookingGameStart(FoodData food)
    {
        CheckIfCooking();

        foodHeld = CookingInputOutputScript.SpawnDisplayFoodInPosition(food, spawn, spawnOffset, false);
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        ChangeTemp();
    }

    public void CheckIfCooking()
    {
        if (currentHeatLevel.name == "Off")
        {
            cooking = false;
        }
        else
        {
            cooking = true;
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
