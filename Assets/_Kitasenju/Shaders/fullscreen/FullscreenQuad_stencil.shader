Shader "full/FullScreenQuad_stencil"
{
	Properties
	{
		_MainTex ("_MainTex", 2D) = "white" {}
		_StencilTex ("_StencilTex", 2D) = "white" {}
		_Brightness ("_Brightness",float) = 0

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
            #include "../effect/util/StencilUV.hlsl"			


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
			sampler2D _StencilTex;
			float4 _MainTex_ST;
			float _Brightness;
			
			v2f vert (appdata v)
			{
				v2f o;
				
				/*
				o.vertex = v.vertex;// + float4(0.5,0.5,0,0);//UnityObjectToClipPos(v.vertex);
				o.vertex.x *= 2.0;
				o.vertex.y *= 2.0;
				o.vertex.z = 0.00001;
				o.vertex.w = 1.0;
				*/
				
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 colStencil = tex2D(_StencilTex, GetStencilUV( i.uv ) );
				
                if(colStencil.r<0.5) clip(-1);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}