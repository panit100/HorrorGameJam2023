using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class ScannerCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text scanText;
    [SerializeField] ProceduralImage batteryImage;
    [SerializeField] ProceduralImage scanImage;
    [SerializeField] private Image scanScreenImage;
    [SerializeField] private Sprite spriteNotDetect;
    [SerializeField] private Sprite spriteDetect;
    [SerializeField] private Sprite spriteScanning;

    ScanObjective scanObjective;

    void Update()
    {
        if(scanObjective != null)
            UpdateScanProgress(scanObjective.GetComponent<Scanable>().scanProgress);
        else
            UpdateScanProgress(0f);
    }

    public void UpdateText(string text)
    {
        // scanText.text = text;
        if(text == "Detect")
        {
            scanScreenImage.sprite = spriteDetect;
        }
        else if(text == "Not Found")
        {
            scanScreenImage.sprite = spriteNotDetect;
        }
        else if(text == "Scanning")
        {
            scanScreenImage.sprite = spriteScanning;
        }
    }

    public void UpdateBattery(float amount)
    {
        batteryImage.fillAmount = Mathf.Clamp(amount / 100f,0f,1f);
    }

    public void UpdateScanProgress(float progress)
    {
        scanImage.fillAmount = Mathf.Clamp(progress / 100f,0f,1f);
    }

    public void SetScanner(ScanObjective _scanObjective)
    {
        scanObjective = _scanObjective;
    }
}
