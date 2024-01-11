Shader "Hidden/VHSPostProcessEffect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_VHSTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
					
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _VHSTex;
			
			float _yScanline;
			float _xScanline;
			float _Intensity;
			float rand(float3 co){
			     return frac(sin( dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
			}
 
			fixed4 frag (v2f_img i) : COLOR{
				half2 tempuv = i.uv;
				fixed4 vhs = tex2D (_VHSTex, i.uv);
			
				float dx = 1-abs(distance(i.uv.y, _xScanline*2));
				float dy = 1-abs(distance(i.uv.y, _yScanline*2));
				
				//float x = ((int)(i.uv.x*320))/320.0;
				dy = ((int)(dy*100))/100.0;
				dy = dy;
				i.uv.x += dy * 0.025 + rand(float3(dy,dy,dy)).r/2;
				
				float white = (vhs.r+vhs.g+vhs.b)/3;
				
				if(dx > 0.99)
					i.uv.y = _xScanline;
				i.uv.y = step(0.99, dy) * (_yScanline) + step(dy, 0.99) * i.uv.y;
				
				i.uv.x = i.uv.x % 100;
				i.uv.y = i.uv.y % 100;
				
				fixed4 c = tex2D (_MainTex, lerp(i.uv,tempuv,_Intensity));
				
				
				
				float bleed = tex2D(_MainTex, i.uv + float2(0.01, 0)).r;
				bleed += tex2D(_MainTex, i.uv + float2(0.02, 0)).r;
				bleed += tex2D(_MainTex, i.uv + float2(0.01, 0.01)).r;
				bleed += tex2D(_MainTex, i.uv + float2(0.02, 0.02)).r;
				bleed /= 2;
				
				if(bleed > 0.1){
					vhs += fixed4(bleed * _xScanline, 0, 0, 0);
				}
				
				float x = ((int)(i.uv.x*320))/320.0;
				float y = ((int)(i.uv.y*240))/240.0;
				
				c -= rand(float3(x, y, _xScanline)) * _xScanline / 5;
				vhs.xyz /= 50;
			
				return c+vhs ;
			}
			ENDCG
		}
	}
Fallback off
}