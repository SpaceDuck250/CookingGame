using UnityEngine;
using System;

// This script it mainly to detect if the player is in range and then if so check inputs
public class InteractAreaScript : MonoBehaviour
{
    public bool withinRange = false;

    public Interactable interactable;

    public event Action<GameObject> OnPlayerEnterRange;
    public event Action OnPlayerExitRange;

    // Checks if the player is in range. If the player is in range then check if the player has clicked the appropriate input key (Example for customers E or for machines T)
    // if the input key is clicked then it will call the interact function
    private void Update()
    {
        if (!withinRange)
        {
            return;
        }

        PlayerHandScript playerHand = PlayerHandScript.instance;
        interactable.CheckInput(playerHand);
    }

    // Check if the player entered interactable range
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            withinRange = true;

            OnPlayerEnterRange?.Invoke(other.gameObject);
            
        }

        
    }

    // Check if player left interactable range
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            withinRange = false;

            OnPlayerExitRange?.Invoke();
        }

        
    }
}
