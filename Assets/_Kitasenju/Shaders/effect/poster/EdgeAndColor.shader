Shader "effects/poster/EdgeAndColor"
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
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 duv = _MainTex_TexelSize.xy;

                half3 cg = GetEdge(_MainTex,i.uv,duv);

                half3 edge = cg * _Sensitivity;

                fixed4 col0 = tex2D(_MainTex, i.uv);

                fixed4 col = fixed4( frac(edge*3+_Time.zzz) ,1) * step(0.5,length(edge.rgb) - _Threshold);
                //fixed4 col = fixed4(edge,1);//_EdgeColor * step(0.5,edge - _Threshold);

                //col = 1-col;


                float2 stencilUV = GetStencilUV( i.uv );
                fixed4 stencil = tex2D(_StencilTex, stencilUV);

                if(_Invert==1) stencil.r = 1 - stencil.r;

                col0.rgb = (col0.r+col0.g+col0.b)*0.33333;
                fixed4 outputCol = lerp(col,col0,stencil.r);


                return outputCol;
                //return _EdgeColor * saturate(edge - _Threshold);
            }
            ENDCG
        }
    }
}