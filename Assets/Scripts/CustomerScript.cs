using UnityEngine;

public class CustomerScript : Interactable
{
    // Put whatever you want inside the interact function. The interact function will be called when the interactKeyCode is pressed (in this case it is letter E but you can change it)
    // For example you can make it show on screen what their order is

    public CustomerAIScript customerAI;

    // When spawn, check if the customerAI is null, if it is null then get the component CustomerAIScript
    private void Start()
    {
        if (customerAI == null)
        {
            customerAI = GetComponentInParent<CustomerAIScript>();
        }
    }

    public override void Interact(PlayerHandScript playerHand)
    {
        print("Customer leaving");

        // Call the FindExit function from the CustomerAIScript
        customerAI.FindExit();
    }
}
