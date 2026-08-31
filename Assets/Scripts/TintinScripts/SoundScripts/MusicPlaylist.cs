using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MusicPlaylist", menuName = "Audio/Music Playlist")]
public class MusicPlaylist : ScriptableObject
{
    public List<AudioClip> tracks = new List<AudioClip>();
}