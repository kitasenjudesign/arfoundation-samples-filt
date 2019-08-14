Shader "effects/simple/Mono"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}        
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _StencilTex;

            //simple
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 stencilUV = GetStencilUV( i.uv );
                //stencilUV.y = 1 - stencilUV.y;
                //float bai = 9.0/12.0 * 0.8;//4;3 16;12 16;9
                //stencilUV.y = stencilUV.y*bai + (1-bai)/2;
                fixed4 stencil = tex2D(_StencilTex, stencilUV);


                // just invert the colors
                col.rgb = lerp( 
                    col,
                    0.3333 * ( col.r+col.g+col.b ),
                    stencil.r
                );
                
                return col;
            }
            ENDCG
        }
    }
}
