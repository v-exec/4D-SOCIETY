Shader "Leak" {
	Properties {
	 	_MainTex ("", 2D) = "white" {}
	}
	SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        
	 	Pass {
            // distort previous accumulation buffer and overwrite into current accumulation buffer

            // TODO: get a better distortion vector field, either procedural or as a texture

            Blend Off

	  		CGPROGRAM
	  		#pragma vertex vert_img
	  		#pragma fragment frag
	  		#include "UnityCG.cginc"
            #include "ClassicNoise3D.hlsl"

            static const float PI = 3.14159;

	  		uniform sampler2D _MainTex;
            uniform sampler2D _SourceTex; 

            float2 remap(float2 coord) {
                //return coord + 0.01 * float2(sin(coord.x * PI * 5), sin(coord.y * PI * 5));
                return coord + 0.01 * float2(
                    cnoise(float3(5.0 * coord, _Time.y * 0.5)), 
                    cnoise(float3(5.0 * (coord + float2(1.0, 1.0)), _Time.y * 0.5))
                );
            }

	  		fixed4 frag(v2f_img i) : COLOR {
			    return 0.95 * fixed4(tex2D(_MainTex, remap(i.uv)).rgba);
	  		}
	  		ENDCG
	 	}

        Pass {
            // copy camera image over current accumulation buffer with alpha

            // TODO: add sky texture or color input
            
            // original, liquid inside object, preserves background alpha
            // Blend SrcAlpha OneMinusSrcAlpha

            // properly working, liquid trailing off object, does not preserve background alpha
            Blend SrcAlpha One 

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform sampler2D _SourceTex;

            fixed4 frag(v2f_img i) : COLOR {
                return fixed4(tex2D(_SourceTex, i.uv).rgba);
            }
            ENDCG
        }
	}
}