Shader "Custom/HologramShader"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0,1,1,1)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Transparency ("Transparency", Range(0,1)) = 0.5
        _ScrollSpeed ("Noise Scroll Speed", Float) = 1.0
        _ScanlineStrength ("Scanline Strength", Range(0,1)) = 0.2
        _ScanlineDensity ("Scanline Density", Float) = 1000.0
        _GlowIntensity ("Glow Intensity", Float) = 2.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _NoiseTex;
            fixed4 _MainColor;
            float _Transparency;
            float _ScrollSpeed;
            float _ScanlineStrength;
            float _ScanlineDensity;
            float _GlowIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Двигаем шум
                float2 movingUV = uv;
                movingUV.y += _Time.y * _ScrollSpeed;
                float noise = tex2D(_NoiseTex, movingUV).r;

                // Сканлайны
                float scanline = sin((uv.y + _Time.y * 0.5) * _ScanlineDensity) * _ScanlineStrength;

                // Финальный цвет
                fixed4 col = _MainColor;
                col.rgb *= _GlowIntensity; // свечение усиливаем!
                col.a = (noise + scanline) * (1.0 - _Transparency);

                return col;
            }
            ENDCG
        }
    }
}
