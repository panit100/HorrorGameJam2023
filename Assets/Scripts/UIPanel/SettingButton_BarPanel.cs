using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SettingButton_BarPanel : MonoBehaviour
{
   
     Animator buttonAnimator;
     UnityEvent<string> clickAction;

    
     public void OnPointerClick(BaseEventData eventData)
     {
         buttonAnimator.SetBool("Clicked",false);
       clickAction?.Invoke(gameObject.name);
     }

     public void SetButtonState(bool state)
     {
         buttonAnimator.SetBool("Clicked",state);
     }

     public void AddEvent(UnityEvent<string> action)
     {
         clickAction = action;
     }
    void Awake()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        clickAction = null;
    }
}
