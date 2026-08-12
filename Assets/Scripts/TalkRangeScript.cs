using UnityEngine;
using System;

public class TalkRangeScript : MonoBehaviour
{
    public CustomerInteractScript interactScript;

    public float interactRange;

    public bool interacting = false;

    public Action OnExitTalkRange;
    public Action OnEnterTalkRange;

    public bool inRange = false;

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
        //if (!interacting)
        //{
        //    return;
        //}

        CheckIfExitRange();
    }

    public void CheckIfExitRange()
    {
        if (PlayerHandScript.instance == null)
        {
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerHandScript.instance.gameObject.transform.position);

        if (distanceToPlayer > interactRange)
        {
            inRange = false;
            OnExitTalkRange?.Invoke();

            if (interacting)
            {
                CustomerExitRange();
                return;
            }
        }
        else
        {
            inRange = true;
            OnEnterTalkRange?.Invoke();

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
