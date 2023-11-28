using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : Singleton<GameOverPanel>
{
    public CanvasGroup canvasGroup;
    Camera tempcam;
    protected override void InitAfterAwake()
    {
        tempcam = transform.parent.GetComponent<Canvas>().worldCamera;
    }

    public void ShowPanelUp()
    {
        tempcam.depth += 1;
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Retry()
    {
        StartCoroutine(GameManager.Instance.ReTryGameScene());
    }

    public void GoMainMenu()
    {
        StartCoroutine(GameManager.Instance.GoToSceneMainMenu());
    }
}
