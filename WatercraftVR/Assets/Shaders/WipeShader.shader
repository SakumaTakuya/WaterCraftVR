Shader "Hidden/WipeEffect" 
{
	Properties
	{
		_Radius("Radius", Range(0,2))=2
		[HideInInspector]_MainTex("Main Tex", 2D) ="white"{}
		// _Texture("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}

    SubShader 
	{
        Pass 
		{
            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert_img
            #pragma fragment frag

			uniform sampler2D _MainTex;
            uniform float _Radius;
			uniform fixed4 _Color;

            fixed4 frag(v2f_img i) : COLOR 
			{
				fixed2 pos = i.uv - fixed2(0.5, 0.5);
				pos.x *= _ScreenParams.x / _ScreenParams.y;
				if( distance(pos, fixed2(0,0)) < _Radius )
				{
					return tex2D(_MainTex, i.uv);
				}
				return _Color;
            }
            ENDCG
        }
    }
}