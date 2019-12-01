Shader "effects/naname/NanameInvertColor"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _DepthTex ("_DepthTex", 2D) = "white" {}
        _DepthTh("_DepthTh",Range(0,1)) = 0.5
        _Detail("_Detail",Range(0,5)) = 0.5
        _Naname("_Naname",Range(0,15)) = 0.5

        [Toggle] _Invert("_Invert", Float) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata members scrPos)
#pragma exclude_renderers d3d11
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
                float4 scrPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _DepthTex;
            sampler2D _StencilTex;
            float _DepthTh;
            float _Detail;
            float _Naname;
            float _Invert;
            float4 _MainTex_ST;
            float _GlobalIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.scrPos = ComputeScreenPos(o.vertex);  

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

                float2 mosaicUV = float2(
                    snoise(float3(i.uv*_Detail, 2.0 + _Time.y*1.0)),
                    snoise(float3(i.uv*_Detail, 2.0 + _Time.y*1.5))
                );

                fixed4 col = tex2D(_MainTex, abs( frac( i.uv + mosaicUV ) ) );
                

                float2 uvv = i.scrPos.xy / i.scrPos.w * aspect;
                
                float n = 1;// + 0.5 + 0.5 * sin( _Time.y );

                if(_ScreenParams.x>_ScreenParams.y) n = 3;

                uvv.x += _Time.x * 3;

                float ss = step ( frac(  n*_Naname*(uvv.x+uvv.y) ),0.5 );

                if(_Invert==1){
                    stencil.r = 1 - stencil.r;
                }

                //マスク
                col.rgb = lerp( 
                    col0.rgb, 
                    lerp( 1.0-col0, col0, ss),
                    stencil.r
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
