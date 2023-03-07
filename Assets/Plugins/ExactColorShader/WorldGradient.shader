Shader "ExactColorShader/WorldGradient"
{
    Properties
    {
        _Color1("Color1", Color) = (0,0,0,0)
        _Color2("Color2", Color) = (1,1,1,1)
        _ScaleOffset("ScaleOffset", Vector) = (0,0,0,0)
        _Direction("Direction", Vector) = (0,0,0,0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Back

        Pass
        {
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
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 vertexPos : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
            };

            fixed4 _ScaleOffset; 
            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Direction;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.vertexPos = v.vertex;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f input) : SV_Target
            {
                fixed d = dot(input.worldPos, _Direction);
                fixed t = saturate(d * _ScaleOffset.x + _ScaleOffset.y);

                fixed4 col = lerp(_Color2, _Color1, t);
                return col;
            }
            ENDCG
        }
    }
}