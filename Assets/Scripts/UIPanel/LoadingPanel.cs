using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : PersistentSingleton<LoadingPanel>
{
    CanvasGroup canvasGroup;

    protected override void InitAfterAwake()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        Close();
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
    }
}
