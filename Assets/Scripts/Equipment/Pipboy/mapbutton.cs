using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapbutton : MonoBehaviour
{
    public GameObject PanelGroup;
    public void ClickToMap()
    {
        PipboyMaterialController.Instance.PanelTransition();
        this.transform.parent.gameObject.SetActive(false);
        PanelGroup.SetActive(true);
    }
}
