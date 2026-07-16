using UnityEngine;
using Customer;

public class LeaveWhenAngryScript : MonoBehaviour
{
    public CustomerStateMachine stateMachine;

    private void Start()
    {
        stateMachine.OnCustomerMoodChange += OnCustomerMoodChange;
    }

    private void OnDestroy()
    {
        stateMachine.OnCustomerMoodChange -= OnCustomerMoodChange;
    }

    public void OnCustomerMoodChange(CustomerMood mood)
    {
        if (mood == CustomerMood.ReallyAngry)
        {
            stateMachine.OnCustomerChangeState?.Invoke(CustomerState.LeavingMap);
            CustomerSpawnerScript.instance.OnCustomerOrderFinish(stateMachine);
        }
    }
}
