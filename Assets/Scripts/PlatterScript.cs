using UnityEngine;

public class PlatterScript : Interactable
{
    public FoodData[] foodHeldArray = new FoodData[4];

    public Transform[] placeAreasArray = new Transform[4];

    public int currentIndex = 0;

    public Vector3 upOffset;

    public override void Interact(PlayerHandScript playerHand)
    {
        if (!CheckIfPlayerIsHoldingFood(playerHand))
        {
            return;
        }

        TryPlaceFoodOnPlaceArea(playerHand.currentFoodHeld);

    }

    public bool CheckIfPlayerIsHoldingFood(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeld == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void TryPlaceFoodOnPlaceArea(FoodData foodData)
    {
        if (currentIndex >= 4)
        {
            return;
        }

        Transform placeParent = placeAreasArray[currentIndex];

        CookingInputOutputScript.SpawnDisplayFoodInPosition(foodData, placeParent, upOffset);

        currentIndex++;
    }
}
