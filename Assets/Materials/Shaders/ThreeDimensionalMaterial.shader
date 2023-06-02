Shader "Custom/ThreeDimensionalMaterial" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
        // Physically based Lambert lighting model, and enable shadows on all light types
        #pragma surface surf Lambert fullforwardshadows

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            // Apply the texture to the material
            fixed4 col = tex2D(_MainTex, IN.uv_MainTex);

            // Calculate the final color
            o.Albedo = col.rgb * _Color.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
        FallBack "Diffuse"
}