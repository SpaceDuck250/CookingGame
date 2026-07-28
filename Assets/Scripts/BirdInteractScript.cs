using UnityEngine;

public class BirdInteractScript : Interactable
{
    public BirdState idleState;
    public BirdFlyIntoTheSunsetState SearchState;
    public BirdAIManager manager;

    public override void Interact(PlayerHandScript playerHand)
    {
        if (manager.currentState != idleState)
        {
            return;
        }

        if (playerHand.currentFoodHeld != null && SearchState.searchFood == null)
        {
            manager.TransitionToNewState(SearchState);

            SearchState.CreateSearchItem(playerHand.currentFoodHeld);
            playerHand.ClearFoodFromHand();
        }

    }
}
