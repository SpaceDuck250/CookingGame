using UnityEngine;

public class TalkCustomerTask : TutorialTask
{
    public CustomerInteractScript customerInteract;

    private void Start()
    {
        customerInteract.OnInteractWithCustomer += OnInteract;
    }

    private void OnDestroy()
    {
        customerInteract.OnInteractWithCustomer -= OnInteract;
    }

    public void OnInteract()
    {
        CompleteTask();
    }
}
