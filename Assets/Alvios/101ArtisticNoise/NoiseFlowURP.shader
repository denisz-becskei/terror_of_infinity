Shader "Alvios/Noise Flow URP" {
	Properties{
		[HDR] _TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
		[HDR]_BaseColor("Base Color", Color) = (0, 0, 0, 1)
		[MainTexture]_MainTex("Main Texture", 2D) = "white" {}
		[MainTexture]_Distortion("Distortion Texture", 2D) = "white" {}
		_Speed("Distortion Speed", Float) = 1
		_Size("Distortion Size", Float) = 1
	}
		SubShader{
			Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

			Pass {
				HLSLPROGRAM
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

				#pragma vertex vert
				#pragma fragment frag

				#pragma target 3.0
				#pragma multi_compile_instancing

				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f {
					float2 uv : TEXCOORD0;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				sampler2D _Distortion;
				float _Speed;
				float _Size;
				half4 _TintColor, _BaseColor;

				v2f vert(appdata v) {
					v2f o;
					o.uv = v.uv;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					return o;
				}

				float4 frag(v2f i) : SV_Target {
					float4 distort = tex2D(_Distortion, i.uv) * 2 - 1;
					float2 speed = _Speed * _Time.xy;

					float4 output = tex2D(_MainTex, i.uv + distort / 3 * _Size + speed);
					float4 flowMid = tex2D(_MainTex, i.uv - distort / 11 * _Size - speed * 1.37 + float2(0.23, 0.71));

					output.rgb *= flowMid.rgb;

					float4 emission = _TintColor * output;
					float4 albedo = emission + _BaseColor;
					return albedo * UNITY_LIGHTMODEL_AMBIENT;
				}
				ENDHLSL
			}
		}
			FallBack "Diffuse"
}