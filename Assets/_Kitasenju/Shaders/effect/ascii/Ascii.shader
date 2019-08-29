Shader "effects/ascii/Ascii"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _AsciiTex ("_AsciiTex", 2D) = "white" {}

        _DepthTh("_DepthTh",Range(0,1)) = 0.5
        _StencilTex ("_StencilTex", 2D) = "white" {} 

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _AsciiTex;
            sampler2D _StencilTex;
            float _DepthTh;
            float _Invert;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 stencilUV = GetStencilUV( i.uv );
                //stencilUV.y = 1 - stencilUV.y;
                //float bai = 9.0/12.0 * 0.8;//4;3 16;12 16;9
                //stencilUV.y = stencilUV.y*bai + (1-bai)/2;
                fixed4 stencil = tex2D(_StencilTex, stencilUV);

                fixed4 col0 = tex2D(_MainTex,i.uv);//normal

                


				//モザイクを何分割するか
				float2 mosaicSize = float2(
					20,
					20 * _ScreenParams.y / _ScreenParams.x
				);
				float2 roundedUv = floor(i.uv*mosaicSize) / mosaicSize;

				//分割した座標から輝度を取得する
				fixed4 srcPixel = tex2D(_MainTex, roundedUv);
				float mean = ( srcPixel.x + srcPixel.y + srcPixel.z ) / 3.0 * 0.999;//1.0にしないため

				//分割内の座標の比率
				float2 uvRatio = ( i.uv - roundedUv ) / (1/mosaicSize);

				//16分割された画像を参照する。
				float split = 16;

                float ox = frac( floor(mean * 2 * split)/split );
                float oy = floor( mean * 2 ) / split;

				float2 uvv = 
                    //float2(floor(mean*split)/split,0) //ox,oy
                    float2(ox,oy)
                    + uvRatio * float2(1/split,1/split); //rx,ry


                //noise col
                float2 mosaicUV = float2(
                    snoise(float3(col0.r,col0.g, 2.0 + _Time.y*0.3 )),
                    snoise(float3(col0.g,col0.b, 2.0 + _Time.y*0.4 ))
                );
                fixed4 colNoise = tex2D(_MainTex, abs( frac( roundedUv+mosaicUV ) ) );                
                
                fixed4 ascii = tex2D(_AsciiTex, uvv);
                ascii = lerp( srcPixel*0.8, srcPixel+fixed4(0.6,0.6,0.6,1), step(0.5,ascii.x) );
                //ascii = lerp( colNoise, srcPixel+fixed4(0.6,0.6,0.6,1), step(0.5,ascii.x) );

                if(_Invert==1) stencil.r = 1 - stencil.r;
				fixed4 col = lerp( 
                    col0,
                    ascii,//ascii
                    step(0.5,stencil.r)
                );

				return col;
            }
            ENDCG
        }
    }
}
