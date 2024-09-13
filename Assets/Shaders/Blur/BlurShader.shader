Shader "Unlit/DistanceToOpaqueWithOptimizedPerformance"
{
    Properties
    {
        _CustomTex ("Custom Texture", 2D) = "white" {} // Custom texture property
        _PixelsPerUnit ("Pixels Per Unit", Float) = 32.0 // Block size property
        _LayerCount ("Layer Count", Int) = 10 // Number of layers
        _LayerColor ("Layer Color", Color) = (1, 1, 1, 1) // Color property for the layers
        _MaxSearchRange ("Max Search Range", Int) = 5 // Max range to search for opaque pixels
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
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

            sampler2D _CustomTex; // Custom texture sampler
            float _PixelsPerUnit; // Number of pixels per unit (block size)
            int _LayerCount; // Number of layers
            float4 _LayerColor; // Color for the layers
            int _MaxSearchRange; // Maximum range for searching opaque pixels

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 texelSize = float2(1.0 / (_ScreenParams.x / _PixelsPerUnit), 1.0 / (_ScreenParams.y / _PixelsPerUnit)); // Treat each block as a single "virtual pixel"
                float4 originalColor = tex2D(_CustomTex, i.uv); // Sample from the custom texture

                // If the pixel is already opaque, return it as is
                if (originalColor.a > 0.0)
                {
                    return originalColor;
                }

                int minDistance = _LayerCount + 1; // Start with a high distance beyond any possible layer

                // Loop through a reduced range of "virtual pixels" around the current pixel
                [loop]
                for (int x = -_MaxSearchRange; x <= _MaxSearchRange; x++) // Limit range to improve performance
                {
                    [loop]
                    for (int y = -_MaxSearchRange; y <= _MaxSearchRange; y++)
                    {
                        if (x == 0 && y == 0) continue; // Skip center pixel

                        float2 offset = float2(x, y) * texelSize;
                        float4 sampleColor = tex2D(_CustomTex, i.uv + offset); // Sample from the custom texture

                        if (sampleColor.a > 0.0) // Found an opaque "virtual pixel"
                        {
                            int distance = max(abs(x), abs(y)); // Manhattan distance

                            // Update the minimum distance if a closer opaque pixel is found
                            minDistance = min(minDistance, distance);
                        }
                    }
                }

                // Calculate the opacity based on the minimum distance
                float opacityStep = 1.0 / float(_LayerCount); // Calculate the opacity reduction per layer
                float opacity = 1.0 - (minDistance * opacityStep); // Calculate the current layer's opacity

                // Clamp the opacity to ensure it stays within [0, 1]
                opacity = saturate(opacity);

                // Apply the chosen color with the calculated opacity
                float4 resultColor = float4(_LayerColor.rgb, opacity); // Set the color to the chosen layer color with the calculated opacity

                return resultColor;
            }
            ENDCG
        }
    }
}
