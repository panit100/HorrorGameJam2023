using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MassageText : MonoBehaviour
{
    [SerializeField] TMP_Text massageText;

    //TODO: Add sound

    public void SetMassageText(string text)
    {
        massageText.text = text;
    }
}
