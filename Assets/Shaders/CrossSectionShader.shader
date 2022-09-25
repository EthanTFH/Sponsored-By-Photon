Shader "Custom/CrossSectionShader"
{
    Properties

    {

        _Color("Color", Color) = (1,1,1,1)

        _CrossColor("Cross Section Color", Color) = (1,1,1,1)

        _MainTex("Albedo (RGB)", 2D) = "white" {}

        _Glossiness("Smoothness", Range(0,1)) = 0.5

        _Metallic("Metallic", Range(0,1)) = 0.0

        _ClippingPlaneY("ClippingPlaneY", Range(-100.0, 100.0)) = 50.0

        _ClippingPlaneX("ClippingPlaneX", Range(-100.0, 100.0)) = 50.0

        _ClippingPlaneZ("ClippingPlaneZ", Range(-100.0, 100.0)) = 50.0

        _ClippignPlaneXT("ClippingPlaneX 2", Range(-100.0, 100.0)) = 50.0
    }



        SubShader

        {

            Tags { "RenderType" = "Opaque" }



            //------------------------------------------------------------------------------------

            Cull Back

            CGPROGRAM

            #pragma surface surf Standard addshadow

            #pragma target 3.0

            sampler2D _MainTex;

            struct Input

            {

                float2 uv_MainTex;

                float3 worldPos;

            };

            half _Glossiness;

            half _Metallic;

            half _ClippingPlaneY;
            half _ClippingPlaneX;
            half _ClippingPlaneZ;
            half _ClippignPlaneXT;

            fixed4 _Color;

            fixed4 _CrossColor;

            bool checkVisibility(fixed3 worldPos)

            {

                return (worldPos[0] > _ClippingPlaneX || worldPos[0] < _ClippignPlaneXT) && (worldPos[1] > _ClippingPlaneY) && (worldPos[2] > _ClippingPlaneZ);

            }

            void surf(Input IN, inout SurfaceOutputStandard o)

            {

                if (checkVisibility(IN.worldPos))discard;

                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

                o.Albedo = c.rgb;

                o.Metallic = _Metallic;

                o.Smoothness = _Glossiness;

                o.Alpha = c.a;

            }

            ENDCG

            Cull Front

            CGPROGRAM

            #pragma surface surf NoLighting noambient 

            struct Input

            {

                half2 uv_MainTex;

                float3 worldPos;

            };

            sampler2D _MainTex;

            fixed4 _Color;

            fixed4 _CrossColor;

            half _ClippingPlaneY;
            half _ClippingPlaneX;
            half _ClippingPlaneZ;
            half _ClippignPlaneXT;

            bool checkVisibility(fixed3 worldPos)

            {

                return (worldPos[0] > _ClippingPlaneX || worldPos[0] < _ClippignPlaneXT) && (worldPos[1] > _ClippingPlaneY) && (worldPos[2] > _ClippingPlaneZ);

            }

            fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)

            {

                fixed4 c;

                c.rgb = s.Albedo;

                c.a = s.Alpha;

                return c;

            }

            void surf(Input IN, inout SurfaceOutput o)

            {

                if (checkVisibility(IN.worldPos))discard;

                o.Albedo = _CrossColor;

            }

            ENDCG

        }
}
