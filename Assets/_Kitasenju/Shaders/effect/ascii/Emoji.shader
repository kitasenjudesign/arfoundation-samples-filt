Shader "effects/ascii/Emoji"
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
            float _GlobalIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float2 GetEmojiUV(float2 uv, float offset){
                
                float strength = 15;
                if(_ScreenParams.x > _ScreenParams.y) strength = 30;

                //strength += _GlobalIntensity*10;

				//モザイクを何分割するか
				float2 mosaicSize = float2(
					strength,
					strength * _ScreenParams.y / _ScreenParams.x
				);

                uv += 1/mosaicSize*offset;

				float2 roundedUv = floor(uv*mosaicSize) / mosaicSize;
                //roundedUv.xy += 1/mosaicSize * offset;


				//分割した座標から輝度を取得する
				fixed4 srcPixel = tex2D(_MainTex, roundedUv);
				float mean = ( srcPixel.x + srcPixel.y + srcPixel.z ) / 3.0;//1.0にしないため
                mean = floor( frac(mean+_Time.x+_GlobalIntensity*0.3) * 10) / 10 * 0.999;

				//分割内の座標の比率
				float2 uvRatio = ( uv - roundedUv ) / (1/mosaicSize);

				//16分割された画像を参照する。
				float split = 16;

                float ox = frac( floor(mean * 16 * split)/split );
                float oy = floor( mean * 16 ) / split;

				float2 uvv = float2(ox,oy) + uvRatio * float2(1/split,1/split); //rx,ry

                return uvv;

            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 stencilUV = GetStencilUV( i.uv );
                fixed4 stencil = tex2D(_StencilTex, stencilUV);

                fixed4 col0 = tex2D(_MainTex,i.uv);//normal

                
                //float2 uvv = GetEmojiUV(i.uv);
                
                fixed4 ascii1 = tex2D(_AsciiTex, GetEmojiUV(i.uv,0.0));
                fixed4 ascii2 = tex2D(_AsciiTex, GetEmojiUV(i.uv,0.5));

                fixed4 ascii = lerp(ascii2, ascii1, ascii1.a);

                ascii = lerp( col0, ascii, step(0.5,ascii.a) );
                //ascii = lerp( colNoise, srcPixel+fixed4(0.6,0.6,0.6,1), step(0.5,ascii.x) );

                if(_Invert==1) stencil.r = 1 - stencil.r;

				fixed4 col = lerp( 
                    col0,
                    //srcPixel,
                    ascii,//ascii
                    step(0.5,stencil.r)
                );

				return col;
            }
            ENDCG
        }
    }
}
