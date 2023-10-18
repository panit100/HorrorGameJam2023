using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LogType
{
    Massage,
    Objective,
}

[Serializable]
public class LogData
{
    public string sender;
    public string sendTime; 
    public string massage;
    public LogType logType;

    public LogData(string _sender,string _sendTime,string _massage, LogType _logType)
    {
        sender = _sender;
        sendTime = _sendTime;
        massage = _massage;
        logType = _logType;
    }

    public string GetLogString()
    {
        if(logType == LogType.Massage)
            return $"<style=SenderName>{sender}</style> : <style=SendTime>{sendTime}</style> : <style=SendMassageLog>{massage}</style>";
        else
            return $"<style=SenderName>{sender}</style> : <style=SendTime>{sendTime}</style> : <style=SendObjectiveLog>{massage}</style>";
    }
}
