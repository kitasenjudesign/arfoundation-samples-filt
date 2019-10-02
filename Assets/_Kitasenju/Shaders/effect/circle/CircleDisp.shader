Shader "effects/circle/CircleDisp"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _Invert ("_Invert", float) = 0
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
            float4 _MainTex_ST;
            float _Invert;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            bool inCircle(float2 position, float2 offset, float size) {
                float len = length(position - offset);
                if (len < size) {
                    return true;
                }
                return false;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                float radius = 0.2;// + 0.1 * sin(_Time.y);
                float or = 0.15 + 0.03 * sin(_Time.y);
                float nn = 0;
                float2 pos = float2(0,0);
                float phase = _Time.y;

                for(float j=0.0;j<8.0;j++){
                    
                    pos.x = cos(j/8*6.28 + phase) * or + 0.5;
                    pos.y = sin(j/8*6.28 + phase) * or + 0.5;

                    if (inCircle (i.uv, pos, radius)) {
                        nn += 0.3;
                    }

                }

                nn = frac(nn);
                //nn = step(0.49,nn);

                float2 uvv = float2(i.uv.x-0.5,i.uv.y-0.5); 
                float amp = length( i.uv );
                float rad = atan2( i.uv.y,i.uv.x );

                //_Amp=0.5*( sin(_Time.y)*0.5+0.5 );

                float2 offset = float2(
                    (0.1*nn) * amp * cos(rad + 3.1415),
                    (0.1*nn) * amp * sin(rad + 3.1415)
                );


                
                fixed4 col0 = tex2D(_MainTex, i.uv);
                fixed4 col  = tex2D(_MainTex, i.uv + offset);

                fixed4 stencil  = tex2D( _StencilTex, GetStencilUV( i.uv ) );
                //col.r = nn;
                //col.g = nn;
                //col.b = nn;
                if(_Invert==1) stencil.r = 1 - stencil.r;
                col.rgb = lerp( col0.rgb, col.rgb, stencil.r);     

                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
