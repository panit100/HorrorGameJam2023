using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using HorrorJam.Audio;

public class MassageText : MonoBehaviour
{
    [SerializeField] TMP_Text massageText;

    //TODO: Add sound && Show text that u have massage

    public void SetMassageText(string text)
    {
        // AudioManager.Instance.PlayOneShot();
        massageText.text = text;
    }
}
