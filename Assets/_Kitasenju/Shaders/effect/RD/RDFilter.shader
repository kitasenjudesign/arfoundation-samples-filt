Shader "Unlit/RDFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex2 ("Texture", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _DepthTex ("_DepthTex", 2D) = "white" {}
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
            sampler2D _MainTex2;
            sampler2D _DepthTex;
            sampler2D _StencilTex;
            float _Invert;
            float4 _MainTex_ST;
            float4 _MainTex2_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col0 = tex2D(_MainTex, i.uv);
                fixed4 stencilColor = tex2D(_MainTex2, i.uv);

                //
                float3 duv = float3(_MainTex2_TexelSize.xy, 0);

                half v0 = tex2D(_MainTex2, i.uv).y;
                half v1 = tex2D(_MainTex2, i.uv - duv.xz).y;
                half v2 = tex2D(_MainTex2, i.uv + duv.xz).y;
                half v3 = tex2D(_MainTex2, i.uv - duv.zy).y;
                half v4 = tex2D(_MainTex2, i.uv + duv.zy).y;

                float3 nml = normalize(float3(v1 - v2, v3 - v4, 0.5));

                

                fixed4 colRd = smoothstep(0.2,0.3,stencilColor.g);//* col0
                colRd.a = 1;

                float2 stencilUV = GetStencilUV(i.uv);
                fixed4 stencil  = tex2D( _StencilTex, stencilUV );

                if(_Invert==1) stencil.r = 1 - stencil.r;

                //col0.rgb = (col0.r +  col0.g + col0.b)/3;
                //fixed4 col = fixed4( lerp( col0.rgb, colRd.rgb, 1-stencil.r), 1 );
                //fixed4 col = fixed4( nml.xyz, 1 );
                fixed4 col = tex2D(_MainTex, i.uv + 0.05*(nml.xy - float2(0.5,0.5)) );
                
                if(stencil.x > 0.5 ){

                    col.rgb *= colRd.rgb;
                    col.rgb = lerp( 
                        float3(0,0,0),
                        //col0.rgb, 
                        col.rgb, 
                        smoothstep(0.3,0.7,(colRd.r +  colRd.g + colRd.b)/3 ) 
                    );

                }else{
                    
                    //col.rgb *= colRd.rgb;
                    col.rgb = col0.rgb;
                }
                //}else{
                //    col.rgb *= 1 - colRd.rgb;
                //}

                //col.rgb = stencilColor.rgb;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
