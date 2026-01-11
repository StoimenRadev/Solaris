Shader "Venus/Static_Option1"
{
    Properties 
    {
        _AtmosphereColor ("Atmosphere Color", Color) = (0.1, 0.35, 1.0, 1.0)
        _AtmospherePow ("Atmosphere Power", Range(1.5, 8)) = 2
        _AtmosphereMultiply ("Atmosphere Multiply", Range(1, 3)) = 1.5

        _DiffuseTex("Diffuse", 2D) = "white" {}
        _CloudAndNightTex("Cloud + Night", 2D) = "black" {}

        _CloudSpeed("Cloud Speed", Range(0, 3)) = 0.3
        _CloudAlpha("Cloud Transparency", Range(0, 1)) = 1.0
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
            
            sampler2D _DiffuseTex;
            sampler2D _CloudAndNightTex;

            float4 _AtmosphereColor;
            float _AtmospherePow;
            float _AtmosphereMultiply;
            float _CloudSpeed;
            float _CloudAlpha;

            struct vertexInput 
            {
                float4 pos    : POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
            };

            struct vertexOutput 
            {
                float4 pos      : POSITION;
                float2 uv       : TEXCOORD0;
                half diffuse    : TEXCOORD1;
                half night      : TEXCOORD2;
                half3 atmosphere: TEXCOORD3;
            };

            vertexOutput vert(vertexInput input) 
            {
                vertexOutput output;
                output.pos = UnityObjectToClipPos(input.pos);
                output.uv = input.uv;

                float3 localLightDir = normalize(ObjSpaceLightDir(input.pos));
                output.diffuse = saturate(dot(localLightDir, input.normal) * 1.2);
                output.night = 1 - saturate(output.diffuse * 2);

                half3 viewDir = normalize(ObjSpaceViewDir(input.pos));
                half3 normalDir = input.normal;
                output.atmosphere =
                    output.diffuse *
                    _AtmosphereColor.rgb *
                    pow(1 - saturate(dot(viewDir, normalDir)), _AtmospherePow) *
                    _AtmosphereMultiply;

                return output;
            }

            half4 frag(vertexOutput input) : COLOR
            {
                half3 colorSample = tex2D(_DiffuseTex, input.uv).rgb;

                // ⭐ Rotate clouds over time (asymmetrical)
                float2 cloudUV = input.uv;
                cloudUV.x = frac(cloudUV.x + (_Time.y * _CloudSpeed));

                half3 cloudAndNightSample = tex2D(_CloudAndNightTex, cloudUV).rgb;

                // Cloud sample from red channel
                half cloudSample = cloudAndNightSample.r;
                cloudSample *= _CloudAlpha; // apply transparency

                // Night lights from green + blue channels
                half3 nightSample = cloudAndNightSample.ggb;

                // Combine
                half3 result =
                    (colorSample + cloudSample) * input.diffuse +
                    nightSample * input.night +
                    input.atmosphere;

                return half4(result, 1.0);
            }
        ENDCG
        }
    }
    
    Fallback "Diffuse"
}
