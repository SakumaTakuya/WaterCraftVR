Shader "Custom/WaterSurfaceShader" 
{
Properties
{
    _Color("Color", color) = (1, 1, 1, 0)
    _Base("Base", color) = (1, 1, 1, 0)
    _DispTex("Disp Texture", 2D) = "gray" {}
    //_Glossiness ("Smoothness", Range(0,1)) = 0.5
   // _Metallic ("Metallic", Range(0,1)) = 0.0
    _MinDist("Min Distance", Range(0.1, 50)) = 10
    _MaxDist("Max Distance", Range(0.1, 50)) = 25
    _TessFactor("Tessellation", Range(1, 100)) = 10
    _Displacement("Displacement", Range(0, 1.0)) = 0.3
    _FresnelConstant("Fresnel Constant", Range(0,1.0)) = 0.9
    _Noise("_Noise", 2D) = "white" {}
	//_WhiteWave("White Wave", float) = 0.1
    //_WaveNoise("Wave Noise", 2D) = "white" {}
}

SubShader
{
    Cull Off
    Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

    CGPROGRAM

    #pragma surface surf StandardSpecular alpha addshadow fullforwardshadows vertex:disp tessellate:tessDistance
    #pragma target 5.0
    #include "Tessellation.cginc"

    uniform float _TessFactor;
    uniform float _Displacement;
    uniform float _MinDist;
    uniform float _MaxDist;
    uniform sampler2D _DispTex;
    uniform float4 _DispTex_TexelSize;
    uniform fixed4 _Color;
    uniform fixed4 _Base;
    uniform fixed _FresnelConstant;
    uniform sampler2D _Noise;
    //uniform half _Glossiness;
    //uniform half _Metallic;

    //uniform float _WhiteWave;
    //uniform sampler2D _WaveNoise;

    struct appdata 
    {
        float4 vertex   : POSITION;
        float4 tangent  : TANGENT;
        float3 normal   : NORMAL;
        float2 texcoord : TEXCOORD0;
        //float2 texcoord2: TEXCOORD1;
    };

    struct Input 
    {
        float2 uv_DispTex;
        float2 uv_Noise;
        float3 worldPos;
    };

    float random (fixed2 p) { 
        return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
    }

    float4 tessDistance(appdata v0, appdata v1, appdata v2) 
    {
        return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, _MinDist, _MaxDist, _TessFactor);
    }

    void disp(inout appdata v)
    {
        float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement * (1.0 + _SinTime.w/10) ;
                //+ UnpackNormal(tex2Dlod(_Noise, float4(v.texcoord.xy*100, 0, 0) + _SinTime.w/10)).r;
        v.vertex.xyz += v.normal * d;
        //v.vertex.y += UnpackNormal(tex2Dlod(_Noise, float4(v.texcoord.xy, 0, 0))).r;
    }

    void surf(Input IN, inout SurfaceOutputStandardSpecular o) 
    {
        fixed4 tex = tex2D(_DispTex, IN.uv_DispTex/100);

        float3 duv = float3(_DispTex_TexelSize.xy, 0) * 10;
        half v1 = tex2D(_DispTex, IN.uv_DispTex - duv.xz).y;
        half v2 = tex2D(_DispTex, IN.uv_DispTex + duv.xz).y;
        half v3 = tex2D(_DispTex, IN.uv_DispTex - duv.zy).y;
        half v4 = tex2D(_DispTex, IN.uv_DispTex + duv.zy).y;
        half2 uvDiff = half2(v1 - v2, v3 - v4); 
        o.Normal = normalize(half3(uvDiff + UnpackNormal(tex2D(_Noise, IN.uv_Noise + _Time.xx)).rg, 0));//lerp(half3(0,1,0) ,),0.5);

        fixed fresnel = saturate(1.0 - dot(o.Normal, normalize(_WorldSpaceCameraPos - IN.worldPos)));//clamp(1.0 - max(dot( o.Normal, -normalize(IN.worldPos - _WorldSpaceCameraPos.xyz)),0),0,1);
	    fresnel = saturate(-_FresnelConstant + (1.0 + _FresnelConstant) * pow(fresnel, 5));

        o.Alpha = lerp(_Base.a, _Color.a, fresnel);
        //o.Specular = fixed3(1,1,1);
        o.Smoothness = 0;
        o.Albedo = lerp(_Base.rgb, (_Color.rgb + ShadeSH9(half4(o.Normal, 1.0)))/2, fresnel);//fixed3(fresnel,fresnel, fresnel); + ShadeSH9(half4(o.Normal, 1.0)))/2
        // o.Albedo *=  tex.r > _WhiteWave * tex2D(_WaveNoise, IN.uv_DispTex * 15).r &&  (tex.r - tex.g) * (tex.r - tex.g) < 0.3 ? 
        //             tex2D(_WaveNoise, IN.uv_DispTex * 20).r * tex.r * 2 : 1; IN.worldPos - _WorldSpaceCameraPos.xyz
        //             /*mul(uvDiff, uvDiff) * (tex.r - tex.g)*/ (_Color.rgb + ShadeSH9(half4(o.Normal, 1.0)))/2
    }

    ENDCG

    }

    FallBack "Diffuse"

}
