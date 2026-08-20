using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialChefScript : Interactable
{
    public string teleportSceneName;

    public SlowTyper slowTyper;

    public List<string> dialogueLines = new List<string>();
    public int currentIndex = -1;

    public List<ExplainationObj> explainationLines = new List<ExplainationObj>();
    public int currentExlainIndex = -1;

    public string nameOfChef = "";

    public bool goingToStall = false;

    public bool startTalkFinish = false;
    public bool reachedStall = false;

    public bool doingServingQuest = false;

    public List<Transform> movePoints = new List<Transform>();

    public int currentDestinationIndex;

    public Transform chefTransform;

    public float smoothValue;

    public Animator chefAnimator;

    public Camera playerCam;

    public TutorialManagerScript tutorialManager;

    public PlayerMovement playerMove;
    public TurnScript playerTurn;
    public Image slideShowImage;

    public GameObject customerSpawner;

    public TutorialArrowsManager tutorialArrowManager;

    private void Start()
    {
        chefAnimator.SetBool("Dancing", true);
    }

    private void Update()
    {
        TryWalkToStallPoint();
    }


    public override void Interact(PlayerHandScript playerHand)
    {
        if (tutorialArrowManager.allTasksFinished)
        {
            SceneManager.LoadScene(teleportSceneName);
        }

        if (!startTalkFinish)
        {
            PlayChefDialogue();
        }
        else if (startTalkFinish && reachedStall)
        {
            PlayExplainationDialogue();
        }
       

    }

    public void PlayChefDialogue()
    {
        currentIndex++;
        if (currentIndex >= dialogueLines.Count)
        {
            chefAnimator.SetBool("Dancing", false);
            chefTransform.localRotation = Quaternion.Euler(0, 0, 0);
            chefTransform.localPosition = Vector3.zero;

            startTalkFinish = true;
            goingToStall = true;

            chefAnimator.SetBool("Walking", true);

            currentIndex = -1;
            slowTyper.CloseDialogue();

            return;
        }

        string chefNameShown = nameOfChef + ": ";

        string currentLine = dialogueLines[currentIndex];
        slowTyper.StartWritingSlowly(chefNameShown, currentLine, transform);

    }

    public void PlayExplainationDialogue()
    {

        playerMove.canMove = false;
        playerTurn.canTurn = false;

        currentExlainIndex++;

        if (currentExlainIndex == 0)
        {
            customerSpawner.SetActive(true);
        }

        if (currentExlainIndex >= explainationLines.Count)
        {
            //tutorialManager.SetCameraAsMain(tutorialManager.playerCam);
            playerMove.canMove = true;
            playerTurn.canTurn = true;
            slideShowImage.gameObject.SetActive(false);
            slowTyper.CloseDialogue();

            if (!tutorialArrowManager.allTasksFinished)
            {
                tutorialArrowManager.Setup();

            }
            //SceneManager.LoadScene(teleportSceneName);


            return;
        }

        ExplainationObj explainObj = explainationLines[currentExlainIndex];

        string chefNameShown = nameOfChef + ": ";
        slowTyper.StartWritingSlowly(chefNameShown, explainObj.dialogueLine, transform);

        if (explainObj.backgroundImage == null)
        {
            slideShowImage.gameObject.SetActive(false);
            return;
        }
        slideShowImage.sprite = explainObj.backgroundImage;
        slideShowImage.gameObject.SetActive(true);
    }

    public void TryWalkToStallPoint()
    {
        if (reachedStall || !goingToStall)
        {
            return;
        }

        Transform currentDestination = movePoints[currentDestinationIndex];

        transform.position = Vector3.MoveTowards(transform.position, currentDestination.position, Time.deltaTime * smoothValue);
        RotateToDestination(currentDestination);

        float distance = Vector3.Distance(transform.position, currentDestination.position);
        if (distance < 0.01f)
        {
            currentDestinationIndex++;

            if (currentDestinationIndex >= movePoints.Count)
            {
                reachedStall = true;
                transform.localRotation = Quaternion.Euler(0, 90, 0);
                chefAnimator.SetBool("Walking", false);

            }
        }
    }

    public void CheckIfPlayerServedCustomerAlready()
    {
        
    }

    public void RotateToDestination(Transform destination)
    {
        Vector3 angleVector = destination.position - transform.position;

        float angle = Mathf.Atan2(angleVector.x, angleVector.z) * Mathf.Rad2Deg;

        transform.localRotation = Quaternion.Euler(0, angle, 0);
    }
}

[Serializable]
public class ExplainationObj
{
    public Sprite backgroundImage;
    public string dialogueLine;
}