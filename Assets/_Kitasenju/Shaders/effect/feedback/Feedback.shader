Shader "effects/feedback/Feedback"
{
    Properties
    {
        [KeywordEnum(POSTEFFECT,QUAD)]
        _VERT("VERT KEYWORD", Float) = 0
        _MainTex ("_MainTex", 2D) = "white" {}
        _MainTex2 ("_MainTex2", 2D) = "white" {}
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
            #include "../noise/SimplexNoise3D.hlsl"

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
            sampler2D _MainTex2;
            float4 _MainTex2_TexelSize;

            sampler2D _DepthTex;
            sampler2D _StencilTex;
            float _DepthTh;
            float _Invert;
            float _Brightness;
            float4 _MainTex_ST;
            float _GlobalIntensity;//VJ用にパラメータを上下させる

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

            float3 rgb2hsv(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float strength = 20;
                if(_ScreenParams.x > _ScreenParams.y) strength = 40;

                strength += + _GlobalIntensity*10;


                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);

                fixed4 col = tex2D(_MainTex, i.uv);
                float3 hsv = rgb2hsv(col.rgb);

                float ss = frac( hsv.x + _Time.x);

                float sine1 = 3 + 1 * sin(_Time.y*0.8);
                float sine2 = 3 + 1 * cos(_Time.y*0.8);

                float rad = hsv.x * 3.1415 * 2 + _Time.x;
                float ds = hsv.y;
                //float rad = floor( frac(hsv.x*4+_Time.x) * 8 ) / 8 * 3.1415 * 2;
                //float ds = floor( hsv.y * 5 ) / 5;
                float dx = 0.007 * ds * cos(rad);
                float dy = 0.007 * ds * sin(rad);

                float2 mosaicUV = i.uv + float2(
                    dx + 0.003 * snoise(float3(i.uv * sine1 * aspect, 11 + _Time.x * 0.6 )),
                    dy + 0.003 * snoise(float3(i.uv * sine2 * aspect, 99 + _Time.x * 0.7 ))                    
                    //(col.x-0.5) * 0.012 + 0.005 * snoise(float3(i.uv * sine1 * aspect, 11 + _Time.x * 0.6 )),
                    //(col.y-0.5) * 0.012 + 0.005 * snoise(float3(i.uv * sine2 * aspect, 99 + _Time.x * 0.7 ))
                );

                /*
                mosaicUV += 0.001 * float2(
                    snoise(float3(i.uv*8*aspect, 111 + _Time.x * 1.2 )),
                    snoise(float3(i.uv*8*aspect, 999 + _Time.x * 1.3 ))
                );*/
                mosaicUV.xy = floor( mosaicUV.xy * 1/_MainTex2_TexelSize.xy ) / (1/_MainTex2_TexelSize.xy);
                
                fixed4 col0 = tex2D(_MainTex2,mosaicUV);

                //i.uv.x = 1 - i.uv.x;
                float2 stencilUV = GetStencilUV(i.uv);

                fixed4 stencil  = tex2D( _StencilTex, stencilUV );
                //fixed4 depth    = tex2D( _DepthTex, stencilUV );

                //マスク
                //col.rgb = lerp( col0.rgb, col.rgb, stencil.r);                

                if(_Invert == 1) stencil.r = 1 - stencil.r;    

                col.rgb = lerp( col0.rgb, col.rgb, stencil.r);
                col.rgb *= 0.9999;


                return col;

            }
            ENDCG
        }
    }
}
