Shader "effects/poster/Poster"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Sensitivity ("Sensitivity", float) = 1.0
        _Threshold ("Threshold", float) = 0.0
        _EdgeColor ("Edge Color", COLOR) = (1,1,1,1)
        _StencilTex ("_StencilTex", 2D) = "white" {}           

        [Toggle] _Invert("_Invert", Float) = 0          
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #pragma multi_compile _ Sobel
           #pragma multi_compile _ Prewitt
           #pragma multi_compile _ RobertsCross

            #include "UnityCG.cginc"
            #include "../noise/SimplexNoise3D.hlsl"
            #include "../util/Edge.hlsl"
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
            sampler2D _StencilTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float _Sensitivity;
            float _Threshold;
            half4 _EdgeColor;
            float _Invert;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            float rand(float2 co){
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 duv = _MainTex_TexelSize.xy;


                fixed4 col0 = tex2D(_MainTex, i.uv);

                float cg = GetEdge(_MainTex, i.uv+(col0.rg-0.5)*0.01, duv);
                half4 edge = cg * _Sensitivity;


                float rr = snoise( float3(i.uv*300,floor( _Time.z*5 ) ) );//rand( floor( i.uv * 300 + floor( _Time.x*100 )*100 ) );
                fixed4 col = floor( ( col0 + 0.05 * rr ) * 3 ) / 3;



                col = col - _EdgeColor * step(0.5,edge - _Threshold);


                float2 stencilUV = GetStencilUV( i.uv );
                fixed4 stencil = tex2D(_StencilTex, stencilUV);

                if( _Invert==1 ) stencil.r = 1 - stencil.r;
                fixed4 outputCol = lerp(col0,col,stencil.r);


                return outputCol;
                //return _EdgeColor * saturate(edge - _Threshold);
            }
            ENDCG
        }
    }
}