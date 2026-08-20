using UnityEngine;

public class CustomerTutorialListener : MonoBehaviour
{
    public GameObject secondWall;

    private void Start()
    {
        CustomerInteractScript.OnAnyCustomerInteract += OnAnyInteract;
    }

    private void OnDestroy()
    {
        CustomerInteractScript.OnAnyCustomerInteract -= OnAnyInteract;

    }

    public void OnAnyInteract(CustomerStateMachine csm)
    {
        secondWall.SetActive(false);
    }
}
