using UnityEngine;

public class TrashCanScript : Interactable
{
    public override void Interact(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeld == null)
        {
            return;
        }

        playerHand.ClearFoodFromHand();
    }
}
