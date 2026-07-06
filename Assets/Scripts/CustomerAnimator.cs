using UnityEngine;

public class CustomerAnimator : MonoBehaviour
{
    public Animator animator;
    public CustomerMovementScript moveScript;

    private void Start()
    {
        moveScript.OnCustomerMove += Walk;
        moveScript.OnCustomerIdle += Idle;
    }

    private void OnDestroy()
    {
        moveScript.OnCustomerMove -= Walk;
        moveScript.OnCustomerIdle -= Idle;
    }

    public void Walk()
    {
        animator.SetBool("sitting", false);
        animator.SetBool("walking", true);
    }

    public void Sit()
    {
        animator.SetBool("walking", false);
        animator.SetBool("sitting", true);
    }

    public void Idle()
    {
        animator.SetBool("walking", false);
        animator.SetBool("sitting", false);
    }
}
