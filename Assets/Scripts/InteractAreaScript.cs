using UnityEngine;
using System;

// This script detects if the player is in range and triggers interact input
public class InteractAreaScript : MonoBehaviour
{
    public bool withinRange = false;

    public Interactable interactable;

    public event Action<GameObject> OnPlayerEnterRange;
    public event Action OnPlayerExitRange;

    public bool active = true;

    // Checks if the player is in range. If the player is in range then check if the player has clicked the appropriate input key (Example for customers E or for machines T)
    // if the input key is clicked then it will call the interact function
    private void Update()
    {
        if (!active)
        {
            return;
        }

        if (!withinRange)
        {
            return;
        }

        PlayerHandScript playerHand = PlayerHandScript.instance;
        interactable.CheckInput(playerHand);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
        {
            return;
        }

        if (other.gameObject.tag == "Player")
        {
            withinRange = true;
            OnPlayerEnterRange?.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!active)
        {
            return;
        }

        if (other.gameObject.tag == "Player")
        {
            withinRange = false;
            OnPlayerExitRange?.Invoke();
        }
    }

    public void HideDisplay()
    {
        active = false;
        OnPlayerExitRange?.Invoke();

    }
}
