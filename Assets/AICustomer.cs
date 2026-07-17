using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public class AICustomer : Interactable
{
    NavMeshAgent customerAgent;
    public Transform customerReady;
    public Transform customerLeave;
    public GameObject dialogueText;
    public TMP_Text textDialogue;
    public Transform player;
    public Transform heldObject;
    public Transform thiefHeldObject;
    bool readyToSteal;
    bool ordered = false;
    public GameObject money;
    //bool caught = true;
    Animator customerAnimator;
    float distance;
    AudioSource audioSource;
    public string[] dialogueLines =
{
    "I like to be late",
    "I like to code"
};
    void Start()
    {
        customerAgent = GetComponent<NavMeshAgent>();
        customerAgent.SetDestination(customerReady.position);
        customerAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Debug.Log(customerAgent.SetDestination(customerReady.position));
        Debug.Log(customerAgent.isOnNavMesh);
    }

    // Update is called once per frame
    void Update()
    {
        //distance = Vector3.Distance(player.position, transform.position);
        //if(distance > 5)
        //{
          //  dialogueText.SetActive(false);
           // textDialogue.text = "";
        //}
         if(!customerAgent.pathPending &&
        customerAgent.remainingDistance <= customerAgent.stoppingDistance && (!customerAgent.hasPath || customerAgent.velocity.sqrMagnitude <= 0.5f))
        {
            customerAgent.ResetPath();
            customerAgent.isStopped = true;
            customerAnimator.SetBool("Idle", true);
        }
    }
    void Leave()
    {
        if(thiefHeldObject.Find("Takeaway Box(Clone)") != null)
        {
            thiefHeldObject.Find("Takeaway Box(Clone)").localPosition = Vector3.zero;
        }
        customerAnimator.SetBool("Idle", false);
        //if(!caught)
        //{
            customerAnimator.SetBool("Collected", true);
            customerAgent.SetDestination(customerLeave.position);
        //}
        //else
        //{
         //   thiefAnimator.SetBool("Caught", true);
        //}
        customerAgent.isStopped = false;
        audioSource.enabled = true;
    }
    public override void Interact(PlayerHandScript playerHand)
    {
        Debug.Log("HOLE IN ONE");
        if (heldObject.Find("Takeaway Box(Clone)") != null && distance <= 5 && !customerAgent.pathPending && customerAgent.remainingDistance <= customerAgent.stoppingDistance && (!customerAgent.hasPath || customerAgent.velocity.sqrMagnitude <= 0.5f))
        {
            dialogueText.SetActive(true);
            textDialogue.text = "Thanks you for the food!";
            
            heldObject.Find("Takeaway Box(Clone)").SetParent(thiefHeldObject);
            money.SetActive(true);
            playerHand.currentFoodHeld = null;
            playerHand.currentFoodHeldObj = null;
            Leave();
        }
        else if (heldObject.Find("Chicken Rice(Clone)") != null && distance <= 5 && !customerAgent.pathPending && customerAgent.remainingDistance <= customerAgent.stoppingDistance && (!customerAgent.hasPath || customerAgent.velocity.sqrMagnitude <= 0.5f))
        {
            dialogueText.SetActive(true);
            textDialogue.text = "Excuse me ah, I ask for dabao eh!";
            StopAllCoroutines();
            StartCoroutine(Dialogue()); 
        }
        else if(!customerAgent.pathPending && customerAgent.remainingDistance <= customerAgent.stoppingDistance && (!customerAgent.hasPath || customerAgent.velocity.sqrMagnitude <= 0.5f))
        {
            //if(!caught)
            //{
            dialogueText.SetActive(true);
            if(ordered)
            {
                textDialogue.text = dialogueLines[Random.Range(0, dialogueLines.Length)];
            }
            else
            {
                textDialogue.text = "I'll like to Dabao chicken rice";
                ordered = true;
            }
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
    IEnumerator Dialogue()
    {
        yield return new WaitForSeconds(2);
        dialogueText.SetActive(false);
    }
}
