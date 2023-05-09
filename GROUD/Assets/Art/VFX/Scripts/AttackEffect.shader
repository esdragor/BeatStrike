Shader "Hidden/AttackEffect"
{
    Properties
    {
        _LerpValue ("LerpValue", Range(0,1)) = 0
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,0,0)
        _DistortTex ("Distort Texture", 2D) = "" {}
        _DistortStr ("DistortStrength", Range(0,1)) = 0
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
            float lerp(float from, float to, float value)
            {
                return from + (to-from)*value;
            }
            float3 lerp3(float3 from, float3 to, float value)
            {
                return from + (to-from)*value;
            }
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            
            sampler2D _MainTex;
            sampler2D _DistortTex;
            float _LerpValue;
            float _DistortStr;
            float3 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 distCol = tex2D(_DistortTex, i.uv);
                fixed4 st = tex2D(_MainTex, i.uv + (distCol.rg*_DistortStr));
                st = st + tex2D(_MainTex, i.uv - (distCol.rg*_DistortStr));
                fixed4 col = tex2D(_MainTex, i.uv);
                st.rgb = (st.r *0.3 + st.g * 0.59 + st.b *0.11)*_Color;
                col.rgb = lerp3(st,col,_LerpValue);
                return col;
            }
            ENDCG
        }
    }
}
