using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GeneralSoundManager : MonoBehaviour
{
    [Header("Music")]
    public MusicPlaylist musicPlaylist;
    public AudioSource musicSrc, effectsSrc;

    [Header("Music Fade")]
    public float musicFadeDuration = 1.5f;
    [Range(0f, 1f)] public float musicVolume = 1f;

    private Coroutine fadeRoutine;
    private Coroutine playSongRoutine;
    private bool isFading = false;
    private AudioClip lastPlayedTrack;

    private bool specialMusicActive = false;

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

        if (musicPlaylist != null && musicPlaylist.tracks.Count > 0)
        {
            playSongRoutine = StartCoroutine(PlaySong());
        }
    }

    // ---------------- MUSIC ----------------

    public void SwitchBackgroundMusic(AudioClip newClip, bool loop = false)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        musicSrc.loop = loop;
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

    public IEnumerator PlaySong()
    {
        while (true)
        {
            if (specialMusicActive)
            {
                yield return null;
                continue;
            }

            AudioClip nextTrack = PickRandomTrack(musicPlaylist.tracks);
            SwitchBackgroundMusic(nextTrack);

            yield return new WaitUntil(() => !isFading && !musicSrc.isPlaying);
        }
    }

    public AudioClip PickRandomTrack(List<AudioClip> trackList)
    {
        if (trackList == null || trackList.Count == 0) return null;
        if (trackList.Count == 1) return trackList[0];

        AudioClip picked;
        do
        {
            picked = trackList[Random.Range(0, trackList.Count)];
        }
        while (picked == lastPlayedTrack);

        lastPlayedTrack = picked;
        return picked;
    }

    public void PlaySpecialMusic(AudioClip clip)
    {
        specialMusicActive = true;
        SwitchBackgroundMusic(clip, loop: true);
    }

    public void StopSpecialMusic()
    {
        specialMusicActive = false;
        musicSrc.loop = false;

        if (playSongRoutine != null)
        {
            StopCoroutine(playSongRoutine);
        }

        playSongRoutine = StartCoroutine(PlaySong());
    }

    // ---------------- SFX ----------------

    public void PlaySoundEffect(SFXBank bank, string clipName, Vector3 position = default)
    {
        SFXBank.Entry entry = bank.GetEntry(clipName);
        if (entry == null || entry.clip == null) return;

        if (!entry.is3D)
        {
            effectsSrc.PlayOneShot(entry.clip);
            return;
        }

        GameObject tempGO = new GameObject("OneShotAudio_" + entry.clip.name);
        tempGO.transform.position = position;

        AudioSource source = tempGO.AddComponent<AudioSource>();
        source.clip = entry.clip;
        source.spatialBlend = 1f;
        source.minDistance = entry.minDistance;
        source.maxDistance = entry.maxDistance;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.Play();

        Destroy(tempGO, entry.clip.length);
    }
}