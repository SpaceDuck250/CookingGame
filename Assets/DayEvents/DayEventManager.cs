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
    public TimeLevel[] timeLevelList = new TimeLevel[3];
    public int currentTimeLevelIndex = -1;

    public TimeLevel currentTimeLevel;

    public float timer;
    public float duration;

    public bool canRunTimer;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!canRunTimer)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            canRunTimer = false;
            timer = 0;

            TransitionToNextTimeLevel();
        }
    }

    private void TransitionToNextTimeLevel()
    {
        currentTimeLevelIndex++;
        if (currentTimeLevelIndex >= timeLevelList.Length)
        {
            currentTimeLevelIndex = timeLevelList.Length - 1;
            return;
        }

        currentTimeLevel = timeLevelList[currentTimeLevelIndex];
        duration = currentTimeLevel.duration;

        canRunTimer = true;
    }



}



