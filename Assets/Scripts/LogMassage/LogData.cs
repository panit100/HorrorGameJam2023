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
    public string senderColor;
    public string timeColor;
    public string massageColor;
    public LogType logType;

    public LogData(string _sender,string _sendTime,string _massage, LogType _logType,string _senderColor,string _timeColor,string _massageColor)
    {
        sender = _sender;
        sendTime = _sendTime;
        massage = _massage;
        logType = _logType;
        senderColor = _senderColor;
        timeColor = _timeColor;
        massageColor = _massageColor;
    }

    public string GetLogString()
    {   
        string _sender = $"<color=#{senderColor}> {sender} </color>";
        string _time = $"<color=#{timeColor}> {sendTime} </color>";
        string _massage = $"<color=#{massageColor}> {massage} </color>";
        return $"{_sender} : {_time} : {_massage}";
    }
}
