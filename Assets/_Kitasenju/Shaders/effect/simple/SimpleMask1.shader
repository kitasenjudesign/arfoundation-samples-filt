Shader "effects/simple/SimpleMask1"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
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
            sampler2D _StencilTex;
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
                fixed4 stencil = tex2D(_StencilTex, stencilUV);
                fixed4 col0 = tex2D(_MainTex,i.uv);//normal
 
                if(_Invert==1) stencil.r = 1 - stencil.r;
				
                col0.a = stencil.r;

                //clip(col0.r-0.5);

				return col0;
            }
            ENDCG
        }
    }
}
