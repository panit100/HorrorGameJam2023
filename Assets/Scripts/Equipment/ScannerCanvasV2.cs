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
        if(scanObjective != null)
            UpdateScanProgress(scanObjective.GetComponent<Scanable>().scanProgress);
        else
            UpdateScanProgress(0f);
    }

    public void UpdateText(string text)
    {
        scanText.text = text;
    }

    // public void UpdateBattery(float amount)
    // {
    //    
    //     //  batteryImage.fillAmount = Mathf.Clamp(amount / 100f,0f,1f);
    //     scanText.text = Mathf.Clamp(amount / 100f, 0f,1f).ToString();
    // }
    public void SwitchImage()
    {
        switch (state)
        {
            case ScanState.Complete:
                Placeholder.sprite = Done;
            case ScanState.Error :
                Placeholder.sprite = 
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
