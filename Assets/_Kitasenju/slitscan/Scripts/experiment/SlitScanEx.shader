Shader "Custom/SlitScanEx"
{
    Properties
    {
        _MainTex1 ("_MainTex1", 2D) = "white" {}
        _MainTex2 ("_MainTex2", 2D) = "white" {}
        _MainTex3 ("_MainTex3", 2D) = "white" {}
        _MainTex4 ("_MainTex4", 2D) = "white" {}
        
        _Displacement ("_Displacement", 2D) = "white" {}
        _Displacement2 ("_Displacement2", 2D) = "white" {}
        _Displacement2Ratio ("_Displacement2Ratio", Range(0,1)) = 0

        
        _Th ("_Th",Vector) = (0,0,0,0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex1;
            sampler2D _MainTex2;
            sampler2D _MainTex3;
            sampler2D _MainTex4;

            sampler2D _Displacement;
            sampler2D _Displacement2;
            
            float4 _Th1;
            float4 _Th2;
            float4 _Th3;
            float4 _Th4;

            float _Displacement2Ratio;

            uniform float4 _Displacement_ST;
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = TRANSFORM_TEX(i.uv,_Displacement);

                fixed4 d = tex2D(_Displacement, uv );
                fixed4 d2 = tex2D(_Displacement2, uv );
                
                d = (d*(1-_Displacement2Ratio) + d2*_Displacement2Ratio);

                d.rgb = sin( frac(d.rgb) * 2 * 3.1415 ) * 0.5 + 0.5;
                
                
                fixed4 col;
                float rr = (d.r + d.g + d.b) / 3;
                rr = 1 - rr;
                fixed2 offset = fixed2(d.g,d.b) * 0;

                if( step(_Th1.x-rr,0) + step(rr-_Th1.y,0) == 2 ){
                    col = tex2D(_MainTex1, i.uv+offset );

                }else if( step(_Th2.x-rr,0) + step(rr-_Th2.y,0) == 2 ){
                    col = tex2D(_MainTex2, i.uv+offset );

                }else if( step(_Th3.x-rr,0) + step(rr-_Th3.y,0) == 2 ){
                    col = tex2D(_MainTex3, i.uv+offset );

                }else if( step(_Th4.x-rr,0) + step(rr-_Th4.y,0) == 2 ){
                    col = tex2D(_MainTex4, i.uv+offset );
                    
                }else{
                    clip(-1);
                }


                
                col.rgb = (col.r+col.g+col.b)*0.333;

                return col;
            }
            ENDCG
        }
    }
}
