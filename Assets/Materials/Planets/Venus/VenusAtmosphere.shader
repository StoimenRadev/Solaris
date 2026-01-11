Shader "Atmosphere/CelestialAtmosphere"
{
    Properties
    {
        _Color ("Atmosphere Color", Color) = (0.1, 0.35, 1.0, 1.0)
        _Intensity ("Glow Intensity", float) = 200
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

        Pass
        {
            Blend One One     // additive glow
            ZWrite Off        // do not write depth
            Cull Front        // render backfaces only (outer shell)
        
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            float4 _Color;
            float  _Intensity;

            struct vertexInput
            {
                float4 pos    : POSITION;
                float3 normal : NORMAL;
            };

            struct vertexOutput
            {
                float4 pos      : POSITION;
                float3 normal   : TEXCOORD0;
                float3 viewDir  : TEXCOORD1;
                float  lightDot : TEXCOORD2;
            };

            vertexOutput vert(vertexInput v)
            {
                vertexOutput o;

                o.pos = UnityObjectToClipPos(v.pos);
                o.normal = v.normal;
                o.viewDir = ObjSpaceViewDir(v.pos);

                // Normalize worldspace light direction
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                o.lightDot = saturate(dot(v.normal, lightDir) * 3.0);

                return o;
            }

            float4 frag(vertexOutput i) : COLOR
            {
                float3 viewDir = normalize(i.viewDir);
                float3 normal  = normalize(i.normal);

                // Rim (atmospheric scattering) effect
                float rim = pow(saturate(dot(viewDir, -normal)), 3.0);

                float alpha = rim * _Intensity;

                float3 finalColor = _Color.rgb * i.lightDot * alpha;

                return float4(finalColor, 1.0);
            }

            ENDCG
        }
    }
}
