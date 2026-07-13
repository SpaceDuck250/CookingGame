using UnityEngine;

public class ArrowPointerScript : MonoBehaviour
{
    public CustomerInteractScript interactScript;

    public GameObject pointerObj;

    private void Start()
    {
        interactScript.OnInteractWithCustomer += ShowPointer;
        CustomerInteractScript.OnEndInteractWithCustomer += HidePointer;
    }

    private void OnDestroy()
    {
        interactScript.OnInteractWithCustomer -= ShowPointer;
        CustomerInteractScript.OnEndInteractWithCustomer -= HidePointer;
    }

    public void ShowPointer()
    {
        pointerObj.SetActive(true);
    }

    public void HidePointer()
    {
        pointerObj.SetActive(false);
    }
}
