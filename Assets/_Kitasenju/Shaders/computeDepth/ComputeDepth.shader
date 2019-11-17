// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ViewPortToPos"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_VP ("viewport position", Vector) = (0.5,0.5,1,0)
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos: TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _VP; // viewPosition
			
			float3 clipToLocal(float4 clipPos){
				
                float4 viewPos = mul(unity_CameraInvProjection, clipPos);
				viewPos.w = 1;
				return mul(viewPos, UNITY_MATRIX_IT_MV).xyz;

			}
			
			v2f vert (appdata v)
			{

                //ビューポート座標
                //スクリーン座標の値を0〜1にした物です。

                //_ProjectionParams	
                //x は 1.0 または -1.0、反転した射影行列で現在レンダリングしている場合は負の値。
                //y は カメラのNear Plane
                //z は カメラのFar Plane
                //w は 1/Far Plane。

				float n = _ProjectionParams.y; // near
				float f = _ProjectionParams.z; // far
				
				float w = _VP.z;
				float z = w * (2*(w-n)/(f-n)-1);//-1 1に。
				float2 xy = w * (2*_VP.xy-1);

                //クリップ座標系	遠近法的な処理が加えられ -1<x,y,z<1となるような座標系
                //4番目の座標Wはカメラに映る範囲を切り取るのに用いられます。
                //https://xr-hub.com/archives/12124
				float4 clipPos = float4(xy,z,1);//w);
				float3 localPos = clipToLocal(clipPos);
				
				v.vertex.xyz += localPos;
				
                //http://edom18.hateblo.jp/entry/2019/08/11/223803
                //float sceneZ = LinearEyeDepth(tex2D(_CameraDepthTexture, i.uv));

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
                o.pos = o.vertex;//

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = half4(i.pos.xyz/i.pos.w*0.5+0.5,0);
				return col;
			}
			ENDCG
		}
	}
}