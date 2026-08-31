using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EventMusicBank", menuName = "Audio/Event Music Bank")]
public class EventMusicBank : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public HawkerEventType eventType;
        public AudioClip musicClip;
    }

    public List<Entry> entries = new List<Entry>();

    public AudioClip GetClipFor(HawkerEventType type)
    {
        foreach (var entry in entries)
        {
            if (entry.eventType == type)
                return entry.musicClip;
        }

        return null;
    }
}