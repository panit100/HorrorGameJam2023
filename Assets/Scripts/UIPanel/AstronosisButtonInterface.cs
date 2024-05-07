using UnityEngine;
using UnityEngine.EventSystems;

public  interface IAstronosisButtonInterface
{
     CanvasGroup buttoncanvasgroup { get; set; }
     void OnPointerClick(BaseEventData eventData)
    {
       
    }

     void InterableState(bool state)
    {
        
    }
}
