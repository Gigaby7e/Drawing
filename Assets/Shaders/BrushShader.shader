Shader "Custom/BrushShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BrushTex ("Brush (RGB)", 2D) = "white" {}
        _BrushPos ("Brush Position", Vector) = (0, 0, 0, 0)
        _BrushSize ("Brush Size", Float) = 0.1
        _BrushColor ("Brush Color", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _BrushTex;
            float4 _BrushPos;
            float _BrushSize;
            float4 _BrushColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 brushUV = (uv - _BrushPos.xy) / _BrushSize + 0.5;
                fixed4 brushColor = tex2D(_BrushTex, brushUV) * _BrushColor;

                if (length(brushUV - 0.5) <= 0.5)
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    return lerp(col, brushColor, brushColor.a);
                }
                else
                {
                    return tex2D(_MainTex, i.uv);
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
