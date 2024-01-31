Shader "Hidden/Custom/CRTv2" {
            HLSLINCLUDE
            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
            float _Curvature;
            float _VignetteWidth;
            Texture2D _MainTex; SamplerState sampler_MainTex;
            TEXTURE2D_SAMPLER2D(_CRTTex, sampler_CRTTex);
            half4 Frag(VaryingsDefault  i) : SV_Target {
                float2 uv = i.texcoord * 2.0f - 1.0f;
                float2 offset = uv.yx / _Curvature;
                uv = uv + uv * offset * offset;
                uv = uv * 0.5f + 0.5f;
               

                half4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,uv);
                if (uv.x <= 0.0f || 1.0f <= uv.x || uv.y <= 0.0f || 1.0f <= uv.y)
                    col = 0;

                uv = uv * 2.0f - 1.0f;
                float2 vignette = _VignetteWidth / _ScreenParams.xy;
                vignette = smoothstep(0.0f, vignette, 1.0f - abs(uv));
                vignette = saturate(vignette);

                col.g *= (sin(i.texcoord.y * _ScreenParams.y * 2.0f) + 1.0f) * 0.15f + 1.0f;
                col.rb *= (cos(i.texcoord.y * _ScreenParams.y * 2.0f) + 1.0f) * 0.135f + 1.0f; 

                return saturate(col) * vignette.x * vignette.y;
            }
            ENDHLSL
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        Pass
        {
              HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}