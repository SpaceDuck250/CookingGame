using UnityEngine;

public class CatInteractScript : Interactable
{
    public CatAIScript manager;

    public override void Interact(PlayerHandScript playerHand)
    {
        if (playerHand.currentFoodHeld == null)
        {
            return;
        }

        bool wasFed = manager.TryFeedFish(playerHand.currentFoodHeld);

        if (wasFed)
        {
            playerHand.ClearFoodFromHand();
        }
    }
}