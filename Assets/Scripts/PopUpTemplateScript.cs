using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Customer;

public class PopUpTemplateScript : MonoBehaviour
{
    CustomerStateMachine stateMachine;

    public TextMeshProUGUI customerNameText;
    public Image customerImage;
    public Image patienceImage;

    public Color displayColor;

    public CustomerData profile;

    public bool runTimer = false;

    public float timer;

    float greenValue = 0.5f;
    float yellowValue = 0.65f;
    float orangeValue = 0.8f;
    float redValue = 1f;

    public void SetupTemplate(CustomerStateMachine customer)
    {
        stateMachine = customer;
        profile = customer.profile;

        customerNameText.text = profile.customerName;
        customerImage.sprite = profile.customerSprite;

        timer = 1;
        runTimer = true;

        stateMachine.OnCustomerChangeState += DestroySelf;
    }

    private void OnDestroy()
    {
        stateMachine.OnCustomerChangeState -= DestroySelf;

    }

    private void Update()
    {
        if (!runTimer)
        {
            return;
        }

        // Because waittimer starts at 0 and goes up
        timer = 1 - (stateMachine.waitTimer/(stateMachine.profile.waitTimeUntilReallyAngry));
        patienceImage.fillAmount = timer;

        TryChangeColor();
    }

    private void TryChangeColor()
    {
        float totalWaitTime = stateMachine.profile.waitTimeUntilReallyAngry;

        if (stateMachine.waitTimer <= totalWaitTime * greenValue)
        {
            displayColor = Color.green;
        }
        else if (stateMachine.waitTimer <= totalWaitTime * yellowValue)
        {
            displayColor = Color.yellow;
        }
        else if (stateMachine.waitTimer <= totalWaitTime * orangeValue)
        {
            displayColor = Color.orange;
        }
        else if (stateMachine.waitTimer <= totalWaitTime * redValue)
        {
            displayColor = Color.red;
        }

        patienceImage.color = displayColor;
    }

    public void DestroySelf(CustomerState state)
    {
        if (state == CustomerState.LeavingMap || state == CustomerState.WalkingToSeat)
        {
            CustomerPopUpManager.OnPopUpFinished?.Invoke(stateMachine);
            Destroy(gameObject);
        }
    }
}
