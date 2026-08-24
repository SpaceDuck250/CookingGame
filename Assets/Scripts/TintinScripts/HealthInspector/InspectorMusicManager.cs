using UnityEngine;

// Bridges the Health Inspector system with the music manager -
// swaps to tense inspector music while they're on-site, then hands back to the normal playlist.
public class InspectorMusicManager : MonoBehaviour
{
    private void Start()
    {
        HealthInspectorSpawnerScript.OnInspectorSpawned += HandleInspectorSpawned;
        HealthInspectorAIScript.OnInspectionComplete += HandleInspectionComplete;
    }

    private void OnDestroy()
    {
        HealthInspectorSpawnerScript.OnInspectorSpawned -= HandleInspectorSpawned;
        HealthInspectorAIScript.OnInspectionComplete -= HandleInspectionComplete;
    }

    private void HandleInspectorSpawned()
    {
        if (GeneralSoundManager.instance != null)
        {
            GeneralSoundManager.instance.StartInspectorMusic();
        }
    }

    private void HandleInspectionComplete()
    {
        if (GeneralSoundManager.instance != null)
        {
            GeneralSoundManager.instance.StopInspectorMusic();
        }
    }
}