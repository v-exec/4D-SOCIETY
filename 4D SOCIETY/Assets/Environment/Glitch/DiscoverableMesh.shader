Shader "Custom/DiscoverableMesh" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)

		_MainTex ("Texture", 2D) = "white" {}
		_Metallic ("Metallic", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}

		_DiscoveryDistance ("Discovery Distance", Range(0.5, 6)) = 3.0
		_PointCount ("Point Count", Int) = 150
		_NoiseResolution ("Noise Resolution", Range(1, 50)) = 35
		_VisibleForTesting ("Visible (For Testing)", Range(-1, 1)) = 1
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		#include "ClassicNoise3D.hlsl"

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Metallic;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_Metallic;
			float3 worldPos;
		};

		float remap (float value, float from1, float to1, float from2, float to2) {
		    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		fixed4 _Color;

		float _DiscoveryDistance;
		int _PointCount;
		float _Amounts[100];
		fixed3 _Discoveries[100];
		float _NoiseResolution;
		float _VisibleForTesting;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			//sum all relevant points' distance to calculate whether frag should be opaque
			float collectiveOpacity = 0.0;

			for (int i = 0; i < _PointCount; i++) {
				float d = distance(IN.worldPos, _Discoveries[i]);
				if (d < (_DiscoveryDistance * remap(_Amounts[i], 5, 0.1, 0, 1))) {
					collectiveOpacity += remap(d, 0, _DiscoveryDistance, 1, 0);
				}
			}

			//treat the collective opacity to not overpower noise pattern
			collectiveOpacity = remap(collectiveOpacity, 0, _DiscoveryDistance * 2, 0, 1);

			//clip using 3D perlin noise
			float dissolve_value = cnoise(IN.worldPos * _NoiseResolution);
			dissolve_value = remap(dissolve_value, -1, 1, 0, 0.4);
			clip((-dissolve_value * _VisibleForTesting) + collectiveOpacity);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			o.Albedo = c.rgb;
			o.Metallic = tex2D (_Metallic, IN.uv_Metallic).r;
			o.Smoothness = tex2D (_Metallic, IN.uv_Metallic).a;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}