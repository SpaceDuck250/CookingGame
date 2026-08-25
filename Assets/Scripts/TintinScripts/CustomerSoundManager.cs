using UnityEngine;
using Customer;

// Attach this to the same customer prefab as CustomerStateMachine.
// Uses a local AudioSource so the sound plays spatially from that customer's position.
public class CustomerSoundManager : MonoBehaviour
{
    public CustomerStateMachine stateMachine;
    public AudioSource soundSrc;

    public AudioClip angryClip;

    private void Start()
    {
        stateMachine.OnCustomerMoodChange += HandleMoodChanged;
    }

    private void OnDestroy()
    {
        stateMachine.OnCustomerMoodChange -= HandleMoodChanged;
    }

    private void HandleMoodChanged(CustomerMood newMood)
    {
        if (newMood == CustomerMood.Angry)
        {
            PlayClip(angryClip);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null || soundSrc == null)
        {
            return;
        }

        soundSrc.PlayOneShot(clip);
    }
}