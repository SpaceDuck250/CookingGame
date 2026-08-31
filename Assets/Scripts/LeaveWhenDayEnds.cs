using Customer;
//using UnityEditorInternal;
using UnityEngine;

public class LeaveWhenDayEnds : MonoBehaviour
{
    public CustomerStateMachine stateMachine;

    private void Start()
    {
        DaySystemManager.OnDayEnd += OnDayEnd;
    }

    private void OnDestroy()
    {
        DaySystemManager.OnDayEnd += OnDayEnd;

    }

    public void OnDayEnd(PlayerDailyStats playerDailyStats)
    {
        if (stateMachine.currentState == CustomerState.WalkingToCounter || stateMachine.currentState == CustomerState.IdleAtCounter || stateMachine.currentState == CustomerState.WaitingForFood)
        {
            Leave();
        }
    }

    public void Leave()
    {
        stateMachine.OnCustomerChangeState?.Invoke(CustomerState.LeavingMap);
        CustomerSpawnerScript.instance.OnCustomerOrderFinish(stateMachine);
    }
}
