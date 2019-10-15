Shader "effects/poster/Edge"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Sensitivity ("Sensitivity", float) = 1.0
        _Threshold ("Threshold", float) = 0.0
        _EdgeColor ("Edge Color", COLOR) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #pragma multi_compile _ Sobel
           #pragma multi_compile _ Prewitt
           #pragma multi_compile _ RobertsCross
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float _Sensitivity;
            float _Threshold;
            half4 _EdgeColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 duv = _MainTex_TexelSize.xy;

                half2 uv0 = i.uv + half2(-duv.x, -duv.y);
                half2 uv1 = i.uv + half2(0, -duv.y);
                half2 uv2 = i.uv + half2(duv.x, -duv.y);
                half2 uv3 = i.uv + half2(-duv.x, 0);
                half2 uv4 = i.uv + half2(0, 0);
                half2 uv5 = i.uv + half2(duv.x, 0);
                half2 uv6 = i.uv + half2(-duv.x, duv.y);
                half2 uv7 = i.uv + half2(0, duv.y);
                half2 uv8 = i.uv + half2(duv.x, duv.y);

                half3 col0 = tex2D(_MainTex, uv0);
                half3 col1 = tex2D(_MainTex, uv1);
                half3 col2 = tex2D(_MainTex, uv2);
                half3 col3 = tex2D(_MainTex, uv3);
                half3 col4 = tex2D(_MainTex, uv4);
                half3 col5 = tex2D(_MainTex, uv5);
                half3 col6 = tex2D(_MainTex, uv6);
                half3 col7 = tex2D(_MainTex, uv7);
                half3 col8 = tex2D(_MainTex, uv8);
 
//#ifdef Sobel
                float cgx = col0 * -1 + col2 * 1 + col3 * -2 + col5 * 2 + col6 * -1 + col8 * 1;
                float cgy = col0 * -1 + col1 * -2 + col2 * -1 + col6 * 1 + col7 * 2 + col8 * 1;
                float cg = sqrt(cgx * cgx + cgy * cgy);
//#elif Prewitt
//                float cgx = col0 * -1 + col2 * 1 + col3 * -1 + col5 * 1 + col6 * -1 + col8 * 1;
//                float cgy = col0 * -1 + col1 * -1 + col2 * -1 + col6 * 1 + col7 * 1 + col8 * 1;
//                float cg = sqrt(cgx * cgx + cgy * cgy);
//#elif RobertsCross
//                float3 cg1 = col8 - col0;
//                float3 cg2 = col6 - col2;
//                float cg = sqrt(dot(cg1, cg1) + dot(cg2, cg2));
//#endif
                half4 edge = cg * _Sensitivity;

                return _EdgeColor * step(0.5,edge - _Threshold);
                //return _EdgeColor * saturate(edge - _Threshold);
            }
            ENDCG
        }
    }
}