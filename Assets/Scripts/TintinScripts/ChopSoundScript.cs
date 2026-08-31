using UnityEngine;
using System.Collections;

public class ChopSoundScript : MonoBehaviour
{
    public CarrotCutter cutterScript;

    public AudioSource soundSrc;
    public AudioClip chopClip;

    public float maxPlayDuration = 0.4f;

    private void Start()
    {
        cutterScript.OnChopped += OnChopped;
    }

    private void OnDestroy()
    {
        cutterScript.OnChopped -= OnChopped;
    }

    private void OnChopped(int currentChops, int requiredChops)
    {
        PlayChopSound();
    }

    private void PlayChopSound()
    {
        if (chopClip == null || soundSrc == null)
        {
            return;
        }

        soundSrc.PlayOneShot(chopClip);
        StartCoroutine(StopAfterDelay());
    }

    private IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(maxPlayDuration);
        soundSrc.Stop();
    }
}