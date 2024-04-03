using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSettingController : PersistentSingleton<MouseSettingController>
{
    float mouseSenvalue = 25f;
    public float MouseSenvalue => mouseSenvalue;
    protected override void InitAfterAwake()
    {

    }

    public void SetMouseSen(float mouseSenSlider)
    {
        mouseSenvalue = mouseSenSlider * 100f;
    }



}
