Shader "slit/SimpleSlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pos("_Pos",vector) = (0,1,0,0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off 
        //ZWrite Off 
        //ZTest Always

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
            float4 _Pos;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if( i.uv.y >= _Pos.x && i.uv.y <= _Pos.y ){
                    //いち
                }else{
                    clip(-1);
                }
                
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
