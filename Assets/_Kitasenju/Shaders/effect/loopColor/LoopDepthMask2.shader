﻿Shader "effects/loop/LoopDepthMask2"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _DepthTex ("_DepthTex", 2D) = "white" {}
        _DepthTh("_DepthTh",Range(0,1)) = 0.5
        _Detail("_Detail",Range(0,5)) = 0.5
        //[Toggle] _Revert("_Revert", Float) = 0

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
            #include "../noise/SimplexNoise3D.hlsl"

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
            sampler2D _DepthTex;
            sampler2D _StencilTex;
            float _DepthTh;
            float _Detail;
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
                // sample the texture
                
                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);
                
                //float2 mosaicUV = round(i.uv*aspect*40)/(aspect*40);
                //noiseuv



                fixed4 col0 = tex2D(_MainTex,i.uv);

                //i.uv.x = 1 - i.uv.x;
                float2 stencilUV = i.uv;
                stencilUV.y = 1 - stencilUV.y;

                float bai = 9.0/12.0 * 0.8;//4;3 16;12 16;9
                stencilUV.y = stencilUV.y*bai + (1-bai)/2;

                fixed4 stencil  = tex2D( _StencilTex, stencilUV );
                fixed4 depth    = tex2D( _DepthTex, stencilUV );

                //mosaic
                fixed4 col = tex2D(_MainTex,  i.uv );
                col = 0.5 + 0.5*sin( col * 10 * _DepthTh );
                col = lerp(col0, lerp(col,col0,0.2), step( frac( (col.x+col.y+col.z) * 0.333 + i.uv.y + _Time.z ), 0.4 ));



                //マスク
                col.rgb = lerp( col0.rgb, col.rgb, stencil.r);                

                //if( depth.r < _DepthTh ){
                //     col.rgb = col0.rgb;
                //}

                return col;

            }
            ENDCG
        }
    }
}
