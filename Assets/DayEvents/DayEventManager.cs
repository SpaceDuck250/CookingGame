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

        // In minutes
        public float duration;

        public List<AudioClip> possibleMusic = new List<AudioClip>();
        public AudioClip timeAmbienceMusic;

    }
}

public class DayEventManager : MonoBehaviour
{
    public List<TimeLevel> timeLevelList = new List<TimeLevel>();

    public Dictionary<string, HawkerEvent> everyEventDictionary = new Dictionary<string, HawkerEvent>();

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {
            print(timeLevelList[0].possibleEvents[1].eventName);
        }
    }


}



