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

        if (!moveScript.holdingTray)
        {
            animator.SetBool("walking", true);
            animator.SetBool("traywalk", false);

        }
        else
        {
            animator.SetBool("traywalk", true);
            animator.SetBool("walking", false);
        }
    }

    public void Sit()
    {
        animator.SetBool("walking", false);
        animator.SetBool("traywalk", false);

        animator.SetBool("sitting", true);
    }

    public void Idle()
    {
        animator.SetBool("walking", false);
        animator.SetBool("sitting", false);
    }
}
