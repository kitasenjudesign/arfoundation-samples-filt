Shader "Unlit/MyHatch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _HatchTex ("Texture", 2D) = "hatch" {}

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
            sampler2D _HatchTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            //rotate 2d
			float2 rotate(float2 uv, float rad){

				float sinX = sin ( rad );
				float cosX = cos ( rad );
				float sinY = sin ( rad );
				float2x2 rotationMatrix = float2x2( cosX, -sinX, sinY, cosX);
				return mul ( uv, rotationMatrix );

			}

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                fixed4 col = tex2D(_MainTex, i.uv);

                float intensity = 0.333 * ( col.r + col.g + col.b );
                
                intensity = floor(intensity*10);
                fixed4 outputCol = fixed4(1,1,1,1);
                fixed4 hatch = fixed4(0,0,0,1);
                
                for(float i=0;i<intensity;i++){

                    //hatch = tex2D(_MainTex, i.uv);
                    //outputCol.rgb -= hatch.rgb;

                }

                

                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return outputCol;
            }
            ENDCG
        }
    }
}
