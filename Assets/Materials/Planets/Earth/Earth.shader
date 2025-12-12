Shader "Earth/Static"
{
   Properties 
    {
        _AtmosphereColor ("Atmosphere Color", Color) = (0.1, 0.35, 1.0, 1.0)
        _AtmospherePow ("Atmosphere Power", Range(1.5, 8)) = 2
        _AtmosphereMultiply ("Atmosphere Multiply", Range(1, 3)) = 1.5

        _DiffuseTex("Diffuse", 2D) = "white" {}
        _CloudAndNightTex("Cloud And Night", 2D) = "black" {}

        _CloudSpeed("Cloud Speed", Range(0, 1)) = 0.2
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
                output.atmosphere = output.diffuse * _AtmosphereColor.rgb * pow(1 - saturate(dot(viewDir, normalDir)), _AtmospherePow) * _AtmosphereMultiply;

                return output;
            }

            half4 frag(vertexOutput input) : COLOR
            {
                half3 colorSample = tex2D(_DiffuseTex, input.uv).rgb;

                // Clouds move slower (3x slower)
                float2 cloudUV = input.uv;
                cloudUV.x += (_Time.y * _CloudSpeed) / 12.0;

                half3 cloudAndNightSample = tex2D(_CloudAndNightTex, cloudUV).rgb;

                // Make clouds white
                half cloudSample = cloudAndNightSample.r;
                cloudSample = cloudSample * 1.0;

                // Night sample
                half3 nightSample = cloudAndNightSample.ggb;

                // Combine everything
                half4 result;
                result.rgb = (colorSample + cloudSample) * input.diffuse + nightSample * input.night + input.atmosphere;
                result.a = 1;

                return result;
            }
        ENDCG
        }
    }
    
    Fallback "Diffuse"
}
