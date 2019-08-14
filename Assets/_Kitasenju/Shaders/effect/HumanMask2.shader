Shader "seg/HumanMask2"
{
    Properties
    {
        _MainColTex ("_MainColTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _Bai("_Bai",float) = 0
        _Bai2("_Bai2",float) = 0

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

            sampler2D _MainColTex;
            sampler2D _StencilTex;
            float _Revert;
            float _Bai;
            float _Bai2;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);
                float2 mosaicUV = round(i.uv*aspect*120)/(aspect*120);

                fixed4 col = tex2D(_MainColTex, mosaicUV);//mosaic
                fixed4 col0 = tex2D(_MainColTex,i.uv);//normal

                //i.uv.x = 1 - i.uv.x;
                float2 stencilUV = i.uv;
                stencilUV.y = 1 - stencilUV.y;

                float bai = 9.0/12.0 * _Bai;//4;3 16;12 16;9
                float2 bai2 = _Bai2;
                stencilUV.x = stencilUV.x*bai2 + (1-bai2)/2;
                stencilUV.y = stencilUV.y*bai + (1-bai)/2;
                
                //i.uv.x = saturate(i.uv.x);


                fixed4 stencil = tex2D(_StencilTex, stencilUV);
                clip( stencil.r - 0.5 );

                fixed4 colOut = lerp(
                    col0,
                    col,
                    smoothstep(0.6,0.8,i.uv.y)
                );

                return colOut;

            }
            ENDCG
        }
    }
}
