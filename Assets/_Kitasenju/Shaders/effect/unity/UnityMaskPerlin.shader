Shader "effects/unity/UnityMaskPerlin"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        [Toggle] _Revert("_Revert", Float) = 0 
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        //ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "../util/StencilUV.hlsl"
            #include "../noise/SimplexNoise3D.hlsl"

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
            float _Invert;
					
            //simple
            fixed4 frag (v2f i) : SV_Target
            {

                float2 offset = float2(
                    0.13*snoise(float3(i.uv*2, 2.0 + _Time.y*0.3)),
                    0.13*snoise(float3(i.uv*2, 2.0 + _Time.y*0.4))
                );
                i.uv += offset;


                fixed4 col = tex2D(_MainTex, i.uv);

                float2 stencilUV = GetStencilUV( i.uv );
                fixed4 stencil = tex2D(_StencilTex, stencilUV);


                // just invert the colors
                /*
                if(_Revert==0){
                    if(stencil.r < 0.5) discard;
                }else{
                    if(stencil.r > 0.5) discard;
                } */

                if(_Invert==1) stencil.r = 1 - stencil.r;
                col.a = stencil.r;  


                return col;
            }
            ENDCG
        }
    }
}
