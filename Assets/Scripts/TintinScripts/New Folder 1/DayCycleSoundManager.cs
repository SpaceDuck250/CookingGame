using UnityEngine;

public class DayCycleSoundManager : MonoBehaviour
{
    public AudioClip nightMusicClip;
    public SFXBank generalBank;

    private void OnEnable()
    {
        DaySystemManager.OnDayEnd += HandleDayEnd;
        DaySystemManager.OnDayStart += HandleDayStart;
    }

    private void OnDisable()
    {
        DaySystemManager.OnDayEnd -= HandleDayEnd;
        DaySystemManager.OnDayStart -= HandleDayStart;
    }

    private void HandleDayEnd(PlayerDailyStats stats)
    {
        GeneralSoundManager.instance.PlaySoundEffect(generalBank, "day_complete");
        GeneralSoundManager.instance.PlaySpecialMusic(nightMusicClip);
    }

    private void HandleDayStart()
    {
        GeneralSoundManager.instance.StopSpecialMusic();
    }
}