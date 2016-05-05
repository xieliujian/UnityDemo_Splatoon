Shader "SpraySoldier/Function/Minimap"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTex ("MaskTex", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct VSInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct PSInput
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
			};

			sampler2D _MainTex;
			sampler2D _MaskTex;
			float _TexScale;
			float _TexOffsetX;
			float _TexOffsetY;
			
			PSInput vert (VSInput v)
			{
				PSInput o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 frag (PSInput i) : COLOR
			{
				float2 texoffset = float2(_TexOffsetX, _TexOffsetY) + 0.5;
				float2 texuv = (i.uv  - 0.5) * _TexScale;
				float4 col = tex2D(_MainTex, texuv + texoffset);
				float4 maskColor = tex2D(_MaskTex, i.uv);
				col.a = maskColor.a;
				return col;
			}

			ENDCG
		}
	}
}
