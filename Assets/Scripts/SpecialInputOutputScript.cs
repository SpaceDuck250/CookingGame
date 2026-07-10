//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class SpecialInputOutputScript : Interactable, ICookStation
//{
//    public event Action<FoodData> OnCookingStart;
//    public event Action<Vector3, GameObject, Transform> OnCookingSuccess;
//    public event Action<Vector3, GameObject, Transform> OnCookingFail;

//    public event Action OnFoodTakenOutOfCookingStation;

//    public Action OnStartCheckingIfValidRecipe;

//    public bool takesMultipleInputFoods;
//    public List<FoodData> inputFoodList = new List<FoodData>();
//    public List<SpecialRecipe> specialRecipeStored = new List<SpecialRecipe>();

//    public SpecialRecipe currentRecipeUsed;

//    private void Start()
//    {
//        OnStartCheckingIfValidRecipe += TryStartCooking;
//    }

//    private void OnDestroy()
//    {
//        OnStartCheckingIfValidRecipe -= TryStartCooking;

//    }

//    public override void Interact(PlayerHandScript playerHand)
//    {
//        AddFoodToCheckList(playerHand.currentFoodHeld);
//    }

//    public void AddFoodToCheckList(FoodData foodHeld)
//    {
//        inputFoodList.Add(foodHeld);
//    }

//    public void TryStartCooking()
//    {
//        if (!CheckIfValidRecipe())
//        {
//            return;
//        }

//        OnCookingStart?.Invoke(currentRecipeUsed.foodGettingCooked);
//    }

//    public bool CheckIfValidRecipe()
//    {
//        if (inputFoodList.Count == 0)
//        {
//            return false;
//        }

//        foreach (SpecialRecipe recipe in specialRecipeStored)
//        {
//            //List<FoodData> recipeList = new List<FoodData>(recipe.inputFoodList);
//            //List<FoodData> inputList = new List<FoodData>(inputFoodList);

//            bool identical = recipe.inputFoodList.OrderBy(n => n.foodName).SequenceEqual(inputFoodList.OrderBy(n => n.foodName));
//            if (identical)
//            {
//                currentRecipeUsed = recipe;
//                return true;
//            }
//        }

//        return false;
//    }

//    public void CallFoodSuccessEvent(Vector3 spawnPos, GameObject displayObj, Transform parent)
//    {
//        OnCookingSuccess?.Invoke(spawnPos, displayObj, parent);
//    }

//    public void CallFoodFailEvent(Vector3 spawnPos, GameObject displayObj, Transform parent)
//    {
//        OnCookingFail?.Invoke(spawnPos, displayObj, parent);
//    }

//    public void CallFoodTakenOutEvent()
//    {
//        OnFoodTakenOutOfCookingStation.Invoke();
//    }
//}
