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

            static const int antialiasing = 4;

            uniform sampler2D _MainTex;
            uniform sampler2D _SourceTex;

            float4 _MainTex_TexelSize;

            float2 distort(float2 coord) {
                float2 noise;
                    
                noise = 0.3 * float2(
                    cnoise(float3(8.0 * (coord), _Time.y * 0.5)), 
                    cnoise(float3(8.0 * (coord +  float2(1.0, 1.0)), _Time.y * 0.5))
                );

                noise = 1.8 * unity_DeltaTime.z * float2(
                    cnoise(float3(2.0 * (coord + noise), _Time.y * 0.2)), 
                    cnoise(float3(2.0 * (coord + noise +  float2(1.0, 1.0)), _Time.y * 0.2))
                );

                return noise;
            }

            fixed4 frag(v2f_img i) : COLOR {
            
                float2 delta = distort(i.uv);
                float2 coord = i.uv;
                float norm = length(i.uv);

                fixed4 result = 0;

                for(int count = 0; count < antialiasing; count++) {
                    result += fixed4(tex2D(_MainTex, coord + delta * (count + 1.0 ) / (antialiasing * 1.0)).rgba);
                }

                return result / (1.0 * antialiasing) * exp(- unity_DeltaTime.z * 12.0);
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