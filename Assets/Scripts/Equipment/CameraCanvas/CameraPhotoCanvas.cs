using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraPhotoCanvas : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RawImage photo;
    [SerializeField] TMP_Text imageIndexText;
    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;
    [SerializeField] Texture2D nullImageTexture;

    int currentImage = 0;

    void Start() 
    {
        previousButton.onClick.AddListener(OnClickPreviousButton);    
        nextButton.onClick.AddListener(OnClickNextButton);
    }

    [Button]
    void ShowUI()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    [Button]
    void HideUI()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    [Button]
    void OnClickPreviousButton()
    {
        currentImage--;
        if(currentImage < 0)
        {
            currentImage = 9;
        }
        LoadImage();
        SetImageText();
    }

    [Button]
    void OnClickNextButton()
    {
        currentImage++;
        if(currentImage > 9)
        {
            currentImage = 0;
        }
        LoadImage();
        SetImageText();
    }

    void LoadImage()
    {
        Texture2D imageTexture = ImageHelper.LoadImageTexture(currentImage);

        if(imageTexture == null)
        {
            imageTexture = nullImageTexture;
        }

        photo.texture = imageTexture;
    }

    void SetImageText()
    {
        int imageIndex = currentImage+1;
        imageIndexText.text = $" {imageIndex}/10 ";
    }
}
