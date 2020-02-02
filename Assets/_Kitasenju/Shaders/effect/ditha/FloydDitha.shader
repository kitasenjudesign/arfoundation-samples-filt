Shader "effects/ditha/FloydDitha"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}        
        _Split("_Split",range(1,10)) = 1
        _Mosaic("_Mosaic",float) = 50
        _Steps("_Steps",float)=1
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
            sampler2D _StencilTex;
            float _Split;
            float _Invert;
            float _Mosaic;
            float _Steps;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            float rand(float2 co) {
                float a = frac(dot(co, float2(2.067390879775102, 12.451168662908249))) - 0.5;
                float s = a * (6.182785114200511 + a * a * (-38.026512460676566 + a * a * 53.392573080032137));
                float t = frac(s * 43758.5453);
                return t;
            }

            fixed4 getNewCol(float2 uv){

                fixed4 oldCol = tex2D(_MainTex,uv);
                float steps = _Steps;
                fixed4 newCol = round((steps * oldCol)) / steps;
                //fixed4 errCol = oldCol - newCol;

                return newCol;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);
                float2 mosaicUV = round(i.uv*aspect*_Mosaic)/(aspect*_Mosaic);


                // sample the texture
                fixed4 oldCol = tex2D(_MainTex,mosaicUV);
                float steps = _Steps;
                fixed4 newCol = round((steps * oldCol)) / steps;
                fixed4 errCol = oldCol - newCol;

                fixed4 col = 
                    4 * errCol + 
                    7/16 * getNewCol( mosaicUV + 1/_Mosaic * float2(1,0) ) + 
                    3/16 * getNewCol( mosaicUV + 1/_Mosaic * float2(-1,1) ) +
                    5/16 * getNewCol( mosaicUV + 1/_Mosaic * float2(0,1) ) +
                    1/16 * getNewCol( mosaicUV + 1/_Mosaic * float2(1,1) );

                //col(1,0) * 7/16 + errCol
                //col(-1,1) * 3/16 + errCol
                //col(0,1) * 5/16 + errCol
                //col(1,1) * 1/16 + errCol


                //addError(img, 7 / 16.0, x + 1, y, errR, errG, errB);
                //addError(img, 3 / 16.0, x - 1, y + 1, errR, errG, errB);
                //addError(img, 5 / 16.0, x, y + 1, errR, errG, errB);
                //addError(img, 1 / 16.0, x + 1, y + 1, errR, errG, errB);

                //fixed4 col = errCol;


                //////////////////
                fixed4 col0 = tex2D(_MainTex,i.uv);

                float2 stencilUV = GetStencilUV(i.uv);

                fixed4 stencil  = tex2D( _StencilTex, stencilUV );


                if(_Invert == 1) stencil.r = 1 - stencil.r;
                col.rgb = lerp( col0.rgb, col.rgb, stencil.r);

                return col;

            }
            ENDCG
        }
    }
}