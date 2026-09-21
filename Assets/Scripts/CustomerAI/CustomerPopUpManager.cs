using UnityEngine;
using System;
using System.Collections.Generic;

public class CustomerPopUpManager : MonoBehaviour
{
    public GameObject popUpParent;

    public GameObject popUpTemplate;

    public static Action<CustomerStateMachine> OnPopUpFinished;

    public List<CustomerStateMachine> popUpsActiveList = new List<CustomerStateMachine>();

    public bool hidden = true;

    private void Start()
    {
        CustomerInteractScript.OnAnyCustomerInteract += TryAddPopUp;
        CustomerInteractScript.OnEndInteractWithCustomer += ShowPopUp;

        OnPopUpFinished += TakeOutPopUp;
        
    }

    private void OnDestroy()
    {
        CustomerInteractScript.OnAnyCustomerInteract -= TryAddPopUp;
        CustomerInteractScript.OnEndInteractWithCustomer -= ShowPopUp;

        OnPopUpFinished -= TakeOutPopUp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!hidden)
            {
                HidePopUp();
            }
            else
            {
                ShowPopUp();
            }
        }
    }

    private void HidePopUp()
    {
        popUpParent.SetActive(false);
        hidden = true;
    }

    public void TryAddPopUp(CustomerStateMachine customer)
    {
        HidePopUp();

        if (popUpsActiveList.Contains(customer))
        {
            return;
        }

        popUpsActiveList.Add(customer);

        GameObject newPopUp = Instantiate(popUpTemplate, popUpParent.transform);
        PopUpTemplateScript popUpScript = newPopUp.GetComponent<PopUpTemplateScript>();
        popUpScript.SetupTemplate(customer);
    }

    public void ShowPopUp()
    {
        popUpParent.SetActive(true);
        hidden = false;
    }

    public void TakeOutPopUp(CustomerStateMachine customer)
    {
        popUpsActiveList.Remove(customer);
    }
}
