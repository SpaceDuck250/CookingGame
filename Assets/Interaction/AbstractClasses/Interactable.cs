using UnityEngine;

// Abstract function because there will be many things that interactable like different machines for customers for example
public abstract class Interactable : MonoBehaviour
{
    // The keycode that will be checked
    public KeyCode interactKeyCode;

    // Checks if keycode is pressed and executes the interact function
    public virtual void CheckInput(PlayerHandScript playerHand)
    {
        if (Input.GetKeyDown(interactKeyCode))
        {
            Interact(playerHand);
        }
    }

    // Interact function is abstract which means it can be different for different scripts.
    public abstract void Interact(PlayerHandScript playerHand);

}
