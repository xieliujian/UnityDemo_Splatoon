
Shader "SpraySoldier/Mobile/StaticObj" 
{
	Properties 
	{
		_Shininess ("Shininess", Range (0.03, 1)) = 1
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		
		Pass
		{
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_fwdbase

			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "SpraySoldier.cginc"

			sampler2D _MainTex;
			sampler2D _BumpMap;
			half _Shininess;

			struct Input 
			{
				float2 uv_MainTex;
			};

			void surf (Input IN, inout SurfaceOutput o) 
			{
				fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = tex.rgb;
				o.Gloss = tex.a;
				o.Alpha = tex.a;
				o.Specular = _Shininess;
				o.Normal = UnpackNormal (tex2D(_BumpMap, IN.uv_MainTex));
			}

			struct VertInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 diffuse : COLOR;
				float2 uv : TEXCOORD0;
				float2 lmap : TEXCOORD1;
			};

			#ifdef LIGHTMAP_OFF
				struct PSInput
				{
					float4 pos : POSITION;
					float4 diffuse : COLOR;
					float2 uv : TEXCOORD0;
					float4 tSpace0 : TEXCOORD1;
					float4 tSpace1 : TEXCOORD2;
					float4 tSpace2 : TEXCOORD3;
					float3 worldViewDir : TEXCOORD4;
					fixed3 vlight : TEXCOORD5; // ambient/SH/vertexlights
					SHADOW_COORDS(6)
					#if SHADER_TARGET >= 30
					  float4 lmap : TEXCOORD7;
					#endif
				};
			#else
				struct PSInput
				{
					float4 pos : POSITION;
					float4 diffuse : COLOR;
					float2 uv : TEXCOORD0;
					float4 tSpace0 : TEXCOORD1;
					float4 tSpace1 : TEXCOORD2;
					float4 tSpace2 : TEXCOORD3;
					float3 worldViewDir : TEXCOORD4;
					float4 lmap : TEXCOORD5;
					SHADOW_COORDS(6)
				};
			#endif

			float4 _MainTex_ST;

			PSInput vert(VertInput v)
			{
				PSInput o;
				UNITY_INITIALIZE_OUTPUT(PSInput, o);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.diffuse = v.diffuse;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				SS_CALCTANGENTSPACE(v, o)

				float3 viewDirForLight = UnityWorldSpaceViewDir(worldPos);
				float3 worldLightDir = UnityWorldSpaceLightDir(worldPos);
				viewDirForLight = normalize(normalize(viewDirForLight) + normalize(worldLightDir));
				o.worldViewDir = viewDirForLight;

				#ifndef LIGHTMAP_OFF
				  o.lmap.xy = v.lmap.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				// SH/ambient and vertex lights
				#ifdef LIGHTMAP_OFF
					#if UNITY_SHOULD_SAMPLE_SH
						float3 shlight = ShadeSH9 (float4(worldNormal,1.0));
						o.vlight = shlight;
					#else
						o.vlight = 0.0;
					#endif

					#ifdef VERTEXLIGHT_ON
						o.vlight += Shade4PointLights (
							unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
							unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
							unity_4LightAtten0, worldPos, worldNormal );
					#endif // VERTEXLIGHT_ON
				#endif // LIGHTMAP_OFF

				TRANSFER_SHADOW(o); // pass shadow coordinates to pixel shader

				return o;
			}

			fixed4  frag (PSInput IN) : COLOR0
			{
				// prepare and unpack data
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT(Input, surfIN);
				
				surfIN.uv_MainTex = IN.uv.xy;
				float3 worldPos = float3(IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w);
				
				#ifndef USING_DIRECTIONAL_LIGHT
					fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				#else
					fixed3 lightDir = _WorldSpaceLightPos0.xyz;
				#endif
				
				fixed3 worldViewDir = normalize(IN.worldViewDir);
				
				#ifdef UNITY_COMPILER_HLSL
					SurfaceOutput o = (SurfaceOutput)0;
				#else
					SurfaceOutput o;
				#endif

				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Specular = 0.0;
				o.Alpha = 0.0;
				o.Gloss = 0.0;

				// call surface function
				surf (surfIN, o);

				// compute lighting & shadowing factor
				UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
				fixed4 c = 0;
				fixed3 worldN;
				worldN.x = dot(IN.tSpace0.xyz, o.Normal);
				worldN.y = dot(IN.tSpace1.xyz, o.Normal);
				worldN.z = dot(IN.tSpace2.xyz, o.Normal);
				o.Normal = worldN;
				#ifdef LIGHTMAP_OFF
					c.rgb += o.Albedo * IN.vlight;
				#endif // LIGHTMAP_OFF

				// lightmaps
				#ifndef LIGHTMAP_OFF
					#ifdef DIRLIGHTMAP_OFF
						// single lightmap
						fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap.xy);
						fixed3 lm = DecodeLightmap (lmtex);
					#elif DIRLIGHTMAP_COMBINED
						// directional lightmaps
						fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap.xy);
						fixed4 lmIndTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd, unity_Lightmap, IN.lmap.xy);
						half3 lm = DecodeDirectionalLightmap (DecodeLightmap(lmtex), lmIndTex, o.Normal);
					#elif DIRLIGHTMAP_SEPARATE
						// directional with specular - no support
						half4 lmtex = 0;
						half3 lm = 0;
					#endif // DIRLIGHTMAP_OFF
				#endif // LIGHTMAP_OFF


				// realtime lighting: call lighting function
				#ifdef LIGHTMAP_OFF
					c += LightingMobileBlinnPhong (o, lightDir, worldViewDir, atten);
				#else
					c.a = o.Alpha;
				#endif

				#ifndef LIGHTMAP_OFF
					// combine lightmaps with realtime shadows
					#ifdef SHADOWS_SCREEN
						#if defined(UNITY_NO_RGBM)
							c.rgb += o.Albedo * min(lm, atten*2);
						#else
							c.rgb += o.Albedo * max(min(lm,(atten*2)*lmtex.rgb), lm*atten);
						#endif
					#else // SHADOWS_SCREEN
						c.rgb += o.Albedo * lm;
					#endif // SHADOWS_SCREEN
				#endif // LIGHTMAP_OFF

				UNITY_OPAQUE_ALPHA(c.a);
				return c;
			}

			ENDCG
		}

	}

	FallBack "SpraySoldier/Function/FallBack"
}

