Shader "CubesDepth05"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		
		_MainTex ("_MainTex", 2D) = "white" {}
		//_MainTex2 ("_MainTex2", 2D) = "white" {}


		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Amount ("_Amount", float) = 0.0
		_Speed  ("_Speed", float) = 1
		_Ratio ("_Ratio", Range(0,1)) = 0
		_Size ("_Size", Range(0.005,0.1)) = 0.04
		_Num("_Num", Vector) = (0,0,0,2)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow
		#pragma instancing_options procedural:setup
		
		struct Input
		{
			//float2 uv_MainTex;
            float4 screenPos;
			float4 col;//position  : POSITION;
		};
		// Boidの構造体
		struct CubeData
		{
			float3 position;
			float3 velocity;
			float4 color;
			float3 basePos;
			float2 uv;
			float time;
		};

		#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
			// Boidデータの構造体バッファ
			StructuredBuffer<CubeData> _CubeDataBuffer;
		#endif

		sampler2D _MainTex; // テクスチャ
		//sampler2D _MainTex2; // テクスチャ

		half   _Glossiness; // 光沢
		half   _Metallic;   // 金属特性
		fixed4 _Color;      // カラー
		float _Amount;
		float _Duration;
		float _Size;
		float _Speed;
		float _Ratio;
		float _Scale;
		float4 _Num;
		float3 _ObjectScale; // Boidオブジェクトのスケール
		float _GlobalIntensity;
		float4x4 _modelMatrix;

		// オイラー角（ラジアン）を回転行列に変換
		float4x4 eulerAnglesToRotationMatrix(float3 angles)
		{
			float ch = cos(angles.y); float sh = sin(angles.y); // heading
			float ca = cos(angles.z); float sa = sin(angles.z); // attitude
			float cb = cos(angles.x); float sb = sin(angles.x); // bank

			// Ry-Rx-Rz (Yaw Pitch Roll)
			return float4x4(
				ch * ca + sh * sb * sa, -ch * sa + sh * sb * ca, sh * cb, 0,
				cb * sa, cb * ca, -sb, 0,
				-sh * ca + ch * sb * sa, sh * sa + ch * sb * ca, ch * cb, 0,
				0, 0, 0, 1
			);
		}

		// 頂点シェーダ
		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

			// インスタンスIDからBoidのデータを取得
			CubeData cubeData = _CubeDataBuffer[unity_InstanceID]; 

			float3 pos = cubeData.position.xyz; // Boidの位置を取得
			//float3 scl = float3(_Size,_Size,_Size);//_ObjectScale;          // Boidのスケールを取得

			float ss = saturate(_Amount);
			//float3 scl = float3(_Size-ss*0.02,_Size-ss*0.02,_Size-ss*0.02);//_ObjectScale;          // Boidのスケールを取得

			float scaleRatio = 1.0 - cubeData.time/_Duration;
			scaleRatio = saturate(scaleRatio);//max(scaleRatio,0);
			//角度を計算するようにしよう

			float3 scl = float3(_Size,_Size,_Size) * scaleRatio;//_ObjectScale;          // Boidのスケールを取得

			//float4 cc1 = tex2Dlod( _MainTex, float4(_CubeDataBuffer[unity_InstanceID].uv,0,0) );
			//float4 cc2 = tex2Dlod( _MainTex2, float4(_CubeDataBuffer[unity_InstanceID].uv,0,0) );
			//float4 cc = lerp(cc1,cc2,_Ratio);


			// オブジェクト座標からワールド座標に変換する行列を定義
			//float4x4 object2world = (float4x4)0; 
			float4x4 object2world = float4x4(
				1,0,0,0,
				0,1,0,0,
				0,0,1,0,
				0,0,0,1
			);

			//float4x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)

			// スケール値を代入
			object2world._11_22_33_44 = float4(scl.xyz * float3(0.2,0.2,10+2*cubeData.basePos.z+_GlobalIntensity), 1.0);

			// 速度からY軸についての回転を算出
			float rotY = 
				atan2(cubeData.velocity.x, cubeData.velocity.z);
			// 速度からX軸についての回転を算出
			float rotX = 
				-asin(cubeData.velocity.y / (length(cubeData.velocity.xyz) + 1e-8));
			// オイラー角（ラジアン）から回転行列を求める
			float4x4 rotMatrix = eulerAnglesToRotationMatrix(float3(rotX, rotY, 0));

			// 行列に回転を適用
			object2world = mul(rotMatrix, object2world);
			// 行列に位置（平行移動）を適用

			//float zz = sin(length( cc.rgb )*3.14*2 + _Time.x * _Speed) * 3 * _Amount;

			object2world._14_24_34 += pos.xyz;// + float3(0,0,zz);

			// 頂点を座標変換

			//親のモデルマトリックスも噛ませます
			//object2world = mul(_modelMatrix, object2world);
			
			//Color取得
			//float3 cc = _CubeDataBuffer[unity_InstanceID].color.xyz;

			v.vertex = mul(object2world, v.vertex);
			
			// 法線を座標変換
			v.normal = normalize(mul(object2world, v.normal));

			//ここで色を渡している
			o.col = cubeData.color;//色
			//o.col = float4(_CubeDataBuffer[unity_InstanceID].uv,0,0);//uv

			#endif
		}
		
		void setup()
		{
		}

		// サーフェスシェーダ
		void surf (Input IN, inout SurfaceOutputStandard o)
		{

			//screen pos を受け取るようにする
			float2 uvv = IN.screenPos.xy / IN.screenPos.w;

			fixed4 c1 = IN.col;//tex2D ( _MainTex, uvv + 0.8*(IN.col.xy-float2(0.5,0.5)) );
			
			o.Albedo = c1.rgb*0.3;//tex2D (_MainTex, IN.col.xy).rgb;//lerp(c1.rgb,c2.rgb,_Ratio);//IN.col.xyz;//c.rgb;// * IN.color.xyz;
			o.Metallic = _Metallic;
			o.Emission = c1.rgb*0.7;
			o.Smoothness = _Glossiness;

		}
		ENDCG
	}
	FallBack "Diffuse"
}