Shader "Unlit/DiscoverableMesh" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_DiscoveryDistance ("DiscoveryDistance", Range(0.5, 6)) = 2.0
		_PointCount ("PointCount", Int) = 50
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		float remap (float value, float from1, float to1, float from2, float to2) {
		    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		float _Glossiness;
		float _Metallic;
		fixed4 _Color;

		sampler2D _DissolveTexture;
		float _DiscoveryDistance;
		int _PointCount;
		float _Amounts[50];
		fixed3 _Discoveries[50];

		void surf (Input IN, inout SurfaceOutputStandard o) {

			//sum all relevant points' distance to calculate whether frag should be opaque
			float collectiveOpacity = 0.0;

			for (int i = 0; i < _PointCount; i++) {
				float d = distance(IN.worldPos, _Discoveries[i]);
				if (d < (_DiscoveryDistance * remap(_Amounts[i], 5, 0.1, 0, 1))) {
					collectiveOpacity += remap(d, 0, _DiscoveryDistance, 1, 0);
				}
			}

			//treat the collective opacity to retain noise pattern
			collectiveOpacity = remap(collectiveOpacity, 0, _DiscoveryDistance * 2, 0, 1);

			//clip
			float dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).r;
			clip(-dissolve_value + collectiveOpacity);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}