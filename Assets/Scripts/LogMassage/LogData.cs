using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LogData
{
    public string sender;
    public string sendTime; 
    public string massage;

    public LogData(string _sender,string _sendTime,string _massage)
    {
        sender = _sender;
        sendTime = _sendTime;
        massage = _massage;
    }

    public string GetLogString()
    {
        return $"{sender} {sendTime} {massage}";
    }
}
