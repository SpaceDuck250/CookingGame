using UnityEngine;
using UnityEngine.Audio;

public class DialogueSoundManager : MonoBehaviour
{
    public AudioSource dialogueSource;

    public AudioClip normalCustomerTalkingClip, chefTalkingClip;

    public void PlayTalkingAudio(AudioClip clip)
    {
        dialogueSource.clip = null;
        dialogueSource.Stop();
        dialogueSource.clip = clip;
        dialogueSource.Play();
    }

    public void PauseTalkingAudio()
    {
        dialogueSource.Pause();
    }
}
