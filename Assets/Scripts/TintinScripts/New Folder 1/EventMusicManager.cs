using UnityEngine;

public class EventMusicManager : MonoBehaviour
{
    public EventMusicBank eventMusicBank;

    private void OnEnable()
    {
        AIEventSystemScript.OnEventStarted += HandleEventStarted;
        AIEventSystemScript.OnEventFinished += HandleEventFinished;

        HealthInspectorSpawnerScript.OnInspectorSpawned += HandleInspectorStarted;
        HealthInspectorAIScript.OnInspectionComplete += HandleInspectorFinished;
    }

    private void OnDisable()
    {
        AIEventSystemScript.OnEventStarted -= HandleEventStarted;
        AIEventSystemScript.OnEventFinished -= HandleEventFinished;

        HealthInspectorSpawnerScript.OnInspectorSpawned -= HandleInspectorStarted;
        HealthInspectorAIScript.OnInspectionComplete -= HandleInspectorFinished;
    }

    private void HandleEventStarted(HawkerEventType eventType)
    {
        AudioClip clip = eventMusicBank.GetClipFor(eventType);
        if (clip == null) return;

        GeneralSoundManager.instance.PlaySpecialMusic(clip);
    }

    private void HandleEventFinished(HawkerEventType eventType)
    {
        AudioClip clip = eventMusicBank.GetClipFor(eventType);
        if (clip == null) return;

        GeneralSoundManager.instance.StopSpecialMusic();
    }

    private void HandleInspectorStarted()
    {
        AudioClip clip = eventMusicBank.GetClipFor(HawkerEventType.Inspector);
        if (clip == null) return;

        GeneralSoundManager.instance.PlaySpecialMusic(clip);
    }

    private void HandleInspectorFinished()
    {
        AudioClip clip = eventMusicBank.GetClipFor(HawkerEventType.Inspector);
        if (clip == null) return;

        GeneralSoundManager.instance.StopSpecialMusic();
    }
}