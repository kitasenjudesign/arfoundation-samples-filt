Shader "full/FullScreenQuad"
{
	Properties
	{

		_MainTex ("Texture", 2D) = "white" {}
		_MainTex2 ("Texture", 2D) = "white" {}
		_FeedbackRatio ("_FeedbackRatio", float) = 0
		_Motion("_Motion",Vector) = (0,0,0,0)
		_IsInvertUV("_IsInvertUV",float) = 1
		
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
			sampler2D _MainTex2;
			float _FeedbackRatio;
			float4 _MainTex_ST;
			float4 _Motion;
			float _IsInvertUV;
			
			v2f vert (appdata v)
			{
				v2f o;
				
				o.vertex = v.vertex;// + float4(0.5,0.5,0,0);//UnityObjectToClipPos(v.vertex);
				o.vertex.x *= 2.0;
				o.vertex.y *= 2.0;
				o.vertex.z = 0.00001;
				o.vertex.w = 1.0;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}
			
			float rand(float2 co){
				return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				if( _IsInvertUV == 1 ) i.uv.y = 1 - i.uv.y;

				float2 dUV = i.uv + _Motion.xy;
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col2 = tex2D(_MainTex2, dUV);

				float ratio = _FeedbackRatio;


				float2 aspect = float2(1,_ScreenParams.y/_ScreenParams.x);
				float bright = (col.r+col.g+col.b)/3;
				float th = step(
					1-ratio,
					0.4999 * bright + 0.5*abs(rand( floor(i.uv*aspect*50)/50 )) 
				);

				fixed4 colOut = lerp(col,col2,th);

                //if(col.r<0.5) clip(-1);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return colOut;
			}
			ENDCG
		}
	}
}