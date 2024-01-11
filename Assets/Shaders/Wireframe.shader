Shader "Custom/Geometry/Wireframe" //https://gist.github.com/baba-s/14ecab2df06188a083e66ab00b2c9499
{
    Properties
    {
        [PowerSlider(3.0)]
        _WireframeVal ("Wireframe width", Range(0., 0.5)) = 0.05
        _WireframeColor ("Wireframe Color", Color) = (0, 0, 0)
        _WireframeSmoothing ("Wireframe Smoothing", Range(0, 10)) = 1
		_WireframeThickness ("Wireframe Thickness", Range(0, 10)) = 1
        _BaseColor ("Base Color", Color) = (0,0,0,1)
        [Toggle] _RemoveDiag("Remove diagonals?", Float) = 0.
    }
    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        

        Pass
        {
            Cull Back
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            // Change "shader_feature" with "pragma_compile" if you want set this keyword from c# code
             #pragma shader_feature __ _REMOVEDIAG_ON

            #include "UnityCG.cginc"

            struct v2g {
                float4 worldPos : SV_POSITION;
                float2 uv : TEXCOORD2;
            };

            struct g2f {
                float4 pos : SV_POSITION;
                float4 bary : TEXCOORD0;
            };
            
            v2g vert(appdata_base v) {
                v2g o;
               o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
             float _WireframeVal;
            float4 _WireframeColor,_BaseColor;
            float _WireframeSmoothing;
            float _WireframeThickness;
            
            [maxvertexcount(3)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
                float4 param = float4(0.,0.,0.,0.);

                #if _REMOVEDIAG_ON
                float EdgeA = length(IN[0].worldPos - IN[1].worldPos);
                float EdgeB = length(IN[1].worldPos - IN[2].worldPos);
                float EdgeC = length(IN[2].worldPos - IN[0].worldPos);

                if(EdgeA > EdgeB && EdgeA > EdgeC)
                    param.y = 1.;
                else if (EdgeB > EdgeC && EdgeB > EdgeA)
                    param.x = 1.;
                else
                    param.z = 1.;
                #endif

                g2f o;
                o.pos = mul(UNITY_MATRIX_VP, IN[0].worldPos);
                o.bary = float4(1., 0., 0.,0.) + param;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[1].worldPos);
                o.bary = float4(0., 0., 1.,0.) + param;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[2].worldPos);
                o.bary = float4(0., 1., 0.,0.) + param;
                triStream.Append(o);
            }
            float filteredGrid( in float3 uv, in float2 dpdx, in float2 dpdy )
            {
                const float N = 10.0;
                float2 w = max(abs(dpdx), abs(dpdy));
                float2 a = uv + 0.5*w;                        
                float2 b = uv - 0.5*w;           
                float2 i = (floor(a)+min(frac(a)*N,1.0)-
                          floor(b)-min(frac(b)*N,1.0))/(N*w);
                return (1.0-i.x)*(1.0-i.y);
            }
            
            half4 frag(g2f i ) : SV_Target
            {
           //i.bary.z = 1 - i.bary.x - i.bary.y;
	        float4 deltas = fwidth( i.bary);
                float4 smoothing = deltas * _WireframeSmoothing;
	float4 thickness = deltas * _WireframeThickness;
            i.bary = smoothstep(thickness, thickness + smoothing, i.bary);
                float minBary = min(i.bary.x, min(i.bary.y, i.bary.z));
            if(!any(bool3(i.bary.xyz <= _WireframeThickness)))
            {
               
                _WireframeColor = 0;
            }
                
                return lerp(_WireframeColor,_BaseColor,minBary);
            }
            ENDHLSL
        }
    }
}