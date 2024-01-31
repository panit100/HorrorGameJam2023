Shader "Hidden/Custom/PixelArtFilterV2" {
            HLSLINCLUDE
          
            #pragma fragment fp

            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
            
          
            Texture2D _MainTex;
             TEXTURE2D_SAMPLER2D(_DownScaleFactor, sampler_DownScaleFactor);
            SamplerState point_clamp_sampler;

            float4 fp(VaryingsDefault i) : SV_Target {
                float4 ScaleFactor = SAMPLE_TEXTURE2D(_DownScaleFactor,sampler_DownScaleFactor,i.texcoord);
                float4 col = SAMPLE_TEXTURE2D(_MainTex,point_clamp_sampler,i.texcoord);
                col *= 0.01;
                col += ScaleFactor;
                return col;
            }
            ENDHLSL
    SubShader
    {
        Cull off 
        ZWrite Off
        ZTest Always
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment fp
            ENDHLSL
        }
    }
}