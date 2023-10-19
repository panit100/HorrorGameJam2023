using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraEquipment : Equipment
{
    [SerializeField] private GameObject animationRoot;
    [SerializeField] CanvasGroup cameraShotCanvas;
    [SerializeField] RawImage imageDisplay;
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] Camera screenshotCamera;
    [SerializeField] float displayImageTime;
    [SerializeField] private GameObject MeshGroup;
    [SerializeField] private Image Flash;
    public UnityEvent onUse;

    Texture2D screenshot;
    bool isUsing = false;

    [Header("For animation")] 
    [SerializeField]private float AnimDuration;
    [SerializeField]private Vector3 initpos;
    [SerializeField]private Vector3 endpos;
    [SerializeField]private Vector3 initrot;
    [SerializeField]private Vector3 endrot;
    private Tween Onhold;
    void Start()
    {
        MeshGroup.SetActive(false);
        equipmentType = EquipmentType.Camera;
        EnableCanvas(false);
    }

    public override void OnUse()
    {
        base.OnUse();

        if(isPress)
            OnPressShutter();
    }
    
    [Button]
    public override void HoldAnim()
    {
        base.HoldAnim();
        if(!Application.isPlaying)return;   
        MeshGroup.SetActive(true);
        Onhold.Kill();
        animationRoot.transform.localPosition = initpos;
        Onhold = animationRoot.transform.DOLocalMove(endpos, AnimDuration).SetEase(Ease.OutExpo);
        animationRoot.transform.localRotation = Quaternion.Euler(initrot);
        animationRoot.transform.DOLocalRotate(endrot,AnimDuration).SetEase(Ease.OutExpo);
        MeshGroup.SetActive(true);
        
    }
    
    public override void PutAnim()
    {
        base.HoldAnim();
        if(!Application.isPlaying)return;   
        MeshGroup.SetActive(false);
        
    }
   
    void OnPressShutter()
    {
        if(isUsing)
            return;
            
        isUsing = true;

        ActiveFlash();
        CaptureScreenshot();
        StartCoroutine(DisplayImage());
        onUse?.Invoke();
    }

    void ActiveFlash()
    {
        Color temp = Flash.color;
        temp.a = 1;
        DOVirtual.Float(temp.a, 0, 1f, (value =>
        {
            temp.a = value;
            Flash.color = temp;
        })).SetEase(Ease.Flash);
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

    void OnDestroy() 
    {
        onUse?.RemoveAllListeners();
    }
}
