Shader "effects/duplication/HumanBunshinBunkatsu"
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

            sampler2D _MainTex;
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

            float rand(float2 co) {
                float a = frac(dot(co, float2(2.067390879775102, 12.451168662908249))) - 0.5;
                float s = a * (6.182785114200511 + a * a * (-38.026512460676566 + a * a * 53.392573080032137));
                float t = frac(s * 43758.5453);
                return t;
            }            

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                /*
                float2 noiseUV = float2(
                    _Strength * 0.3 * snoise(float3(i.uv*3, 2.0 + _Time.y*1.0)),
                    _Strength * 0.3 * snoise(float3(i.uv*3, 2.0 + _Time.y*1.5))
                );  */

				//モザイクを何分割するか
				float2 mosaicSize = float2(
					10,
					10 * _ScreenParams.y / _ScreenParams.x
				);
				float2 roundedUv = floor(i.uv*mosaicSize) / mosaicSize;

                float2 dd = rand(
                    float2( roundedUv.x*2, roundedUv.y*2+_Strength*2 )
                ) - float2(0.5,0.5);

                float2 uvv = ( 
                    i.uv +
                    dd * float2(0.2, 0.2*_ScreenParams.x/ _ScreenParams.y) 
                );

                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);
                //float2 mosaicUV = i.uv;//round(i.uv*aspect*120)/(aspect*120);
                //fixed4 col = tex2D(_MainColTex, mosaicUV);//mosaic
                fixed4 col0 = tex2D(_MainColTex,uvv);//normal
                fixed4 colA = tex2D(_MainTex,i.uv);

                //i.uv.x = 1 - i.uv.x;
                float2 stencilUV = GetStencilUV( uvv );
                fixed4 stencil = tex2D(_StencilTex, stencilUV);

                if(stencilUV.y<0.001) stencil.r = 0;
                if(stencilUV.y>0.999) stencil.r = 0;

                //clip( stencil.r - 0.6 );

                fixed4 colOut = lerp(colA,col0,smoothstep(0.7,1,stencil.r));

                return colOut;

            }
            ENDCG
        }
    }
}
