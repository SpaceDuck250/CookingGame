using UnityEngine;
using Cat;

public class CatAnimator : MonoBehaviour
{
    public Animator animator;
    public CatAIScript catAi;

    public float moveSpeedThreshold = 0.1f;

    private void Awake()
    {
        catAi.OnCatChangeState += UpdateAnimationForState;
    }

    private void OnDestroy()
    {
        catAi.OnCatChangeState -= UpdateAnimationForState;
    }

    private void Update()
    {
        if (catAi.currentState == CatState.Waiting)
        {
            bool isMoving = catAi.agent.velocity.magnitude > moveSpeedThreshold;
            animator.SetBool("Walking", isMoving);
            animator.SetBool("Sitting", !isMoving);
        }
    }

    private void UpdateAnimationForState(CatState newState)
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Sitting", false);
        animator.SetBool("Eating", false);

        switch (newState)
        {
            case CatState.WalkingToStall:
            case CatState.Leaving:
                animator.SetBool("Walking", true);
                break;

            case CatState.Waiting:
                animator.SetBool("Sitting", true); 
                break;

            case CatState.Eating:
                animator.SetBool("Eating", true);
                break;
        }
    }
}