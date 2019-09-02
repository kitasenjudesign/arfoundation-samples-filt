Shader "effects/duplication/HumanBunshin"
{
    Properties
    {
        _MainColTex ("_MainColTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _Strength ("_Strength",float) = 0
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

            sampler2D _MainColTex;
            sampler2D _StencilTex;
            float _Strength;
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
                /*
                float2 noiseUV = float2(
                    _Strength * 0.3 * snoise(float3(i.uv*3, 2.0 + _Time.y*1.0)),
                    _Strength * 0.3 * snoise(float3(i.uv*3, 2.0 + _Time.y*1.5))
                );  */

                float2 dd = float2(
                    0.3*_Strength * sin( _Time.z+_Strength*3.14*2 ),
                    -_Strength * 0.5
                );

                float2 uvv = ( i.uv + dd );

                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);
                //float2 mosaicUV = i.uv;//round(i.uv*aspect*120)/(aspect*120);
                //fixed4 col = tex2D(_MainColTex, mosaicUV);//mosaic
                fixed4 col0 = tex2D(_MainColTex,uvv);//normal

                //i.uv.x = 1 - i.uv.x;
                float2 stencilUV = GetStencilUV( uvv );
                fixed4 stencil = tex2D(_StencilTex, stencilUV);
                clip( stencil.r - 0.6 );

                fixed4 colOut = col0;

                return colOut;

            }
            ENDCG
        }
    }
}
