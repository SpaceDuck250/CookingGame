using UnityEngine;
using System;

public class InteractAreaScript : MonoBehaviour
{
    public bool withinRange = false;

    public Interactable interactable;

    public event Action<GameObject> OnPlayerEnterRange;
    public event Action OnPlayerExitRange;

    private void Update()
    {
        if (!withinRange)
        {
            return;
        }

        PlayerHandScript playerHand = PlayerHandScript.instance;
        interactable.CheckInput(playerHand);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        withinRange = true;

        OnPlayerEnterRange?.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        withinRange = false;

        OnPlayerExitRange?.Invoke();
    }
}
