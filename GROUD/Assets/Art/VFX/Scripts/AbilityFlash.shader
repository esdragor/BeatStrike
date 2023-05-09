Shader "Hidden/AbilityFlash"
{
    Properties
    {
        _LerpValue ("LerpValue", float) = 0.5
        _MainTex ("Texture", 2D) = "white" {}
        _UVDeform ("UV Deform", 2D) = "white" {}
        _Thresh("Distance Color", float) = 0.5
        _ThresholdValue ("Threshold", float) = 0.5
        _distStr ("Distort Strength", float) = 0.5
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float3 lerp(float3 from, float3 to, float value)
            {
                return from + (to-from)*value;
            }

            sampler2D _MainTex;
            sampler2D _UVDeform;
            float _Thresh;
            float _ThresholdValue;
            float _distStr;
            float _LerpValue;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 uvD = tex2D(_UVDeform, i.uv);
                fixed4 col = tex2D(_MainTex, (i.uv + (uvD.rg*(_distStr*(2*uvD.r*i.uv.y-1)*(_LerpValue)))));
                fixed4 sC = tex2D(_MainTex, i.uv);
                float grayscale = dot(col, float3(0.2126, 0.7152, 0.0722));
                col = (grayscale > _ThresholdValue) ? 1 : 0;
                col.rgb = lerp(col, sC, (1-_LerpValue));
                return col;
            }
            ENDCG
        }
    }
}
