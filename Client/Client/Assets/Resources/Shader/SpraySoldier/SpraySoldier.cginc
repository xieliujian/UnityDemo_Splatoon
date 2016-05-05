

#ifndef __SpraySoldier_h__
#define __SpraySoldier_h__

#define SS_CALCTANGENTSPACE(i, o) \
	float3 worldPos = mul(_Object2World, i.vertex).xyz;	\
	float3 worldNormal = UnityObjectToWorldNormal(i.normal); \
	float3 worldTangent = UnityObjectToWorldDir(i.tangent.xyz);	\
	float3 worldBinormal = cross(worldNormal, worldTangent) * i.tangent.w;	\
	o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);	\
	o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);	\
	o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);

inline fixed4 SS_UnityBlinnPhongLight1 (SurfaceOutput s, half3 viewDir, UnityLight light, float3 worldNormal, float3 normal)
{
	half3 h = normalize (light.dir + viewDir);
	
	//fixed diff = max (0, dot (s.Normal, light.dir));
	float diff = max (0, dot(normal, light.dir));

	float gloss = 1.2;
    float specPow = exp2( gloss * 10.0 + 1.0);
	
	float nh = max (0, dot (s.Normal, h));
	float spec = pow (nh, s.Specular * specPow) * s.Gloss;
	
	fixed4 c;
	//c.rgb = s.Albedo * light.color * diff + light.color * _SpecColor.rgb * spec;
	c.rgb = s.Albedo * light.color * diff + light.color * spec;
	c.a = s.Alpha;

	return c;
}

inline fixed4 SS_UnityBlinnPhongLight (SurfaceOutput s, half3 viewDir, UnityLight light)
{
	half3 h = normalize (light.dir + viewDir);
	
	fixed diff = max (0, dot (s.Normal, light.dir));
	
	float nh = max (0, dot (s.Normal, h));
	float spec = pow (nh, s.Specular * 128.0) * s.Gloss;
	
	fixed4 c;
	c.rgb = s.Albedo * light.color * diff + light.color * _SpecColor.rgb * spec;
	c.a = s.Alpha;

	return c;
}

inline fixed4 SS_LightingBlinnPhong1 (SurfaceOutput s, half3 viewDir, UnityGI gi, float3 worldNormal, float3 normal)
{
	fixed4 c;
	c = SS_UnityBlinnPhongLight1 (s, viewDir, gi.light, worldNormal, normal);

	#if defined(DIRLIGHTMAP_SEPARATE)
		#ifdef LIGHTMAP_ON
			c += UnityBlinnPhongLight (s, viewDir, gi.light2);
		#endif
		#ifdef DYNAMICLIGHTMAP_ON
			c += UnityBlinnPhongLight (s, viewDir, gi.light3);
		#endif
	#endif

	#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
		c.rgb += s.Albedo * gi.indirect.diffuse;
	#endif

	return c;
}

inline fixed4 SS_LightingBlinnPhong (SurfaceOutput s, half3 viewDir, UnityGI gi)
{
	fixed4 c;
	c = SS_UnityBlinnPhongLight (s, viewDir, gi.light);

	#if defined(DIRLIGHTMAP_SEPARATE)
		#ifdef LIGHTMAP_ON
			c += UnityBlinnPhongLight (s, viewDir, gi.light2);
		#endif
		#ifdef DYNAMICLIGHTMAP_ON
			c += UnityBlinnPhongLight (s, viewDir, gi.light3);
		#endif
	#endif

	#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
		c.rgb += s.Albedo * gi.indirect.diffuse;
	#endif

	return c;
}

inline UnityGI SS_UnityGlobalIllumination (UnityGIInput data, half occlusion, half oneMinusRoughness, half3 normalWorld, bool reflections)
{
	UnityGI o_gi;
	UNITY_INITIALIZE_OUTPUT(UnityGI, o_gi);

	// Explicitly reset all members of UnityGI
	ResetUnityGI(o_gi);

	#if UNITY_SHOULD_SAMPLE_SH
		#if UNITY_SAMPLE_FULL_SH_PER_PIXEL
			half3 sh = ShadeSH9(half4(normalWorld, 1.0));
		#elif (SHADER_TARGET >= 30)
			half3 sh = data.ambient + ShadeSH12Order(half4(normalWorld, 1.0));
		#else
			half3 sh = data.ambient;
		#endif
	
		o_gi.indirect.diffuse += sh;
	#endif

	#if !defined(LIGHTMAP_ON)
		o_gi.light = data.light;
		o_gi.light.color *= data.atten;

	#else
		// Baked lightmaps
		fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, data.lightmapUV.xy); 
		half3 bakedColor = DecodeLightmap(bakedColorTex);
		
		#ifdef DIRLIGHTMAP_OFF
			o_gi.indirect.diffuse = bakedColor;

			#ifdef SHADOWS_SCREEN
				o_gi.indirect.diffuse = MixLightmapWithRealtimeAttenuation (o_gi.indirect.diffuse, data.atten, bakedColorTex);
			#endif // SHADOWS_SCREEN

		#elif DIRLIGHTMAP_COMBINED
			fixed4 bakedDirTex = UNITY_SAMPLE_TEX2D_SAMPLER (unity_LightmapInd, unity_Lightmap, data.lightmapUV.xy);
			o_gi.indirect.diffuse = DecodeDirectionalLightmap (bakedColor, bakedDirTex, normalWorld);

			#ifdef SHADOWS_SCREEN
				o_gi.indirect.diffuse = MixLightmapWithRealtimeAttenuation (o_gi.indirect.diffuse, data.atten, bakedColorTex);
			#endif // SHADOWS_SCREEN

		#elif DIRLIGHTMAP_SEPARATE
			// Left halves of both intensity and direction lightmaps store direct light; right halves - indirect.

			// Direct
			fixed4 bakedDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd, unity_Lightmap, data.lightmapUV.xy);
			o_gi.indirect.diffuse += DecodeDirectionalSpecularLightmap (bakedColor, bakedDirTex, normalWorld, false, 0, o_gi.light);

			// Indirect
			half2 uvIndirect = data.lightmapUV.xy + half2(0.5, 0);
			bakedColor = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, uvIndirect));
			bakedDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd, unity_Lightmap, uvIndirect);
			o_gi.indirect.diffuse += DecodeDirectionalSpecularLightmap (bakedColor, bakedDirTex, normalWorld, false, 0, o_gi.light2);
		#endif
	#endif
	
	#ifdef DYNAMICLIGHTMAP_ON
		// Dynamic lightmaps
		fixed4 realtimeColorTex = UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, data.lightmapUV.zw);
		half3 realtimeColor = DecodeRealtimeLightmap (realtimeColorTex);

		#ifdef DIRLIGHTMAP_OFF
			o_gi.indirect.diffuse += realtimeColor;

		#elif DIRLIGHTMAP_COMBINED
			half4 realtimeDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicDirectionality, unity_DynamicLightmap, data.lightmapUV.zw);
			o_gi.indirect.diffuse += DecodeDirectionalLightmap (realtimeColor, realtimeDirTex, normalWorld);

		#elif DIRLIGHTMAP_SEPARATE
			half4 realtimeDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicDirectionality, unity_DynamicLightmap, data.lightmapUV.zw);
			half4 realtimeNormalTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicNormal, unity_DynamicLightmap, data.lightmapUV.zw);
			o_gi.indirect.diffuse += DecodeDirectionalSpecularLightmap (realtimeColor, realtimeDirTex, normalWorld, true, realtimeNormalTex, o_gi.light3);
		#endif
	#endif
	o_gi.indirect.diffuse *= occlusion;

	if (reflections)
	{
		half3 worldNormal = reflect(-data.worldViewDir, normalWorld);

		#if UNITY_SPECCUBE_BOX_PROJECTION		
			half3 worldNormal0 = BoxProjectedCubemapDirection (worldNormal, data.worldPos, data.probePosition[0], data.boxMin[0], data.boxMax[0]);
		#else
			half3 worldNormal0 = worldNormal;
		#endif

		half3 env0 = Unity_GlossyEnvironment (UNITY_PASS_TEXCUBE(unity_SpecCube0), data.probeHDR[0], worldNormal0, 1-oneMinusRoughness);
		#if UNITY_SPECCUBE_BLENDING
			const float kBlendFactor = 0.99999;
			float blendLerp = data.boxMin[0].w;
			UNITY_BRANCH
			if (blendLerp < kBlendFactor)
			{
				#if UNITY_SPECCUBE_BOX_PROJECTION
					half3 worldNormal1 = BoxProjectedCubemapDirection (worldNormal, data.worldPos, data.probePosition[1], data.boxMin[1], data.boxMax[1]);
				#else
					half3 worldNormal1 = worldNormal;
				#endif

				half3 env1 = Unity_GlossyEnvironment (UNITY_PASS_TEXCUBE(unity_SpecCube1), data.probeHDR[1], worldNormal1, 1-oneMinusRoughness);
				o_gi.indirect.specular = lerp(env1, env0, blendLerp);
			}
			else
			{
				o_gi.indirect.specular = env0;
			}
		#else
			o_gi.indirect.specular = env0;
		#endif
	}

	o_gi.indirect.specular *= occlusion;

	return o_gi;
}

inline void SS_LightingBlinnPhong_GI (SurfaceOutput s, UnityGIInput data, inout UnityGI gi)
{
	gi = SS_UnityGlobalIllumination (data, 1.0, s.Gloss, s.Normal, false);
}

inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
	UNITY_OPAQUE_ALPHA(c.a);
	return c;
}

#endif


