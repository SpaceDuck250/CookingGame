using UnityEngine;
using System;

public class TalkRangeScript : MonoBehaviour
{
    public CustomerInteractScript interactScript;

    public float interactRange;

    public bool interacting = false;

    private void Start()
    {
        interactScript.OnInteractWithCustomer += Interact;
        CustomerInteractScript.OnEndInteractWithCustomer += EndInteract;
    }

    private void OnDestroy()
    {
        interactScript.OnInteractWithCustomer -= Interact;
        CustomerInteractScript.OnEndInteractWithCustomer -= EndInteract;
    }


    private void Update()
    {
        if (!interacting)
        {
            return;
        }

        CheckIfExitRange();
    }

    public void CheckIfExitRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerHandScript.instance.gameObject.transform.position);

        if (distanceToPlayer > interactRange)
        {
            CustomerExitRange();
        }
    }

    public void CustomerExitRange()
    {
        interacting = false;
        interactScript.CloseConversation();
    }

    public void Interact()
    {
        interacting = true;
    }

    public void EndInteract()
    {
        interacting = false;
    }
}
