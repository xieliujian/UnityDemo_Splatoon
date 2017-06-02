// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SpraySoldier/Function/CompoundBlocked"
{
	Properties
	{
		_MainTex ("Base(rgb)", 2D) = "white" {}
		_RimColor ("Rim Color", Color) = (0.1, 0.9, 0.1, 0.5)
		_RimPower ("Rim Power", Range(0.2, 10.0)) = 0.2
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent-20" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			ZTest greater
			ZWrite off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float4 _RimColor;
			float _RimPower;
			float _CutValue = 0.3;

			struct VSInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct PSInput
			{
				float4 pos : POSITION;
				float3 normal : TEXCOORD0;
				float3 viewdir : TEXCOORD1;
			};

			PSInput vert(VSInput v)
			{
				PSInput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);
				o.normal = worldNormal;
				o.viewdir = _WorldSpaceCameraPos.xyz - worldPos;

				return o;
			}

			float4 frag(PSInput i) : COLOR
			{
				float3 N = normalize(i.normal);
				float3 V = normalize(i.viewdir);
				float rim = 1.0 - saturate(dot(N, V));

				if (rim < _CutValue)
					discard;

				return _RimColor * pow(rim, _RimPower);
			}

			ENDCG
		}

		Pass
		{
			ZTest less
			ZWrite on

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
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			PSInput vert(VSInput v)
			{
				PSInput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			float4 frag(PSInput i) : COLOR
			{
				return tex2D(_MainTex, i.uv);
			}

			ENDCG
		}
	}
}
