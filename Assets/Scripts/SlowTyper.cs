using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SlowTyper : MonoBehaviour
{
    public TextMeshProUGUI dialogueTextComponent;
    public string dialogueLine;
    public float typeTime;

    public void StartWritingSlowly(string name, string newLine)
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine(name, newLine));
    }

    public IEnumerator TypeLine(string name, string newLine)
    {
        dialogueTextComponent.text = string.Empty;

        dialogueLine = newLine;
        char[] chars = dialogueLine.ToCharArray();

        dialogueTextComponent.text += name;

        for (int i = 0; i < chars.Length; i++)
        {
            dialogueTextComponent.text += chars[i];
            yield return new WaitForSeconds(typeTime);
        }
    }
}
