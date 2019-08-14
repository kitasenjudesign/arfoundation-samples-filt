Shader "seg/HumanMask"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _OldTex("_OldTex", 2D) = "white" {}
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
            #include "./noise/SimplexNoise3D.hlsl"

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
            sampler2D _OldTex;
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
                // sample the texture
                
                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);
                float2 mosaicUV = round(i.uv*aspect*40)/(aspect*40);

                fixed4 col = tex2D(_MainTex, mosaicUV);//mosaic
                fixed4 col0 = tex2D(_MainTex,i.uv);//normal

                //i.uv.x = 1 - i.uv.x;
                float2 stencilUV = i.uv;
                stencilUV.y = 1 - stencilUV.y;

                float bai = 9.0/12.0 * 0.8;//4;3 16;12 16;9
                stencilUV.y = stencilUV.y*bai + (1-bai)/2;
                
                //i.uv.x = saturate(i.uv.x);


                float2 offset = float2(
                    snoise(float3(i.uv*2.6, _Time.y*1.0)),
                    snoise(float3(i.uv*2.6, _Time.y*1.5))
                );

                fixed4 stencil = tex2D(_StencilTex, stencilUV);

                float2 uvv = i.uv + offset * 0.005;
                uvv = saturate(uvv);
                fixed4 old = tex2D(_OldTex, uvv);

                if( _Revert == 0 ){
                    col = lerp( col0, old, step( stencil.r, 0.5 ) );
                }else{
                    col = lerp( col0, old, step( 1.0 - stencil.r, 0.5 ) );
                }
                //col.rgb = col0.rgb;//, col.rgb, stencil.r),
                
                return col;

            }
            ENDCG
        }
    }
}
