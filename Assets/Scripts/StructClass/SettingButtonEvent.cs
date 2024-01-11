using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SettingButtonEvent
{
    public String eventName;
    public SettingButton_BarPanel button;
    public UnityEvent<string> buttonEvent;
    public CanvasGroup currentPanel;
}
