using UnityEngine;

public class RatAnimator : MonoBehaviour
{
    public RatAIScript ratAi;
    public bool playingAnimation = false;

    public Animator animator;

    private void Start()
    {
        ratAi.OnMouseEating += PlayRatEatingAnim;
        ratAi.OnMouseStopEating += StopRatEatingAnim;
    }

    private void OnDestroy()
    {
        ratAi.OnMouseEating -= PlayRatEatingAnim;
        ratAi.OnMouseStopEating -= StopRatEatingAnim;
    }

    private void StopRatEatingAnim(bool value)
    {
        playingAnimation = false;
        animator.SetBool("Eating", false);
    }

    private void PlayRatEatingAnim(GameObject foodObj)
    {
        if (playingAnimation)
        {
            return;
        }

        playingAnimation = true;

        animator.SetBool("Eating", true);
    }


}
