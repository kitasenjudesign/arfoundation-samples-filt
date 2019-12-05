Shader "effects/ripple/Ripple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            float _Invert;
            float _GlobalIntensity;
            sampler2D _StencilTex;

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
                float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);

                float2 stencilUV = GetStencilUV(i.uv);
                fixed4 stencil  = tex2D( _StencilTex, stencilUV );

                float2 d = i.uv - float2(0.5,0.5);
                
                float amp = length( d * aspect );
                float rad = atan2(d.y,d.x);

                float aa = 0.04 + 0.03 * _GlobalIntensity;
                amp = aa*sin( amp * 30 - _Time.z*5 );// + 0.01*sin(rad*10+_Time.x);
                //float amp2 = 0.03*cos( amp * 30 - _Time.z*3 );

                float2 uvv = float2(
                    amp * cos(rad),
                    amp * sin(rad)
                );

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv + uvv);
                fixed4 col0 = tex2D(_MainTex,i.uv);

                if( _Invert == 1) stencil.r = 1 - stencil.r;
                col.rgb = lerp( col0.rgb, col.rgb, stencil.r ); 

                
                return col;
            }
            ENDCG
        }
    }
}
