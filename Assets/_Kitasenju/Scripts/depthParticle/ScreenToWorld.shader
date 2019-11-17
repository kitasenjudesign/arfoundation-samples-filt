Shader "Unlit/ScreenToWorld"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScreenPos("ScreenPos", Vector) = (0,0,0,0)
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
			float4 _MainTex_ST;
			float4 _ScreenPos;

			float4x4 _InvProjMat;
			float4x4 _InvViewMat;

			v2f vert (appdata v)
			{
				v2f o;

				float n = _ProjectionParams.y;//near
				float f = _ProjectionParams.z;//far
				
				float w = _ScreenPos.z;
				float z = w * (2*(w-n)/(f-n)-1);
				float2 xy = w * (2*_ScreenPos.xy-1);
				float4 clipPos = float4(xy,z,w);

				float4 camPos = mul( _InvProjMat, clipPos );//カメラ座標
        		camPos.w = 1;
		        float4 worldPos = mul(_InvViewMat, camPos);//ワールド座標

				v.vertex.xyz += worldPos;

				o.vertex = UnityObjectToClipPos( v.vertex );
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				

				//_ScreenPos.xy

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = fixed4(1,0,0,1);//tex2D(_MainTex, i.uv);
				//col.r = 1;//_Matrix1[0][2];
				//col.g = _Matrix1[2][0];
				//col.b = 0;
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
}
