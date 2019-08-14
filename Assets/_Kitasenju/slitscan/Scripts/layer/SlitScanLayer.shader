Shader "Custom/SlitScanLayer"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _Displacement ("_Displacement", 2D) = "white" {}        
        _Th ("_Th",Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        cull off

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

            sampler2D _MainTex;
            sampler2D _Displacement;
            float4 _Th;

            uniform float4 _Displacement_ST;
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;//TRANSFORM_TEX(i.uv,_Displacement);
                fixed4 d = tex2D(_Displacement, uv );

                
                fixed4 col;
                float rr = (d.r + d.g + d.b) * 0.333;

                fixed2 offset = fixed2(d.g,d.b) * 0;

                if( step(_Th.x-rr,0) + step(rr-_Th.y,0) == 2 ){
                    col = tex2D(_MainTex, i.uv+offset );

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
