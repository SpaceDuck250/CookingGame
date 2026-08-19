using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
        //StartCoroutine(PlaySong());
        if (morningMusicList.Count == 0)
        {
            return;
        }

        StartCoroutine(SetupMusic());
    }

    public void SwitchBackgroundMusic(AudioClip newClip)
    {
        //musicSrc.loop = true;
        musicSrc.clip = newClip;
        musicSrc.Play();
    }

    // Only for general sounds like pickup or click
    public void PlaySoundEffect(AudioClip soundEffect)
    {
        effectsSrc.PlayOneShot(soundEffect);
    }
    public IEnumerator SetupMusic()
    {
        foreach (AudioClip clip in morningMusicList)
        {
            clip.LoadAudioData();

            while (clip.loadState == AudioDataLoadState.Loading)
                yield return null;
        }

        foreach (AudioClip clip in afternoonMusicList)
        {
            clip.LoadAudioData();

            while (clip.loadState == AudioDataLoadState.Loading)
                yield return null;
        }

        StartCoroutine(PlaySong());
    }

    public IEnumerator PlaySong()
    {
        while (true)
        {
            List<AudioClip> audioListToUse = GetListFromTimeOfDay(TimeCycleScript.currentTimeOfDay);

            AudioClip randomClip = PickRandomSongFromList(audioListToUse);

            SwitchBackgroundMusic(randomClip);

            yield return new WaitUntil(() => !musicSrc.isPlaying);
        }
        
    }

    public AudioClip PickRandomSongFromList(List<AudioClip> musicList)
    {
        int ranVal = Random.Range(0, musicList.Count);

        return musicList[ranVal];
    }

    public List<AudioClip> GetListFromTimeOfDay(TimeOfDay currentTime)
    {
        switch (currentTime)
        {
            case TimeOfDay.Day:
                return morningMusicList;

            case TimeOfDay.Afternoon:
                return afternoonMusicList;

            case TimeOfDay.Evening:
                return afternoonMusicList;
            default:
                return null;
        }
    }


}
