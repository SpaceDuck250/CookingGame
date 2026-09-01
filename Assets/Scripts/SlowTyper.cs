using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SlowTyper : MonoBehaviour
{
    public TextMeshProUGUI dialogueTextComponent;
    public string dialogueLine;
    public float typeTime;

    public DialogueSoundManager soundManager;

    public GameObject dialogueContainer;

    public Image customerPortraitImage;

    public bool playerFrozen = false;

    private void Start()
    {
        typeTime = 0.01f;
    }

    public void StartWritingSlowly(string name, string newLine, Transform talker, Sprite customerSprite = null, bool freezePlayer = true)
    {
        if (freezePlayer)
        {
            playerFrozen = true;
            PlayerHandScript.instance.FreezePlayer(true, talker);
        }

        dialogueContainer.SetActive(true);

        SetPortrait(customerSprite);

        StopAllCoroutines();
        StartCoroutine(TypeLine(name, newLine));
    }

    private void SetPortrait(Sprite customerSprite)
    {
        if (customerPortraitImage == null)
        {
            return;
        }

        if (customerSprite == null)
        {
            customerPortraitImage.enabled = false;
            return;
        }

        customerPortraitImage.sprite = customerSprite;
        customerPortraitImage.enabled = true;
    }

    public IEnumerator TypeLine(string name, string newLine)
    {
        soundManager.PlayTalkingAudio(soundManager.normalCustomerTalkingClip);

        dialogueTextComponent.text = string.Empty;

        dialogueLine = newLine;
        char[] chars = dialogueLine.ToCharArray();

        dialogueTextComponent.text += name;

        for (int i = 0; i < chars.Length; i++)
        {
            dialogueTextComponent.text += chars[i];
            yield return new WaitForSeconds(typeTime);
        }

        soundManager.PauseTalkingAudio();
    }

    public void CloseDialogue()
    {
        if (playerFrozen)
        {
            playerFrozen = false;
            PlayerHandScript.instance.FreezePlayer(false, transform);

        }
        dialogueContainer.SetActive(false);

    }
}