Shader "Custom/SeparableGaussianBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
        _Radius ("Blur Radius", Float) = 5.0
        _Sigma ("Sigma", Float) = 2.0
    }
    SubShader
    {
        // 수평 블러 Pass
        Pass
        {
            Name "HorizontalBlur"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragH
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // (1/width, 1/height, width, height)
            float _Radius;
            float _Sigma;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 fragH(v2f i) : SV_Target
            {
                int radius = (int)_Radius;
                float4 color = 0;
                float totalWeight = 0;

                // 수평 방향으로만 샘플링
                for (int x = -radius; x <= radius; x++)
                {
                    float dist = (float)x;
                    float weight = exp(- (dist * dist) / (2.0 * _Sigma * _Sigma));
                    float2 offset = float2(x, 0) * _MainTex_TexelSize.xy;
                    color += tex2D(_MainTex, i.uv + offset) * weight;
                    totalWeight += weight;
                }

                return color / totalWeight;
            }
            ENDCG
        }

        // 수직 블러 Pass
        Pass
        {
            Name "VerticalBlur"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragV
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _Radius;
            float _Sigma;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 fragV(v2f i) : SV_Target
            {
                int radius = (int)_Radius;
                float4 color = 0;
                float totalWeight = 0;

                // 수직 방향으로만 샘플링
                for (int y = -radius; y <= radius; y++)
                {
                    float dist = (float)y;
                    float weight = exp(- (dist * dist) / (2.0 * _Sigma * _Sigma));
                    float2 offset = float2(0, y) * _MainTex_TexelSize.xy;
                    color += tex2D(_MainTex, i.uv + offset) * weight;
                    totalWeight += weight;
                }

                return color / totalWeight;
            }
            ENDCG
        }
    }
}
