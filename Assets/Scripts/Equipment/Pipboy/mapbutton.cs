using System.Collections;
using System.Collections.Generic;
using HorrorJam.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

public class mapbutton : MonoBehaviour
{
    public GameObject PanelGroup;

    [Button]
    public void ClickToMap()
    {
        AudioManager.Instance.PlayOneShot("clickUI");
        PipboyManager.Instance.PanelTransition();
        this.transform.parent.gameObject.SetActive(false);
        PanelGroup.SetActive(true);
    }
}
