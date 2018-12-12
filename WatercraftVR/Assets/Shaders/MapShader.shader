// Shader "Hidden/MapSahder"
// {
// 	Properties
// 	{
// 		_MainTex ("Texture", 2D) = "white" {}
// 		_Map ("Map", 2D) = "white" {}
// 	}
// 	SubShader
// 	{
// 		// No culling or depth
// 		Cull Off ZWrite Off ZTest Always

// 		Pass
// 		{
// 			CGPROGRAM
// 			#pragma vertex vert
// 			#pragma fragment frag
			
// 			#include "UnityCG.cginc"

// 			struct appdata
// 			{
// 				float4 vertex : POSITION;
// 				float2 uv : TEXCOORD0;
// 			};

// 			struct v2f
// 			{
// 				float2 uv : TEXCOORD0;
// 				float4 vertex : SV_POSITION;
// 			};

// 			v2f vert (appdata v)
// 			{
// 				v2f o;
// 				o.vertex = UnityObjectToClipPos(v.vertex);
// 				o.uv = v.uv;
// 				return o;
// 			}
			
// 			uniform sampler2D _MainTex;
// 			uniform sampler2D _Map;

// 			fixed4 frag (v2f i) : SV_Target
// 			{
// 				fixed4 col = tex2D(_MainTex, i.uv);
// 				fixed sum = col.r + col.g + col.b;
// 				if(sum < 0.01)
// 				{
// 					col = tex2D(_Map, i.uv);
// 				}

// 				return col;
// 			}
// 			ENDCG
// 		}
// 	}
// }

Shader "UI/Map"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Map ("Map", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)

		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};

			fixed4 _Color;
			float4 _ClipRect;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;

				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0) * float2(-1,1) * OUT.vertex.w;
				#endif

				OUT.color = IN.color * _Color;
				return OUT;
			}

			uniform sampler2D _MainTex;
			uniform sampler2D _Map;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = tex2D(_MainTex, IN.texcoord);
				color.a *= IN.color.r;
				if(color.r < 0.01)
				{
					color = 0;
				}
				else
				{
					color.rgb = 1;
				}
				
				color += tex2D(_Map, IN.texcoord);
				// color.rgb += IN.color.rgb;
				
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
}