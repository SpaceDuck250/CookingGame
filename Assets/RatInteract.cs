using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class RatInteract : Interactable
{
    public GameObject broom;
    RatAIScript ratAI;
    RatAnimator ratAnimator;
    Animator animator;
    NavMeshAgent agent;
    HoldableFoodScript holdableFoodScript;
    void Update()
    {
        broom = GameObject.Find("Broom");
        ratAI = GetComponent<RatAIScript>();
        ratAnimator = GetComponent<RatAnimator>();
        agent = GetComponent<NavMeshAgent>();
        holdableFoodScript = GetComponent<HoldableFoodScript>();
        animator = GetComponent<Animator>();
    }
    public override void Interact(PlayerHandScript playerHand)
    {

        print(" rat");
        if(playerHand.currentFoodHeldObj == broom)
        {
            this.transform.rotation = Quaternion.Euler(0,0,180);
            ratAI.enabled = false;
            StartCoroutine(DyingRat());
        }
    }
    IEnumerator DyingRat()
    {
        yield return new WaitForSeconds(10);
        ratAnimator.enabled = false;
        animator.enabled = false;
        this.enabled = false;
        agent.enabled = false;
        holdableFoodScript.enabled = true;
        this.gameObject.layer = LayerMask.NameToLayer("Food");
    }
}
