using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Button_MainMenu : MonoBehaviour,IAstronosisButtonInterface
{
    Animator buttonAnimator;
    UnityAction clickAction;
    public CanvasGroup buttoncanvasgroup { get; set; }

    void Awake()
    {
        buttonAnimator = GetComponent<Animator>();
        TryGetComponent<CanvasGroup>(out CanvasGroup tempCanvasGroup);
        buttoncanvasgroup = tempCanvasGroup;
    }
    public void InterableState(bool state)
    {
        buttoncanvasgroup.interactable = state;
        
    }
    public void OnPointerClick(BaseEventData eventData)
    {
        buttonAnimator.SetBool("Clicked",true);
        clickAction?.Invoke();
        buttonAnimator.Rebind();
        buttonAnimator.Update(0f);
    }

    public void OnHover(BaseEventData eventData)
    {
        buttonAnimator.SetTrigger("Hover");
    }
    public void ExitHover(BaseEventData eventData)
    {
        buttonAnimator.SetTrigger("Normal");
    }

    public void AddListener(UnityAction action)
    {
        
        clickAction += action;
    }
}
