using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public KeyCode interactKeyCode;

    public virtual void CheckInput(PlayerHandScript playerHand)
    {
        if (Input.GetKeyDown(interactKeyCode))
        {
            Interact(playerHand);
        }
    }

    public abstract void Interact(PlayerHandScript playerHand);

}
