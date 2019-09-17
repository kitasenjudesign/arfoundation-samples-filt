Shader "effects/bloom/WhiteBloom"
{
    Properties
    {
        [KeywordEnum(POSTEFFECT,QUAD)]
        _VERT("VERT KEYWORD", Float) = 0
        _MainTex ("_MainTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _DepthTex ("_DepthTex", 2D) = "white" {}
        _DepthTh("_DepthTh",Range(0,1)) = 0.5
        _Brightness("_Brightness",Float) = 1
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
            #include "../util/StencilUV.hlsl"
            #include "../util/FullScreen.hlsl"
            #pragma multi_compile _VERT_POSTEFFECT _VERT_QUAD

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
            float _Invert;
            float _Brightness;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                //POSTエフェクトに使うか、フルスクリーン用quadに使うか
                #ifdef _VERT_POSTEFFECT
                o.vertex = UnityObjectToClipPos(v.vertex);
                #elif _VERT_QUAD
                o.vertex = GetFullScreenVert( v.vertex );
                #endif

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);
                float2 mosaicUV = round(i.uv*aspect*20)/(aspect*20);

                float nn = 3+2*sin(_Time.y*30);
                fixed4 col = tex2D(_MainTex,i.uv) * fixed4(nn,nn,nn,1.0);
                fixed4 col0 = tex2D(_MainTex,i.uv) * fixed4(0.9,0.9,0.9,1.0);

                //i.uv.x = 1 - i.uv.x;
                float2 stencilUV = GetStencilUV(i.uv);

                fixed4 stencil  = tex2D( _StencilTex, stencilUV );
                //fixed4 depth    = tex2D( _DepthTex, stencilUV );

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

                return col * _Brightness;

            }
            ENDCG
        }
    }
}
