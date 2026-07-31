using UnityEngine;

public class KnifeAnimatorScript : MonoBehaviour
{
    public Animator animator;

    public GameObject knifeObj;

    public void PlayKnifeChop()
    {
        knifeObj.SetActive(true);
        animator.SetTrigger("Chop");
    }

    public void FinishChop()
    {
        knifeObj.SetActive(false);
    }
}
