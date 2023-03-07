Shader "ExactColorShader/ToonTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TextureAlphaMultiplier ("Texture Alpha Multiplier", Range(0, 1)) = 1
        [KeywordEnum(Multiply, Add, Replace)] _TextureBlendMode("Texture Blend Mode", Float) = 2
        [Space]

        _ColorLight ("Color Light", Color) = (1,1,1,1)
        _ColorDark ("Color Dark", Color) = (1,1,1,1)

        [Space][Space][Space]
        _RampThreshold ("Ramp Threshold", Range(0,1)) = 0.75
        _RampSmooth ("Ramp Smoothness", Range(0, 2)) = 1

        [Space][Space][Space]
        [HDR]
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,0)
        _SpecularAmount("Specular Amount", Range(0, 1)) = 0.1
        _SpecularHardness("Specular Hardness", Range(0, 1)) = 1
        _SpecularVisibleInShadow("Specular Visible In Shadow", Range(0, 1)) = 1

        [Space][Space][Space]
        [HDR]
        _RimColor("Rim Color", Color) = (1,1,1,0)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.5
        _RimHardness("Rim Hardness", Range(0, 1)) = 1
        _RimInTransparency("Rim In Transparency", Range(0, 1)) = 1

        [Space][Space][Space]
        [MaterialToggle] _UseUnityFog ("Unity Fog", Float) = 0
    }
    SubShader
    {
        Tags
        { 
            "RenderType" = "Transparent"
            "Queue"="Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back

        Pass
        {
            Tags
            {
                "LightMode" = "ForwardBase"
                "PassFlags" = "OnlyDirectional"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
                SHADOW_COORDS(2)
                UNITY_FOG_COORDS(3)
            };

            sampler2D _MainTex;
            half _TextureAlphaMultiplier;
            float4 _MainTex_ST;
            half _TextureBlendMode;
            half4 _ColorLight;
            half4 _ColorDark;
            half _RampSmooth;
            half _RampThreshold;
            half4 _SpecularColor;
            half _SpecularAmount;
            half _SpecularHardness;
            half _SpecularVisibleInShadow;
            half4 _RimColor;
            half _RimAmount;
            half _RimHardness;
            half _RimInTransparency;
            half _UseUnityFog;

            float remap(float inputValue, float rangeInputMin, float rangeInputMax, float rangeOutputMin, float rangeOutputMax)
            {
                return ((inputValue - rangeInputMin) / (rangeInputMax - rangeInputMin)) * (rangeOutputMax - rangeOutputMin) + rangeOutputMin;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half3 viewDir = normalize(i.viewDir);
                half3 normal = normalize(i.worldNormal);
                half NdotL = dot(_WorldSpaceLightPos0, normal);
                half threshold01 = remap(_RampThreshold, 0, 1, -1, 1);
                half NdotLRamp = saturate(smoothstep(threshold01 - _RampSmooth * 0.5, threshold01 + _RampSmooth * 0.5, NdotL));
                half4 baseColor = lerp(_ColorDark, _ColorLight, NdotLRamp); 

                // main texture
                half4 mainTexColor = tex2D(_MainTex, i.uv);

                half4 result = mainTexColor;
                if (_TextureBlendMode == 0)
                {
                    result = mainTexColor * baseColor;
                }
                else if (_TextureBlendMode == 1)
                {
                    half3 color = mainTexColor.rgb + baseColor.rgb;
                    result = half4(color.r, color.g, color.b, mainTexColor.a * baseColor.a);
                }
                else if (_TextureBlendMode == 2)
                {
                    result = lerp(baseColor, mainTexColor, saturate(NdotLRamp + 1 - (_ColorLight.a * _ColorDark.a)));
                }

                //specular
                if (_SpecularColor.a > 0 && _SpecularAmount > 0)
                {
                    half3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                    half NdotH = dot(normal, halfVector);
                    half specularHardness = smoothstep((1 - _SpecularAmount) - 0, (1 - _SpecularAmount) + (_SpecularHardness * _SpecularAmount), NdotH);

                    // color and add specular
                    result = lerp(result, _SpecularColor, specularHardness * _SpecularColor.a);
                }

                //rim light
                if (_RimColor.a > 0 && _RimAmount > 0)
                {
                    half4 rimDot = 1 - dot(viewDir, normal);
                    half4 rimHardness = smoothstep((1 - _RimAmount) - (1 - _RimHardness) * 0.5, (1 - _RimAmount) + (1 - _RimHardness) * 0.5, rimDot);
                    half4 rimWithTransparency = rimHardness * saturate(result.a + _RimInTransparency);
                    result = lerp(result, _RimColor, rimWithTransparency * _RimColor.a);
                }

                if (_UseUnityFog)
                {
                    UNITY_APPLY_FOG(i.fogCoord, result);
                }
                return result * _TextureAlphaMultiplier;
            }
            ENDCG
        }

        // shadow caster rendering pass
        Pass
        {
            Tags {"LightMode"="ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f 
            { 
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}
