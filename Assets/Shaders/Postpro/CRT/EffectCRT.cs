using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

[Serializable]
[PostProcess(typeof(CRTRenderer),PostProcessEvent.AfterStack,"Hidden/Custom/CRTv2")]
public sealed class EffectCRT : PostProcessEffectSettings
{
    [Range(1.0f, 10.0f), Tooltip("The curvature in CRT.")]
   public FloatParameter curvature =  new FloatParameter { value = 1.0f };
   [Range(1.0f, 100.0f), Tooltip("The vignetteWidth in CRT.")]
   public FloatParameter vignetteWidth =  new FloatParameter { value = 30.0f };
   [Tooltip("Displays the Distortion Effects in debug view.")]
   public BoolParameter DebugView = new BoolParameter { value = false };
   
}

internal sealed class CRTRenderer : PostProcessEffectRenderer<EffectCRT>
{
  
    private Shader _distortionShader;
    
    public override void Init()
    {
       
       
        _distortionShader = Shader.Find("Hidden/Custom/CRTv2");
        base.Init();
    }
   
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(_distortionShader);
        sheet.properties.SetFloat("_Curvature", settings.curvature);
        sheet.properties.SetFloat("_VignetteWidth", settings.vignetteWidth);

        
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}