// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "effects/hatch/SingleObjectHatch"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		_Hatch0("Hatch 0", 2D) = "white" {}
		_Hatch1("Hatch 1", 2D) = "white" {}

        _Th("_Th",Vector) = (0,0,0,0)
        _StencilTex ("_StencilTex", 2D) = "white" {}   

        [Toggle] _Revert("_Revert", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
						
            #include "../noise/SimplexNoise3D.hlsl"
			#include "../util/StencilUV.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 norm : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 nrm : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _StencilTex;

			sampler2D _Hatch0;
			sampler2D _Hatch1;
			float _Revert;
			float4 _LightColor0;
			
			float4 _Th;

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				o.nrm = mul(float4(v.norm, 0.0), unity_WorldToObject).xyz;
				return o;
			}

			fixed3 Hatching(float2 _uv1, float2 _uv2, half _intensity)
			{
				half3 hatch0 = tex2D(_Hatch0, _uv1).rgb;
				half3 hatch1 = tex2D(_Hatch1, _uv2).rgb;

				half3 overbright = max(0, _intensity - 1.0);

				half3 weightsA = saturate((_intensity * 6.0) + half3(-0, -1, -2));
				half3 weightsB = saturate((_intensity * 6.0) + half3(-3, -4, -5));

				weightsA.xy -= weightsA.yz;
				weightsA.z -= weightsB.x;
				weightsB.xy -= weightsB.zy;

				hatch0 = hatch0 * weightsA;
				hatch1 = hatch1 * weightsB;

				half3 hatching = overbright + hatch0.r +
					hatch0.g + hatch0.b +
					hatch1.r + hatch1.g +
					hatch1.b;

				return hatching;

			}

			float rand(float2 co){
				return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
			}

			float2 rotate(float2 uv, float rad){

				float sinX = sin ( rad );
				float cosX = cos ( rad );
				float sinY = sin ( rad );
				float2x2 rotationMatrix = float2x2( cosX, -sinX, sinY, cosX);
				return mul ( uv, rotationMatrix );

			}


			fixed4 frag (v2f i) : SV_Target
			{

				fixed4 color = tex2D(_MainTex, i.uv);
				fixed3 diffuse = color.rgb;// * _LightColor0.rgb * dot(_WorldSpaceLightPos0, normalize(i.nrm));

				fixed intensity = dot(diffuse, fixed3(0.2326, 0.7152, 0.0722));
				
				//
				float scl = 4;
				//fixed2 rotUV1 = rotate( i.uv * scl, sin( floor(_Time.y*2.0) * 999.9 )*0.8  );
				//fixed2 rotUV2 = rotate( i.uv * scl, sin( floor(_Time.y*2.0) * 999.9 )*1.1  );
				float tt = floor(_Time.y * 4)*10;
				float detail = 0.005+4*(0.5 + 0.5*sin(tt));
				//fixed2 rotUV1 = rotate( i.uv * scl, floor( snoise( float3(i.uv*detail+intensity, tt ) ) * 20 ) / 20 * 2*3.14 );
				//fixed2 rotUV2 = rotate( i.uv * scl, floor( snoise( float3(i.uv*detail+intensity, tt ) ) * 20 ) / 20 * 2*3.14 );

				float2 fuv = i.uv * float2( 1, _ScreenParams.y/_ScreenParams.x );
				fixed2 rotUV1 = rotate(
					 fuv * scl, 
					 floor( snoise( float3(fuv.x*detail,fuv.y*detail+intensity*0.1, tt ) ) * 20 ) / 20 * 2*3.14 
				);
				fixed2 rotUV2 = rotate(
					 fuv * scl, 
					 floor( snoise( float3(fuv.y*detail,fuv.y*detail+intensity*0.1, tt ) ) * 20 ) / 20 * 2*3.14 
				);

				color.rgb =  Hatching( rotUV1, rotUV2, intensity);



				
				fixed4 col0 = tex2D(_MainTex, i.uv);

				//stencil
                float2 stencilUV = GetStencilUV( i.uv );
                fixed4 stencil = tex2D(_StencilTex, stencilUV);
				
				fixed4 outCol = lerp( col0, color, step(0.5,stencil.r) );
				
				if( _Revert == 1 ){
					//col0.rgb = (col0.r+col0.g+col0.b)/3;
                    outCol = lerp( col0, color, step(0.5,1-stencil.r) );
                }



				return outCol;
			}
			ENDCG
		}
	}
}
