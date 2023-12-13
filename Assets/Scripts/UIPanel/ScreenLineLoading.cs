using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenLineLoading : MonoBehaviour
{
    private Graphic thismat;
    public UnityAction afterevemt { get; set; }
    [Range(0.0f, 1.0f)] public float width;
    // Start is called before the first frame update
    void Start()
    {
        thismat = GetComponent<Graphic>();
        thismat.material = Instantiate(thismat.materialForRendering);
    }
    
    [Button]
    public void FillLine(float width)
    {
        var clampwidth = Mathf.Clamp01(width);
        DOTween.To(() => thismat.material.GetFloat("_LineWidthY"), x => thismat.material.SetFloat("_LineWidthY", x), width, 0.15f).SetEase(Ease.Linear)
            .OnComplete(afterevemt.Invoke);

    }

    public void InstantFill(float width)
    {
        thismat.material.SetFloat("_LineWidthY", width);
    }
}
