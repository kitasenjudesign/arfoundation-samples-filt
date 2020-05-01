Shader "Custom/CubemappingSkybox"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Roughness ("_Roughness", float) = 1
        _CubeTex("_CubeTex", CUBE) = "" {}
	}

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex: POSITION;
                half3 normal: NORMAL;
                half2 uv: TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                float3 pos2 : TEXCOORD1;
                half3 normal : TEXCOORD2;
            };

            fixed4 _Color;
            float _Roughness;
            samplerCUBE _CubeTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.pos2 = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);
                half3 viewDir = normalize(_WorldSpaceCameraPos - i.pos2);
                half3 reflDir = reflect(-viewDir, i.normal);

                // キューブマップと反射方向のベクトルから反射先の色を取得する
                //half4 refColor = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflDir);

                // samplerCUBE _Tex;

                half4 refColor = UNITY_SAMPLE_TEXCUBE_LOD(
                    unity_SpecCube0,
                    //_CubeTex,
                    reflDir,
                    _Roughness    
                );


                return half4(
                    lerp(refColor,_Color,0.8)
                    //max(refColor.rgb*0.7,_Color.rgb), 1
                );
            }
            ENDCG
        }
    }
}