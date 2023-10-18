using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MassageText : MonoBehaviour
{
    [SerializeField] TMP_Text massageText;

    //TODO: Add sound && Show text that u have massage

    public void SetMassageText(string text)
    {
        massageText.text = text;
    }
}
