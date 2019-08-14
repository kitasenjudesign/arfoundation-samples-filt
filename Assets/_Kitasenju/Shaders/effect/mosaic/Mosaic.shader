Shader "Effects/Mosaic"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        [Toggle] _Revert("_Revert", Float) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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
            sampler2D _StencilTex;
            float _Revert;

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);
                float2 mosaicUV = round(i.uv*aspect*40)/(aspect*40);

                fixed4 col = tex2D(_MainTex, mosaicUV);//モザイク画像
                fixed4 col0 = tex2D(_MainTex,i.uv);//元画像

                float2 stencilUV = i.uv;
                stencilUV.y = 1 - stencilUV.y;

                //謎マジックナンバー bai
                float bai = 9.0/12.0 * 0.8;//4;3 16;12 16;9
                stencilUV.y = stencilUV.y*bai + (1-bai)/2;

                fixed4 stencil = tex2D(_StencilTex, stencilUV);

                //マスクを反転したいとき
                col.rgb = lerp(
                    lerp( col0.rgb, col.rgb, stencil.r),
                    lerp( col0.rgb, col.rgb, 1 - stencil.r),
                    _Revert
                );
                
                return col;

            }
            ENDCG
        }
    }
}
