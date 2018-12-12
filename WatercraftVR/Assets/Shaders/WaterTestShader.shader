// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WaterTest"
{

	Properties
	{
		_WaterColor("Water Color", color) = (0.8, 0.9, 0.6, 1.0)
		_WaterBase("Water Base", color) = (0.1, 0.19, 0.22, 1.0)
		_NormalTex("Normal", 2D) = "white" {}
		_Shininess("Sininess", float) = 0.1

		[Space(1)]
		_DispTex("Disp Texture", 2D) = "gray" {}
		_MinDist("Min Distance", Range(0.1, 50)) = 10
		_MaxDist("Max Distance", Range(0.1, 50)) = 25
		_TessFactor("Tessellation", Range(1, 50)) = 10
		_Displacement("Displacement", Range(0, 1.0)) = 0.3
	}

	SubShader
	{

		Tags { "RenderType"="Opaque" }

		CGINCLUDE

		#include "Tessellation.cginc"
		#include "UnityCG.cginc"
		#include "AutoLight.cginc"

		uniform fixed4 _LightColor0;

		uniform float _TessFactor;
		uniform float _Displacement;
		uniform float _MinDist;
		uniform float _MaxDist;
		uniform sampler2D _DispTex;
    	uniform float4 _DispTex_TexelSize;

		uniform sampler2D _NormalTex;
		uniform fixed4 _WaterColor;
		uniform fixed4 _WaterBase;

		uniform half _Shininess;

		// // Get random value.
		// half hash(vec2 p)
		// {
		// 	half h = dot(p, half2(127.1, 311.7));   
		// 	return frac(sin(h) * 43758.5453123);
		// }

		// // Get Noise.
		// half noise(half2 p)
		// {
		// 	half2 i = floor(p);
		// 	half2 f = fract(p);

		// 	// u = -2.0f^3 + 3.0f^2
		// 	half2 u = f * f * (3.0 - 2.0 * f);

		// 	// Get each grid vertices.
		// 	// +---+---+
		// 	// | a | b |
		// 	// +---+---+
		// 	// | c | d |
		// 	// +---+---+
		// 	half a = hash(i + half2(0.0,0.0));
		// 	half b = hash(i + half2(1.0,0.0));
		// 	half c = hash(i + half2(0.0,1.0));
		// 	half d = hash(i + half2(1.0,1.0));

		// 	// Interpolate grid parameters with x and y.
		// 	half result = lerp(lerp(a, b, u.x),
		// 						lerp(c, d, u.x), u.y);
		// 	return (2.0 * result) - 1.0;
		// }

		fixed3 getSeaColor(float3 pos, float3 norm, float3 light, float3 view)
		{
			fixed fresnel = saturate(1.0 - dot(norm, -view));
			fresnel = pow(fresnel, 3.0) * 0.05;
//0.01 + 0.09 * pow(1.0 - dot(view, norm), 5);
			fixed3 reflected = ShadeSH9(half4(norm, 1.0));
			fixed3 diffuse = saturate(dot(norm, light)) * _LightColor0;
			fixed3 refracted = _WaterBase + diffuse * _WaterColor * 0.12;
//pow(dot(norm, light) * 0.4 + 0.6, 80.0);//
			return lerp(refracted, reflected, fresnel);
		}

		struct VsInput
		{
			float3 vertex   : POSITION;
			float3 normal   : NORMAL;
			float4 tangent   : TANGENT;
			float2 texcoord : TEXCOORD0;
		};

		struct HsInput
		{
			float4 f4Position : POS;
			float3 f3Normal   : NORMAL;
			float4 f4Tangent  : TANGENT;
			float2 f2TexCoord : TEXCOORD;
		};

		struct HsControlPointOutput
		{
			float3 f3Position : POS;
			float3 f3Normal   : NORMAL;
			float4 f4Tangent  : TANGENT;
			float2 f2TexCoord : TEXCOORD;
		};

		struct HsConstantOutput
		{
			float fTessFactor[3]    : SV_TessFactor;
			float fInsideTessFactor : SV_InsideTessFactor;
		};

		struct DsOutput
		{
			float4 f4Position : SV_Position;
			float2 f2TexCoord : TEXCOORD0;
			float3 f3Normal   : NORMAL;
			float3 viewDir    : TEXCOORD1;
			float3 lightDir   : TEXCOORD2;
			LIGHTING_COORDS(3, 4)  
		};

		HsInput vert(VsInput i)
		{
			HsInput o;
			o.f4Position = float4(i.vertex, 1.0);
			o.f3Normal   = i.normal;
			o.f4Tangent  = i.tangent;
			o.f2TexCoord = i.texcoord;
			return o;
		}

		[domain("tri")]
		[partitioning("integer")]
		[outputtopology("triangle_cw")]
		[patchconstantfunc("hullConst")]
		[outputcontrolpoints(3)]
		HsControlPointOutput hull(InputPatch<HsInput, 3> i, uint id : SV_OutputControlPointID)
		{
			HsControlPointOutput o = (HsControlPointOutput)0;
			o.f3Position = i[id].f4Position.xyz;
			o.f3Normal   = i[id].f3Normal;
			o.f4Tangent  = i[id].f4Tangent;
			o.f2TexCoord = i[id].f2TexCoord;
			return o;
		}

		// HsConstantOutput hullConst(InputPatch<HsInput, 3> i)
		// {
		// 	HsConstantOutput o = (HsConstantOutput)0;
			
		// 	float4 p0 = i[0].f4Position;
		// 	float4 p1 = i[1].f4Position;
		// 	float4 p2 = i[2].f4Position;
		// 	float4 tessFactor = UnityDistanceBasedTess(p0, p1, p2, _MinDist, _MaxDist, _TessFactor);

		// 	o.fTessFactor[0] = tessFactor.x;
		// 	o.fTessFactor[1] = tessFactor.y;
		// 	o.fTessFactor[2] = tessFactor.z;
		// 	o.fInsideTessFactor = tessFactor.w;
				
		// 	return o;
		// }

		HsConstantOutput hullConst(InputPatch<HsInput, 3> i)
		{
			HsConstantOutput o = (HsConstantOutput)0;
			
			float distance = length(float3(UNITY_MATRIX_MV[0][3], UNITY_MATRIX_MV[1][3], UNITY_MATRIX_MV[2][3]));
			float tessFactor = pow(_TessFactor / distance, 2);
			o.fTessFactor[0] = o.fTessFactor[1] = o.fTessFactor[2] = o.fInsideTessFactor = tessFactor;
				
			return o;
		}

		[domain("tri")]
		DsOutput domain(
			HsConstantOutput hsConst, 
			const OutputPatch<HsControlPointOutput, 3> i, 
			float3 bary : SV_DomainLocation)
		{
			DsOutput o = (DsOutput)0;

			float3 f3Position = 
				bary.x * i[0].f3Position + 
				bary.y * i[1].f3Position +
				bary.z * i[2].f3Position;

			float4 f4Tangent = normalize(
				bary.x * i[0].f4Tangent +
				bary.y * i[1].f4Tangent + 
				bary.z * i[2].f4Tangent);

			o.f3Normal = normalize(
				bary.x * i[0].f3Normal +
				bary.y * i[1].f3Normal + 
				bary.z * i[2].f3Normal);

			o.f2TexCoord = 
				bary.x * i[0].f2TexCoord + 
				bary.y * i[1].f2TexCoord + 
				bary.z * i[2].f2TexCoord;

			float disp = (tex2Dlod(_DispTex, float4(o.f2TexCoord, 0, 0)).r + tex2Dlod(_NormalTex, float4(o.f2TexCoord, 0, 0) + _Time.x*0.2).r*2) * _Displacement;
			f3Position.xyz += o.f3Normal * disp;

			o.f4Position = UnityObjectToClipPos(float4(f3Position.xyz, 1.0));
			
			float3 binormal = cross(o.f3Normal, f4Tangent.xyz) * f4Tangent.w;
			float3x3 rotation = float3x3(f4Tangent.xyz, binormal, o.f3Normal);
			o.viewDir  = normalize(ObjSpaceViewDir(float4(f3Position,1.0)));//ObjSpaceViewDir(float4(f3Position, 1.0));//normalize(mul(rotation, ObjSpaceViewDir( float4(f3Position, 1.0))));
           	o.lightDir = normalize(ObjSpaceLightDir(float4(f3Position,1.0)));//ObjSpaceLightDir(float4(f3Position, 1.0));//normalize(mul(rotation,  ObjSpaceLightDir( float4(f3Position, 1.0))));
			//TRANSFER_VERTEX_TO_FRAGMENT(o);

			return o;
		}

		fixed4 frag(DsOutput i) : SV_Target
		{
			float3 duv = float3(_DispTex_TexelSize.xy, 0) * 10;
			half v1 = tex2D(_DispTex, i.f2TexCoord - duv.xz).y;
			half v2 = tex2D(_DispTex, i.f2TexCoord + duv.xz).y;
			half v3 = tex2D(_DispTex, i.f2TexCoord - duv.zy).y;
			half v4 = tex2D(_DispTex, i.f2TexCoord + duv.zy).y;
			half2 uvDiff = half2(v1 - v2, v3 - v4); 
			float3 normal =  normalize(float3(uvDiff, 1.0) -  UnpackNormal(tex2D(_NormalTex, i.f2TexCoord + _Time.x*0.2)));

			fixed3 halfDir = normalize(i.lightDir + i.viewDir);	
			fixed3 color = getSeaColor(i.f4Position.xyz, normal, i.lightDir, i.viewDir);
			UNITY_LIGHT_ATTENUATION(atten, i, i.f4Position);
			color += _WaterColor * (tex2D(_DispTex, i.f2TexCoord).y - 0.6) * 0.18 * atten;
			color += pow(saturate(dot(normal, halfDir)), _Shininess * 128.0); 
			return fixed4(color, 1);
		}

		ENDCG

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma hull hull
			#pragma domain domain
			ENDCG
		}

	}

	Fallback "Unlit/Texture"

}