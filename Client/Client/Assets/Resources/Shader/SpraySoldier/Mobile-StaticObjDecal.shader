
Shader "SpraySoldier/Mobile/StaticObjDecal" 
{
	Properties 
	{
		_Shininess ("Shininess", Range (0.03, 1)) = 1
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}

		_DecalTex ("DecalTex", 2D) = "black" {}
		_DecalBump("DecalBump", 2D) = "bump" {}
		_DecalSky ("DecalSky", Cube) = "_Skybox" {}
		_DecalNormalInter ("DecalNormalInter", Range(0, 1)) = 0.33
		_DecalSpecular ("DecalSpecular", Float ) = 1.0
        _DecalGloss ("DecalGloss", Float ) = 1.0
        _DecalSpecAmbient ("DecalSpecAmbient", Float ) = 1.0
        _DecalDiffAmbient ("DecalDiffAmbient", Color) = (0.4338235, 0.4338235, 0.4338235, 1.0)
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

			sampler2D _DecalTex;
			sampler2D _DecalBump;
			samplerCUBE _DecalSky;
			float _DecalNormalInter;
			float _DecalSpecular;
			float _DecalGloss;
			float _DecalSpecAmbient;
			float4 _DecalDiffAmbient;

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
					float2 decaluv : TEXCOORD1;
					float4 tSpace0 : TEXCOORD2;
					float4 tSpace1 : TEXCOORD3;
					float4 tSpace2 : TEXCOORD4;
					float3 worldViewDir : TEXCOORD5;
					fixed3 vlight : TEXCOORD6; // ambient/SH/vertexlights
					SHADOW_COORDS(7)
					#if SHADER_TARGET >= 30
					  float4 lmap : TEXCOORD8;
					#endif
				};
			#else
				struct PSInput
				{
					float4 pos : POSITION;
					float4 diffuse : COLOR;
					float2 uv : TEXCOORD0;
					float2 decaluv : TEXCOORD1;
					float4 tSpace0 : TEXCOORD2;
					float4 tSpace1 : TEXCOORD3;
					float4 tSpace2 : TEXCOORD4;
					float3 worldViewDir : TEXCOORD5;
					float4 lmap : TEXCOORD6;
					SHADOW_COORDS(7)
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
				o.decaluv = v.lmap;

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
				// *******************************
				// ******计算没有贴花的部分******
				// *******************************

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

				// *******************************
				// ******计算有贴花的部分******
				// *******************************
				float4 decalcolor = tex2D(_DecalTex, IN.decaluv);

				// normal :
				float2 decalbumpuv = IN.uv.xy * 0.1;
				float3 normalLocal = lerp(float3(0.0, 0.0, 1.0), UnpackNormal(tex2D(_DecalBump, decalbumpuv)).rgb, _DecalNormalInter);

				worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));

				float3 normalDirection;
				normalDirection.x = dot(IN.tSpace0.xyz, normalLocal);
				normalDirection.y = dot(IN.tSpace1.xyz, normalLocal);
				normalDirection.z = dot(IN.tSpace2.xyz, normalLocal);
				normalDirection = normalize(normalDirection);

				float3 viewReflectDirection = reflect( -worldViewDir, normalDirection );
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(worldViewDir + lightDirection);

				// lighting :
				float attenuation = LIGHT_ATTENUATION(IN);

				#ifdef LIGHTMAP_OFF
					float3 attenColor = attenuation * _LightColor0.xyz;
				#endif

				#ifndef LIGHTMAP_OFF
					float3 attenColor = lm;
				#endif

				// diffuse :
				float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb;

				// gloss :
				float gloss = _DecalGloss;
                float specPow = exp2( gloss * 10.0 + 1.0);

				// specular :
				NdotL = max(0.0, NdotL);
                float4 node_74 = texCUBE(_DecalSky, viewReflectDirection);
                float3 specularColor = float3(_DecalSpecular, _DecalSpecular, _DecalSpecular);
                float3 specularAmb = (node_74.rgb * node_74.a * _DecalSpecAmbient) * specularColor;
                
				#ifdef LIGHTMAP_OFF
					float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0, dot(halfDirection, normalDirection)), specPow) * specularColor + specularAmb;
				#endif

				#ifndef LIGHTMAP_OFF
					float3 specular = floor(attenuation) * pow(max(0, dot(halfDirection, normalDirection)), specPow) * specularColor + specularAmb;
					specular *= lm;
				#endif

				float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                diffuseLight += _DecalDiffAmbient.rgb; // Diffuse Ambient Light
                finalColor += diffuseLight * decalcolor.rgb;
                finalColor += specular;

				c.rgb = finalColor.rgb * decalcolor.a + (1 - decalcolor.a) * c.rgb;

				UNITY_OPAQUE_ALPHA(c.a);
				return c;
			}

			ENDCG
		}

	}

	FallBack "SpraySoldier/Function/DecalFallBack"
}
