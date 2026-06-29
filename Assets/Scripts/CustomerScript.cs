using UnityEngine;

public class CustomerScript : Interactable
{
    // Put whatever you want inside the interact function. The interact function will be called when the interactKeyCode is pressed (in this case it is letter E but you can change it)
    // For example you can make it show on screen what their order is

    public override void Interact(PlayerHandScript playerHand)
    {
        print("Do something");
    }
}
