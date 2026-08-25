using UnityEngine;

// Plays a looping background ambience track (e.g. crowd chatter).
// Put this on its own GameObject anywhere in the scene - it just needs to exist and be active.
public class AmbientSoundManager : MonoBehaviour
{
    public AudioSource ambienceSrc;
    public AudioClip crowdChatterClip;

    [Range(0f, 1f)] public float ambienceVolume = 0.5f;

    private void Start()
    {
        if (crowdChatterClip == null)
        {
            return;
        }

        ambienceSrc.clip = crowdChatterClip;
        ambienceSrc.loop = true;
        ambienceSrc.volume = ambienceVolume;
        ambienceSrc.Play();
    }
}