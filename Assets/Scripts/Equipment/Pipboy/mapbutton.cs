using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class mapbutton : MonoBehaviour
{
    public GameObject PanelGroup;

    [Button]
    public void ClickToMap()
    {
        PipboyMaterialController.Instance.PanelTransition();
        this.transform.parent.gameObject.SetActive(false);
        PanelGroup.SetActive(true);
    }
}
