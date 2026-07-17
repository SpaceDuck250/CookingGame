using UnityEngine;

public class ButtonScript : Interactable
{
    public CageOfDeath cageOfDeath;
    public override void Interact(PlayerHandScript playerHand)
    {
        print("dog");
        cageOfDeath.enabled = false;
    }
}
