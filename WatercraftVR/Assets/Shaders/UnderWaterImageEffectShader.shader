Shader "Hidden/UnderWaterImageEffect"
{
	Properties
	{
        _MainTex("MainTex", 2D) = ""{}
		_Color("Color", Color) = (0,0,1,1)
		_Surface("Surface", float) = 0
		_Bias("Bias", float) = 20
		_MinDepth("Minimum Depth", float) = 0
		_MaxDepth("Maximum Depth", float) = 0.75
	}
	
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

        Tags { "RenderType"="Opaque" }
        LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"



			struct v2f
			{
				float2 uv       : TEXCOORD0;
				float3 viewDir  : TEXCOORD1;
				float4 vertex   : SV_POSITION;
			};

			v2f vert (appdata_tan v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;

				TANGENT_SPACE_ROTATION;
                o.viewDir  = normalize(mul(rotation, ObjSpaceViewDir(v.vertex)));
				return o;
			}
			
			uniform sampler2D _MainTex;
			uniform sampler2D _CameraDepthTexture;

			uniform fixed4 _Color;
			uniform float _Surface;
			uniform float _Bias;
			uniform float _MinDepth;
			uniform float _MaxDepth;
			// uniform float4x4 _CameraTRS;


			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// float3 cameraPos = _WorldSpaceCameraPos.xyz;
				
				// //if(cameraPos.y > 0) return col;

				// float nearClip = _ProjectionParams.y;
				// float farClip = _ProjectionParams.z;


				half rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
        		half depth = Linear01Depth(rawDepth);
				// half far = lerp(nearClip, farClip, depth);
				
				// float2 uv_norm = i.uv * 2 - 1;
				// float3 viewDir = normalize(float3(uv_norm,-(uv_norm.x * uv_norm.x + uv_norm.y * uv_norm.y) + 1));

				// float3 pixPos = cameraPos + mul(_CameraTRS, float4(0,0,1,1)) * far;
				if(_WorldSpaceCameraPos.y < _Surface && depth < 0.99)
				{
					col = lerp(col ,_Color, clamp(depth * _Bias , _MinDepth, _MaxDepth));
				}
				
        		return col;
			}
			ENDCG
		}
	}
}
