Shader "effects/unity/UnityMaskPerlinB"
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


					float yama(float rr, float beki) {
						
						float hoge = 0.5 + 0.5 * sin(rr * 3.14 - 3.14 * 0.5);
						//out = pow(out, beki);
						return hoge;// out;
					
					}
					float yama2(float rr, float beki) {
						
						float hoge = rr * 2.0;
						if ( hoge < 1.0) {
							hoge = pow(hoge, 1./beki) * 0.5;
						}else {
							hoge = pow(hoge-1.0, beki) * 0.5 + 0.5;
						}
						//out = pow(out, beki);
						return hoge;// out;
					
					}
					


            //simple
            fixed4 frag (v2f i) : SV_Target
            {
                    float strength = 1.4;

                    //0.4-0.6 no hani wo kurikaesu
                    float minX = 0.45;
                    float maxX = 0.55;
                    float amp = maxX - minX;

                    float nn = ( 1 + 1 * sin( _Time.z ) );

                    float xx = minX + amp * (0.5 + 0.5 * sin(yama2(i.uv.x, 2.0) * nn * 3.14 - 3.14 * 0.5));
                    float minA = 0.3;// 1;
                    float maxA = 0.7;// 9;
					
                    if (i.uv.x < minA) {
                        
                        xx = xx * pow(i.uv.x / minA, 0.2);
                        
                    }else if (i.uv.x > maxA) {
                        
                        xx = lerp(xx, i.uv.x, pow((i.uv.x - maxA) / (1.0 - maxA), 7.0));
                        
                    }                    

                    xx += 0.01 * sin(i.uv.y * 2. * 3.14);
					float yy = i.uv.y + 0.05 * sin(i.uv.x * 6.0 * 3.14);

                        //float ss = strength;
						float ss = 0.5 + 0.5 * sin(_Time.z );
						ss = ss * strength;
						xx = lerp(i.uv.x, xx, ss );
						yy = lerp(i.uv.y, yy, ss );
						
						//xx = 0.5+0.5*sin( 2.*3.14*vUv.x -3.14/2.0);
							
						//xx = 0.5 + 0.5 * sin(xx * 2.0 * 3.14 * 5.0);
						//vUv.x
						
						float2 axis = float2( xx, yy );





                /*
                float2 offset = float2(
                    0.13*snoise(float3(i.uv*2, 2.0 + _Time.y*0.3)),
                    0.13*snoise(float3(i.uv*2, 2.0 + _Time.y*0.4))
                );
                i.uv += offset;
                */

                fixed4 col = tex2D(_MainTex, axis);

                
                float2 stencilUV = GetStencilUV( axis );//i.uv );
                fixed4 stencil = tex2D(_StencilTex, stencilUV);

                if(_Invert==1) stencil.r = 1 - stencil.r;
                col.a = stencil.r;  
                

                return col;
            }
            ENDCG
        }
    }
}
