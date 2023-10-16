using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraEquipment : Equipment
{
    [SerializeField] CanvasGroup cameraShotCanvas;
    [SerializeField] RawImage imageDisplay;
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] Camera screenshotCamera;
    [SerializeField] float displayImageTime;

    Texture2D screenshot;
    bool isUsing = false;

    void Start()
    {
        equipmentType = EquipmentType.Camera;
        EnableCanvas(false);
    }

    public override void OnUse()
    {
        base.OnUse();

        if(isPress)
            OnPressShutter();
    }

    void OnPressShutter()
    {
        if(isUsing)
            return;
            
        isUsing = true;

        ActiveFlash();
        CaptureScreenshot();
        StartCoroutine(DisplayImage());
    }

    void ActiveFlash()
    {

    }

    void CaptureScreenshot()
    {
        // Disable UI temporarily to exclude it from the screenshot
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = false;
        }

        // Capture the screenshot
        screenshotCamera.targetTexture = null; // To capture the whole screen
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        screenshotCamera.targetTexture = rt;
        screenshotCamera.Render();

        // Read the pixels from the RenderTexture
        screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();
        RenderTexture.active = null;
        screenshotCamera.targetTexture = null;
        Destroy(rt);

        // Re-enable UI
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = true;
        }

    }

    IEnumerator DisplayImage()
    {
        yield return new WaitForEndOfFrame();
        // Display the screenshot
        imageDisplay.texture = screenshot;

        EnableCanvas(true);

        yield return new WaitForSeconds(displayImageTime);

        EnableCanvas(false);
        isUsing = false;
    }

    void EnableCanvas(bool enable)
    {
        if(enable)
            cameraShotCanvas.alpha = 1;
        else
            cameraShotCanvas.alpha = 0;
    }
}
