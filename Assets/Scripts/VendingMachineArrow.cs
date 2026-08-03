using UnityEngine;

public class VendingMachineArrow : Interactable
{
    public int incrementAmount = 1;

    public CraftingTableScript vendingMachine;

    public override void Interact(PlayerHandScript playerHand)
    {
        IncrementByAmount();
    }

    public void IncrementByAmount()
    {
        vendingMachine.CycleThroughRecipeList(incrementAmount);
    }
}
