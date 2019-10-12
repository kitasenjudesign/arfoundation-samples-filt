Shader "effects/loop/LoopColorMask"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _DepthTex ("_DepthTex", 2D) = "white" {}
        _BlurTex ("_BlurTex", 2D) = "white" {}
        _DepthTh("_DepthTh",Range(0,1)) = 0.5
        _Detail("_Detail",Range(0,5)) = 0.5
        
        [Toggle] _Invert("_Invert", Float) = 0

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
            //#include "./noise/SimplexNoise3D.hlsl"
            #include "../util/StencilUV.hlsl"

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

            sampler2D _BlurTex;
            sampler2D _DepthTex;
            sampler2D _StencilTex;
            float _DepthTh;
            float _Detail;
            float _Invert;
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
                float2 stencilUV = GetStencilUV(i.uv);

                fixed4 stencil  = tex2D( _StencilTex, stencilUV );
                fixed4 depth    = tex2D( _DepthTex, stencilUV );

                //mosaic
                //fixed4 col = tex2D(_MainTex,  i.uv );
                fixed4 col = tex2D(_BlurTex,  i.uv );

                //col = 0.5 + 0.5*sin( col * 50 * _DepthTh + _Time.z * 2.0 );
                col = frac( col * 10 + _Time.z * 1.0 );
                
                //マスク
                //col.rgb = lerp( col0.rgb, col.rgb, stencil.r);                
                col.rgb = lerp(
                    lerp( col0.rgb, col.rgb, stencil.r),
                    lerp( col0.rgb, col.rgb, 1-stencil.r),
                    _Invert
                );
                //if( depth.r < _DepthTh ){
                //     col.rgb = col0.rgb;
                //}

                return col;

            }
            ENDCG
        }
    }
}
