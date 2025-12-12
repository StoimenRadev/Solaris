Shader "Mercury/Static"
{
    Properties 
    {
        _AtmosphereColor ("Atmosphere Color", Color) = (0.1, 0.35, 1.0, 1.0)
        _AtmospherePow ("Atmosphere Power", Range(1.5, 8)) = 2
        _AtmosphereMultiply ("Atmosphere Multiply", Range(1, 3)) = 1.5

        _MainTex ("Mercury Texture", 2D) = "white" {}
    }

    SubShader 
    {
        ZWrite On
        ZTest LEqual

        Pass
        {
        CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert 
            #pragma fragment frag
            
            sampler2D _MainTex;

            float4 _AtmosphereColor;
            float _AtmospherePow;
            float _AtmosphereMultiply;

            struct vertexInput 
            {
                float4 pos    : POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
            };

            struct vertexOutput 
            {
                float4 pos       : POSITION;
                float2 uv        : TEXCOORD0;
                half diffuse     : TEXCOORD1;
                half nightFactor : TEXCOORD2;
                half3 atmosphere : TEXCOORD3;
            };

            vertexOutput vert(vertexInput input) 
            {
                vertexOutput o;
                o.pos = UnityObjectToClipPos(input.pos);
                o.uv = input.uv;

                float3 lightDir = normalize(ObjSpaceLightDir(input.pos));
                o.diffuse = saturate(dot(lightDir, input.normal) * 1.2);

                // Night = inverse of diffuse
                o.nightFactor = 1 - saturate(o.diffuse * 2);

                half3 viewDir = normalize(ObjSpaceViewDir(input.pos));
                half3 normalDir = input.normal;

                o.atmosphere = o.diffuse *
                               _AtmosphereColor.rgb *
                               pow(1 - saturate(dot(viewDir, normalDir)), _AtmospherePow) *
                               _AtmosphereMultiply;

                return o;
            }

            half4 frag(vertexOutput input) : COLOR
            {
                half3 color = tex2D(_MainTex, input.uv).rgb;

                // Simple night shading (darkened texture)
                half3 nightColor = color * 0.015; // very dark

                half3 finalColor =
                    color * input.diffuse +      // Day side
                    nightColor * input.nightFactor + // Night side
                    input.atmosphere;            // Atmosphere

                return half4(finalColor, 1.0);
            }
        ENDCG
        }
    }
    
    Fallback "Diffuse"
}
