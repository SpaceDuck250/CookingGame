using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip musicClip1, musicClip2, storeClip;

    public AudioClip pickupClip, sizzleClip;

    public AudioSource musicSrc, effectsSrx, longEffectsSrc;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SwitchBackgroundMusic(musicClip1);
    }

    public void SwitchBackgroundMusic(AudioClip newClip)
    {
        musicSrc.loop = true;
        musicSrc.clip = newClip;
        musicSrc.Play();
    }
}
