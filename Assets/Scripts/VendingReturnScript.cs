using UnityEngine;

public class VendingReturnScript : Interactable
{
    public CraftingTableScript vendingMachine;

    public override void Interact(PlayerHandScript playerHand)
    {
        if (vendingMachine.busyReturning)
        {
            return;
        }

        StartCoroutine(vendingMachine.ReturnAllInputFoodBack());
    }
}
