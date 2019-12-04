Shader "effects/sin/NoiseSin"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _DepthTex ("_DepthTex", 2D) = "white" {}
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
            #include "../noise/SimplexNoise3D.hlsl"
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

                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);

                //を作る
                /*
                fixed4 col0 = tex2D(_MainTex,i.uv);
                float2 stencilUV = GetStencilUV( i.uv );
                fixed4 stencil  = tex2D( _StencilTex, stencilUV );
                float2 mosaicUV = float2(
                    snoise(float3(i.uv.x+col0.r*2.0,i.uv.y+col0.g*2.0, 2.0 + _Time.x*1)),
                    snoise(float3(i.uv.x+col0.g*2.0,i.uv.y+col0.b*2.0, 2.0 + _Time.x*1))
                );
                fixed4 col = tex2D(_MainTex, abs( frac( i.uv+mosaicUV*0.3 ) ) );
                */

                //if(_Invert==1) stencil.r = 1 - stencil.r;
                //col.rgb = lerp( col0.rgb, col.rgb, stencil.r);                
                //col.rgb = lerp( col0.rgb, float3(0,0,0), stencil.r);                


                //sinを作る
                float spd = 1;
                if( _ScreenParams.x > _ScreenParams.y ) spd *= 0.5;

                float amp = 0.5 + 0.5 * sin( _Time.z * spd );

                float2 uvv = i.uv + float2( 0.2 * amp * sin(i.uv.y * (20.0+20*amp) * spd + _Time.z),0 );
                fixed4 col0 = tex2D(_MainTex,uvv);//sin curve

                float2 stencilUV = GetStencilUV( _Invert==0 ? uvv : i.uv );


                fixed4 stencil  = tex2D( _StencilTex, stencilUV );

                float ratio = stencil.r;
                if(_Invert==1){
                    ratio = 1 - ratio;
                }

                //fixed4 colA = tex2D(_MainTex, uvv );
                //if(_Invert==1) stencil.r = 1 - stencil.r;
                
                fixed4 col = tex2D(_MainTex,i.uv);
                col.rgb = lerp(
                    col,
                    col0.rgb,
                    //lerp( col0.rgb, colA.rgb, ratio),
                    ratio
                );

                //col.rgb = lerp( col0.rgb, col.rgb, stencil.r);    

                return col;

            }
            ENDCG
        }
    }
}
