using System;
using UnityEngine;

public class TimeManager : PersistentSingleton<TimeManager>
{
    [SerializeField] int StartHour;
    [SerializeField] int StartMin;
    [SerializeField] int StartSec;

    TimeSpan currentTime;

    protected override void InitAfterAwake()
    {

    }

    public void SetCurrentTime()
    {
        currentTime = new TimeSpan(StartHour, StartMin, StartSec);
    }

    void Update()
    {
        if (!GameManager.Instance.IsPause)
            currentTime += TimeSpan.FromSeconds(Time.deltaTime);
    }

    public string GetCurrentTime()
    {
        return $"{currentTime.Hours}:{currentTime.Minutes}:{currentTime.Seconds} ";
    }
}
