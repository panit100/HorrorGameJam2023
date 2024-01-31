using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PixelArtFilterRender),PostProcessEvent.BeforeStack,"Hidden/Custom/PixelArtFilterV2")]
public sealed class PixelArtFilterEffect : PostProcessEffectSettings
{
    [Range(0, 8), Tooltip("The down-scale factor to apply to the generated texture.")]
    public IntParameter DownScaleFactor = new IntParameter { value = 0 };
}

internal sealed class PixelArtFilterRender : PostProcessEffectRenderer<PixelArtFilterEffect>
{
    private Shader PixelArtFilter;
    private int propertyID;
    public override DepthTextureMode GetCameraFlags()
    {
        return DepthTextureMode.Depth;
    }
    public override void Init()
    {
        PixelArtFilter = Shader.Find("Hidden/Custom/PixelArtFilterV2");
        propertyID = Shader.PropertyToID("_DownScaleFactor");
        base.Init();
    }

    public override void Render(PostProcessRenderContext context)
    {
         var sheet = context.propertySheets.Get(PixelArtFilter);
         int temp = Shader.PropertyToID("_DownScaleFactor");
       // context.command.GetTemporaryRT(temp, -1, -1, 0, FilterMode.Bilinear);
        // var currentSource = context.source;
        context.command.GetTemporaryRT(temp,
            context.camera.pixelWidth >> settings.DownScaleFactor,
            context.camera.pixelHeight >> settings.DownScaleFactor,
            0, FilterMode.Point, RenderTextureFormat.Default);
         context.command.SetRenderTarget(temp);
        // context.command.ClearRenderTarget(false, true, Color.clear);
         context.command.BlitFullscreenTriangle(context.source,temp);
         context.command.BlitFullscreenTriangle(temp,context.destination,sheet,0);
         context.command.ReleaseTemporaryRT(temp);
    }

    public override void Release()
    {
        base.Release();
    }
}
