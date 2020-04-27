Shader "StencilAndObject"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _StencilTex ("_StencilTex", 2D) = "white" {}
        _CamTex ("_CamTex1", 2D) = "white" {}
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
            #include "./StencilUV.hlsl"

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
            sampler2D _CamTex1;      
            sampler2D _DepthTex;
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);      
            
            /*
            float2 GetStencilUV( float2 uv ){

                float2 stencilUV = float2(
                    1-uv.y,
                    1-uv.x
                );

                float camTexWidth = 1920;
                float camTexHeight = 1440;
                float aspect = (camTexWidth/camTexHeight) / (_ScreenParams.y/_ScreenParams.x);

                stencilUV.y = stencilUV.y * aspect + (1-aspect)/2;

                return stencilUV;

            }*/

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);//CGの色
                fixed4 camCol = tex2D(_CamTex1,i.uv);//カメラの色
                
                fixed4 stencil = tex2D(_StencilTex, GetStencilUV(i.uv));//stencil
                float depth = tex2D(_DepthTex, GetStencilUV(i.uv)).r;//カメラのdepth

                //CGのdepth
                float sceneZ = LinearEyeDepth(tex2D(_CameraDepthTexture, i.uv));
                float depth01 = Linear01Depth(tex2D(_CameraDepthTexture, i.uv));

                //
                float delta = saturate(sceneZ - depth);

                

                //ステンシルがないところは、必ずCGを優先する
                if(stencil.r<0.5){
                    if(depth01>0.5){
                        col.g=0;
                        col.b=0;
                    }
                    return col;
                }

                //depthを比較し
                if(delta>0){
                    return camCol;//カメラ色
                }else{
                    return col;//CG色
                }
                //return lerp( col, sceneZ>depth?camCol:col, stencil.r);

            }
            ENDCG
        }
    }
}
