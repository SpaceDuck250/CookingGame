using UnityEngine;
using System.Collections.Generic;
using System;

public class TutorialChefScript : Interactable
{
    public SlowTyper slowTyper;

    public List<string> dialogueLines = new List<string>();
    public int currentIndex = -1;

    public List<ExplainationObj> explainationLines = new List<ExplainationObj>();
    public int currentExlainIndex = -1;

    public string nameOfChef = "";

    public bool goingToStall = false;

    public bool startTalkFinish = false;
    public bool reachedStall = false;

    public List<Transform> movePoints = new List<Transform>();

    public int currentDestinationIndex;

    public Transform chefTransform;

    public float smoothValue;

    public Animator chefAnimator;

    public Camera playerCam;

    public TutorialManagerScript tutorialManager;

    private void Update()
    {
        TryWalkToStallPoint();
    }


    public override void Interact(PlayerHandScript playerHand)
    {
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
            startTalkFinish = true;
            goingToStall = true;
            chefAnimator.SetBool("Walking", true);

            currentIndex = -1;
            slowTyper.CloseDialogue();

            return;
        }

        string chefNameShown = nameOfChef + ": ";

        string currentLine = dialogueLines[currentIndex];
        slowTyper.StartWritingSlowly(chefNameShown, currentLine);

    }

    public void PlayExplainationDialogue()
    {


        currentExlainIndex++;

        if (currentExlainIndex == 0)
        {
            tutorialManager.SetCameraAsMain(tutorialManager.gameCamera);
        }

        if (currentExlainIndex >= explainationLines.Count)
        {
            tutorialManager.SetCameraAsMain(tutorialManager.playerCam);

            return;
        }

        ExplainationObj explainObj = explainationLines[currentExlainIndex];

        tutorialManager.gameCamera.transform.position = explainObj.cameraPoint.position;
        tutorialManager.gameCamera.transform.localRotation = Quaternion.Euler(explainObj.cameraRotation);

        string chefNameShown = nameOfChef + ": ";
        slowTyper.StartWritingSlowly(chefNameShown, explainObj.dialogueLine);
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
                transform.localRotation = Quaternion.Euler(0, 150, 0);
                chefAnimator.SetBool("Walking", false);

            }
        }
    }

    public void RotateToDestination(Transform destination)
    {
        Vector3 angleVector = destination.position - transform.position;

        float angle = Mathf.Atan2(angleVector.x, angleVector.z) * Mathf.Rad2Deg;

        transform.localRotation = Quaternion.Euler(0, angle + 30, 0);
    }
}

[Serializable]
public class ExplainationObj
{
    public Transform cameraPoint;
    public Vector3 cameraRotation;
    public string dialogueLine;
}