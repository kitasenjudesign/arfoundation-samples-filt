Shader "effects/duplication/SlitMask"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _Index("_Index",float) = 0
        //_DepthTex ("_DepthTex", 2D) = "white" {}
        _DepthTh("_DepthTh",Range(0,1)) = 0.5
        //_Detail("_Detail",Range(0,5)) = 0.5
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
            sampler2D _MainSlitTex;
            
            //sampler2D _DepthTex;
            sampler2D _StencilTex;
            float _DepthTh;
            float _Detail;
            float4 _MainTex_ST;
            float _Revert;
            float _Index;

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
                
                /*
                float2 noiseUV = float2(
                    snoise(float3(i.uv*10+_Index, 2.0 + _Time.y*1.0)),
                    snoise(float3(i.uv*10+_Index, 2.0 + _Time.y*1.5))
                );*/


                float2 uvv = i.uv;


                fixed4 col0 = tex2D(_MainTex, uvv);

                float2 stencilUV = uvv;
                stencilUV.y = 1 - stencilUV.y;

                float bai = 9.0/12.0 * 0.8;//4;3 16;12 16;9
                stencilUV.y = stencilUV.y*bai + (1-bai)/2;

                fixed4 stencil  = tex2D( _StencilTex, stencilUV );
                //fixed4 depth    = tex2D( _DepthTex, stencilUV );

                

                fixed4 col = tex2D(_MainSlitTex, uvv );//
                

                //マスク
                if(_Revert==0){
                    col.rgb = lerp( col0.rgb, col.rgb, stencil.r);                
                }else{
                    col.rgb = lerp( col.rgb, col0.rgb, stencil.r);         
                }
                //if( depth.r < _DepthTh ){
                //     col.rgb = col0.rgb;
                //}

                return col;

            }
            ENDCG
        }
    }
}
