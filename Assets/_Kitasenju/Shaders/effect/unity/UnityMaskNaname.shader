Shader "effects/unity/UnityMaskNaname"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        //[Toggle] _Revert("_Revert", Float) = 0 
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
            float _Revert;
            //simple
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 stencilUV = GetStencilUV( i.uv );
                fixed4 stencil = tex2D(_StencilTex, stencilUV);


                //float ss = step ( frac(  10*(i.uv.x+i.uv.y) ),0.5 );
                clip( frac(  10*(i.uv.x+i.uv.y+_Time.x) ) - 0.5 );

                return col;
            }
            ENDCG
        }
    }
}
