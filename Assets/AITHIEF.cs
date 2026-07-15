using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public class AITHIEF : Interactable
{
    NavMeshAgent thiefAgent;
    public Transform thiefReady;
    public Transform thiefEscape;
    public GameObject dialogueText;
    public TMP_Text textDialogue;
    public Transform player;
    public Transform heldObject;
    public Transform thiefHeldObject;
    bool readyToSteal;
    //bool caught = true;
    Animator thiefAnimator;
    float distance;
    AudioSource audioSource;
    void Start()
    {
        thiefAgent = GetComponent<NavMeshAgent>();
        thiefAgent.SetDestination(thiefReady.position);
        thiefAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Debug.Log(thiefAgent.SetDestination(thiefReady.position));
        Debug.Log(thiefAgent.isOnNavMesh);
    }

    // Update is called once per frame
    void Update()
    {
        //distance = Vector3.Distance(player.position, transform.position);
        //if(distance > 5)
        //{
         //   dialogueText.SetActive(false);
        //    textDialogue.text = "";
       // }
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
        if(thiefHeldObject.Find("Takeaway Box(Clone)") != null)
        {
            thiefHeldObject.Find("Takeaway Box(Clone)").localPosition = Vector3.zero;
        }
        thiefAnimator.SetBool("Idle", false);
        //if(!caught)
        //{
            thiefAnimator.SetBool("Collected", true);
            thiefAgent.SetDestination(thiefEscape.position);
        //}
        //else
        //{
         //   thiefAnimator.SetBool("Caught", true);
        //}
        thiefAgent.isStopped = false;
        StartCoroutine(Goofy());
    }
    public override void Interact(PlayerHandScript playerHand)
    {
        Debug.Log("HOLE IN ONE");
        if (heldObject.Find("Takeaway Box(Clone)") != null && distance <= 5 && !thiefAgent.pathPending && thiefAgent.remainingDistance <= thiefAgent.stoppingDistance && (!thiefAgent.hasPath || thiefAgent.velocity.sqrMagnitude <= 0.5f))
        {
            dialogueText.SetActive(true);
            textDialogue.text = "Thanks for the free food sucker!";
            StopAllCoroutines();
            StartCoroutine(Dialogue()); 
            heldObject.Find("Takeaway Box(Clone)").SetParent(thiefHeldObject);
            playerHand.currentFoodHeld = null;
            playerHand.currentFoodHeldObj = null;
            Escape();
        }
        else if (heldObject.Find("Chicken Rice(Clone)") != null && distance <= 5 && !thiefAgent.pathPending && thiefAgent.remainingDistance <= thiefAgent.stoppingDistance && (!thiefAgent.hasPath || thiefAgent.velocity.sqrMagnitude <= 0.5f))
        {
            dialogueText.SetActive(true);
            textDialogue.text = "I asked for Dabao, take your time yea?";
            StopAllCoroutines();
            StartCoroutine(Dialogue()); 
        }
        else if(!thiefAgent.pathPending && thiefAgent.remainingDistance <= thiefAgent.stoppingDistance && (!thiefAgent.hasPath || thiefAgent.velocity.sqrMagnitude <= 0.5f))
        {
        
            //if(!caught)
            //{
            dialogueText.SetActive(true);
            textDialogue.text = "I'll like to Dabao chicken rice please";
            StopAllCoroutines();
            StartCoroutine(Dialogue());     
           // }
            //else
            //{
            //    Escape();
            //}
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.name);
        if(other.gameObject.CompareTag("Escape"))
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator Goofy()
    {
        audioSource.enabled = true;
        yield return new WaitForSeconds(2);
        audioSource.enabled = false;
    }
    IEnumerator Dialogue()
    {
        yield return new WaitForSeconds(2);
        dialogueText.SetActive(false);
    }
}
