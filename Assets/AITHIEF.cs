using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public class AITHIEF : Interactable
{
    NavMeshAgent thiefAgent;
    public Transform thiefReady;
    public GameObject dialogueText;
    public TMP_Text textDialogue;
    public Transform player;
    bool readyToSteal;
    Animator thiefAnimator;
    void Start()
    {
        thiefAgent = GetComponent<NavMeshAgent>();
        thiefAgent.SetDestination(thiefReady.position);
        thiefAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if(distance > 5)
        {
            dialogueText.SetActive(false);
            textDialogue.text = "";
        }
         if(!thiefAgent.pathPending &&
        thiefAgent.remainingDistance <= thiefAgent.stoppingDistance && (!thiefAgent.hasPath || thiefAgent.velocity.sqrMagnitude <= 0.5f))
        {
            thiefAgent.ResetPath();
            thiefAgent.isStopped = true;
            thiefAnimator.SetBool("Idle", true);
        }
    }
    void Escape()
    {
        
    }
    public override void Interact(PlayerHandScript playerHand)
    {
        Debug.Log("HOLE IN ONE");
        if(!thiefAgent.pathPending &&
        thiefAgent.remainingDistance <= thiefAgent.stoppingDistance && (!thiefAgent.hasPath || thiefAgent.velocity.sqrMagnitude <= 0.5f))
        {
            dialogueText.SetActive(true);
            textDialogue.text = "I'll like your mom please";
        }
    }
}
