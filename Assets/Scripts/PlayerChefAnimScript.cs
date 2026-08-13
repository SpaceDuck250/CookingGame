using UnityEngine;

public class PlayerChefAnimScript : MonoBehaviour
{
    public PlayerMovement playerMoveScript;

    public Animator chefAnimator;

    public void PlayAnim()
    {
        chefAnimator.SetTrigger("PlayWalkAnim");
    }

    public void DoChefWalkAnim()
    {
        chefAnimator.SetBool("Walking", true);
    }
}
