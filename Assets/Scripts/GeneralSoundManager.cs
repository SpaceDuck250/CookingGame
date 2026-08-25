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
    // Total time for one transition - half spent fading out the old song, half fading in the new one
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
        // Safety net: if this field got left at 0 (e.g. it was added after this component
        // already existed in the scene), fall back to full volume instead of silent music.
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

        // Fade the current song out (skips instantly if nothing was playing yet)
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

        // Swap to the new clip and fade it in
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

        playSongRoutine = StartCoroutine(PlaySong());
    }

    // Called when the health inspector shows up - takes over the music until the visit is done
    public void StartInspectorMusic()
    {
        if (playSongRoutine != null)
        {
            StopCoroutine(playSongRoutine);
            playSongRoutine = null;
        }

        // Loop the inspector music since we don't know how long the visit will last
        musicSrc.loop = true;
        SwitchBackgroundMusic(inspectorMusicClip);
    }

    // Called when the inspector leaves - hands control back to the normal playlist
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