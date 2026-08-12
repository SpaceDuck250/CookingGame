using UnityEngine;
using System.Collections.Generic;

public class TutorialChefScript : Interactable
{
    public SlowTyper slowTyper;

    public List<string> dialogueLines = new List<string>();
    public int currentIndex = -1;

    public string nameOfChef = "";

    public override void Interact(PlayerHandScript playerHand)
    {
        PlayChefDialogue();
        
    }

    public void PlayChefDialogue()
    {
        currentIndex++;
        if (currentIndex >= dialogueLines.Count)
        {
            currentIndex = -1;
            slowTyper.CloseDialogue();
            return;
        }

        string chefNameShown = nameOfChef + ": ";

        string currentLine = dialogueLines[currentIndex];
        slowTyper.StartWritingSlowly(chefNameShown, currentLine);

    }
    

}
