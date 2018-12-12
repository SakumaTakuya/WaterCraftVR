Shader "Simulation/WaterSimulationShader"
{
	Properties
	{
		_Constant("Constant Value", float) = 1.999
		_Attenution("Attenution", float) = 0.994
		_Stride("Delta UV", float) = 3.0
		_Collision("Collision Texture", 2D) = "white"{}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "Update"
			CGPROGRAM
			#pragma vertex CustomRenderTextureVertexShader
			#pragma fragment frag
			#include "UnityCustomRenderTexture.cginc"

			uniform float _Constant;
			uniform float _Attenution;
			uniform float _Stride;

			uniform sampler2D _Collision;

			float4 frag(v2f_customrendertexture i) : SV_Target
			{
				float stride = _Stride;
				float2 uv = i.globalTexcoord;

				float du = 1.0 / _CustomRenderTextureWidth;
				float dv = 1.0 / _CustomRenderTextureHeight;
				// float3 duv = float3(1,0,1);
				float3 duv = float3(du, 0.0, dv);

				float2  col = tex2D(_SelfTexture2D, uv);

				float px = tex2D(_SelfTexture2D, uv + duv.xy).r;
				float nx = tex2D(_SelfTexture2D, uv - duv.xy).r;
				float pz = tex2D(_SelfTexture2D, uv + duv.yz).r;
				float nz = tex2D(_SelfTexture2D, uv - duv.yz).r;
				
				float prev = col.g;
				float curr = col.r;
				float next = 2 * curr - prev + _Constant * (
					lerp(col.r, px, stride) +
					lerp(col.r, nx, stride) +
					lerp(col.r, pz, stride) +
					lerp(col.r, nz, stride) -
					4 * curr
				);
				next += tex2D(_Collision,1-uv);
				next *= _Attenution;
				// 

				return float4(next, curr, 0.0, 0.0);
			}

			ENDCG
		}
	}
}
