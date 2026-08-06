using UnityEngine;
using System.Collections.Generic;

public class GeneralSoundManager : MonoBehaviour
{
    public AudioClip musicClip1, musicClip2, storeClip;

    public List<AudioClip> morningMusicList = new List<AudioClip>();
    public List<AudioClip> afternoonMusicList = new List<AudioClip>();

    public AudioClip pickupClip, sizzleClip;

    public AudioSource musicSrc, effectsSrc;

    public static GeneralSoundManager instance;

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

    // Only for general sounds like pickup or click
    public void PlaySoundEffect(AudioClip soundEffect)
    {
        effectsSrc.PlayOneShot(soundEffect);
    }


}
