
Shader "SpraySoldier/Function/DrawDecal" 
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "black" {}
		_Color ("Color", Color) = (1,1,1,1)
	}

	SubShader 
	{
		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha, One One

		Pass
		{
			CGPROGRAM

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			sampler2D _MainTex;
			float4 _Color;
			float _rotation;
			float4 _CenterPos;

			struct VertInput
			{
				float3 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct PSInput
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			#include "UnityCG.cginc"

			PSInput vert(VertInput i)
			{
				PSInput o;

				float posx = i.vertex.x * 2.0 - 1.0;
				float posy = -(i.vertex.y * 2.0 - 1.0);

				o.position = float4(posx, posy, 0.0, 1.0);
				o.uv = i.uv;
				return o;
			}

			fixed4  frag (PSInput i) : COLOR0
			{
				fixed4 c = tex2D (_MainTex, i.uv);
				c.rgb *= _Color.rgb;
				//c.rgb *= float3(0.310, 0.724, 0.2);
				//c.rgb *= float3(0.149, 0.263, 0.761);
				//c.rgb *= float3(0.075, 0.639, 0.918);
				//c.rgb *= float3(0.639, 0.122, 0.788);

				return c;
			}

			ENDCG
		}


		Pass
		{
			Blend One One

			CGPROGRAM

			// Use shader model 2.0 target, to get nicer looking lighting
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			struct VertInput
			{
				float3 vertex : POSITION;
			};

			struct PSInput
			{
				float4 position : POSITION;
			};

			#include "UnityCG.cginc"

			PSInput vert(VertInput i)
			{
				PSInput o;
				float posx = i.vertex.x * 2.0 - 1.0;
				float posy = -(i.vertex.y * 2.0 - 1.0);

				o.position = float4(posx, posy, 0.0, 1.0);
				return o;
			}

			fixed4  frag (PSInput i) : COLOR0
			{
				return float4(1.0, 1.0, 1.0, 0.0);
			}

			ENDCG
		}

	}

	FallBack Off
}
