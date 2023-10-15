using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraEquipment : Equipment
{
    [SerializeField] RawImage imageDisplay;
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] Camera screenshotCamera;

    public override void OnUse()
    {
        base.OnUse();

        if(isPress)
            OnPressShutter();
    }

    void OnPressShutter()
    {
        ActiveFlash();
        CaptureScreenshot();
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
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
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

        // Display the screenshot
        imageDisplay.texture = screenshot;
    }
}
