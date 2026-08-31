using UnityEngine;
using Customer;

public class CustomerSoundManager : MonoBehaviour
{
    public CustomerStateMachine stateMachine;
    public SFXBank customerBank;

    private void Start()
    {
        stateMachine.OnCustomerMoodChange += HandleMoodChanged;
    }

    private void OnDestroy()
    {
        stateMachine.OnCustomerMoodChange -= HandleMoodChanged;
    }

    private void HandleMoodChanged(CustomerMood newMood)
    {
        if (newMood == CustomerMood.Angry)
        {
            GeneralSoundManager.instance.PlaySoundEffect(customerBank, "angry", transform.position);
        }
    }
}