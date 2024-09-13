Shader "Hidden/Dither"
{
    Properties
    {
        _DitherPattern("Dither Pattern", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
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
            sampler2D _DitherPattern;
            float4 _MainTex_TexelSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);
                float2 ditherUV = i.uv * _MainTex_TexelSize.xy * 512; // Scale pattern to screen size
                fixed4 dither = tex2D(_DitherPattern, ditherUV);
                color.rgb *= step(dither.r, color.r); // Apply dithering
                return color;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
