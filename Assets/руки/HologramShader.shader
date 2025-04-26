Shader "Custom/HologramShader"
{
    Properties
    {
        _Color ("Color", Color) = (0, 1, 1, 1)
        _Speed ("Pulse Speed", Float) = 2.0
        _Intensity ("Pulse Intensity", Float) = 0.5
        _Alpha ("Alpha", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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

            fixed4 _Color;
            float _Speed;
            float _Intensity;
            float _Alpha;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float pulse = sin(_Time.y * _Speed) * _Intensity + (1 - _Intensity);
                return fixed4(_Color.rgb * pulse, _Alpha * pulse);
            }
            ENDCG
        }
    }
}
