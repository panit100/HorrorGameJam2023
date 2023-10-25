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

    public string GetLogString(Color senderColor , Color timeColor, Color massageColor)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(senderColor)}><b>{sender}</color></b> : <color=#{ColorUtility.ToHtmlStringRGB(timeColor)}>{sendTime}</color> : <color=#{ColorUtility.ToHtmlStringRGB(massageColor)}>{massage}</color>";
    }
}
