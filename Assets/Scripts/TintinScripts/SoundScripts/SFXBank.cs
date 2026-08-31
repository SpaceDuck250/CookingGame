using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SFXBank", menuName = "Audio/SFX Bank")]
public class SFXBank : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string name;
        public AudioClip clip;

        public bool is3D = false;

        public float minDistance = 3f;

        public float maxDistance = 15f;
    }

    public List<Entry> clips = new List<Entry>();

    private Dictionary<string, Entry> lookup;

    private void BuildLookupIfNeeded()
    {
        if (lookup != null) return;

        lookup = new Dictionary<string, Entry>();
        foreach (var entry in clips)
            lookup[entry.name] = entry;
    }

    public Entry GetEntry(string clipName)
    {
        BuildLookupIfNeeded();

        if (lookup.TryGetValue(clipName, out Entry entry))
            return entry;

        Debug.LogWarning($"{name}: no clip called '{clipName}'");
        return null;
    }
}