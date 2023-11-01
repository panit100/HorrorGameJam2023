using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class ScannerCanvasV2 : MonoBehaviour
{
    [SerializeField] TMP_Text scanText;
    [SerializeField] private Image Placeholder;
    [SerializeField] Sprite batteryImage;
    [SerializeField] Sprite scanImage;
    [SerializeField] Sprite Ready;
    [SerializeField] Sprite Error;
    [SerializeField] Sprite Done;

    public enum ScanState
    {
        Scanning,
        Ready,
        Drain,
        Error,
        Complete
    }
    public ScanState state;
    
    ScanObjective scanObjective;

    void Update()
    {
        if (scanObjective != null)
        {
            UpdateScanProgress(scanObjective.GetComponent<Scanable>().scanProgress);
            if (scanObjective.GetComponent<Scanable>().scanProgress >= 100)
                SwitchImage(ScanState.Complete);
        }
        else
            UpdateScanProgress(0f);
          
    }

    public void UpdateText(string text)
    {
        SwitchImage(ScanState.Scanning);
        scanText.text = text;
    }

    public void UpdateBattery(float amount)
    {
        //  batteryImage.fillAmount = Mathf.Clamp(amount / 100f,0f,1f);
    }
    public void SwitchImage(ScanState state)
    {
        switch(state)
        {
            case ScanState.Complete:
                Placeholder.sprite = Done;
                break;
            case ScanState.Error :
                Placeholder.sprite = Error;
                break;
            case ScanState.Drain:
                Placeholder.sprite = batteryImage;
                break;
            case ScanState.Ready:
                Placeholder.sprite = Ready;
                break;
            case ScanState.Scanning :
                Placeholder.sprite = scanImage;
                break;
        }
    }

    public void UpdateScanProgress(float progress)
    {
      //  scanImage.fillAmount = Mathf.Clamp(progress / 100f,0f,1f);
      Placeholder.sprite = scanImage;
      scanText.text = Mathf.Clamp(progress / 100f, 0f,1f).ToString();
    }

    public void SetScanner(ScanObjective _scanObjective)
    {
        scanObjective = _scanObjective;
    }
}
