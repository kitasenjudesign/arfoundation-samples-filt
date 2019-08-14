Shader "Unlit/TestDepthImage6depth"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            sampler2D _Depth;
            sampler2D _Depth1;
            //sampler2D _Depth2;
            float4 _Th;

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //depth 1こで　いいのでは。

                // sample the texture
                float4 d0 = tex2D(_Depth,i.uv);
                float4 d1 = tex2D(_Depth1, i.uv);
                //float4 d2 = tex2D(_Depth2, i.uv);
                float4 col = float4(0,0,0,1);

                //col.rgb = max(d0.rgb,d1.rgb);

                
                if(d0.r==0 && d1.r>0){
                    col.r = d1.r;
                    col.g = _Th.x;
                }else if( d1.r>0 && d1.r < d0.r ){
                    col.r = d1.r;
                    col.g = _Th.x;
                }else{
                    col.r = d0.r;//そのまま
                    col.g = d0.g;//そのまま
                }

                return col;
            }
            ENDCG
        }
    }
}
