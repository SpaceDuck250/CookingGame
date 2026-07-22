using System;
using System.Collections.Generic;
using UnityEngine;
using HawkerEventAndTime;

namespace HawkerEventAndTime
{
    [Serializable]
    public class TimeLevel
    {
        public string timeName;

        // Should contain the hawkerevent abstract class component
        public List<HawkerEvent> possibleEvents = new List<HawkerEvent>();

        public float duration;

        public List<AudioClip> possibleMusic = new List<AudioClip>();
        public AudioClip timeAmbienceMusic;

    }
}

public class DayEventManager : MonoBehaviour
{
    public List<TimeLevel> timeLevelList = new List<TimeLevel>();
    

}



