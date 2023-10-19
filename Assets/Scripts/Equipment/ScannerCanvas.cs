using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class ScannerCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text scanText;
    [SerializeField] ProceduralImage batteryImage;

    public void UpdateText(string text)
    {
        scanText.text = text;
    }

    public void UpdateBattery(float amount)
    {
        batteryImage.fillAmount = Mathf.Clamp(amount / 100f,0f,1f);
    }
}
