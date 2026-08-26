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

    [Header("Music Fade")]
    public float musicFadeDuration = 1.5f;
    [Range(0f, 1f)] public float musicVolume = 1f;

    [Header("Health Inspector")]
    public AudioClip inspectorMusicClip;

    private Coroutine fadeRoutine;
    private Coroutine playSongRoutine;
    private bool isFading = false;

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
        if (musicVolume <= 0f)
        {
            musicVolume = 1f;
        }

        musicSrc.volume = 0f;

        if (morningMusicList.Count == 0)
        {
            return;
        }

        StartCoroutine(SetupMusic());
    }

    public void SwitchBackgroundMusic(AudioClip newClip)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeToNewSong(newClip));
    }

    private IEnumerator FadeToNewSong(AudioClip newClip)
    {
        isFading = true;

        float halfDuration = musicFadeDuration / 2f;

        float startVolume = musicSrc.volume;
        float t = 0f;

        while (t < halfDuration)
        {
            t += Time.deltaTime;
            musicSrc.volume = Mathf.Lerp(startVolume, 0f, t / halfDuration);
            yield return null;
        }

        musicSrc.volume = 0f;
        musicSrc.Stop();

        musicSrc.clip = newClip;
        musicSrc.Play();

        t = 0f;
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            musicSrc.volume = Mathf.Lerp(0f, musicVolume, t / halfDuration);
            yield return null;
        }

        musicSrc.volume = musicVolume;
        isFading = false;
    }

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

        playSongRoutine = StartCoroutine(PlaySong());
    }

    public void StartInspectorMusic()
    {
        if (playSongRoutine != null)
        {
            StopCoroutine(playSongRoutine);
            playSongRoutine = null;
        }

        musicSrc.loop = true;
        SwitchBackgroundMusic(inspectorMusicClip);
    }

    public void StopInspectorMusic()
    {
        musicSrc.loop = false;
        playSongRoutine = StartCoroutine(PlaySong());
    }

    public IEnumerator PlaySong()
    {
        while (true)
        {
            List<AudioClip> audioListToUse = GetListFromTimeOfDay(TimeCycleScript.currentTimeOfDay);

            AudioClip randomClip = PickRandomSongFromList(audioListToUse);

            SwitchBackgroundMusic(randomClip);

            yield return new WaitUntil(() => !isFading && !musicSrc.isPlaying);
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