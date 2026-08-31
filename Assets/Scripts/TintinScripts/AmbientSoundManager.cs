using UnityEngine;

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