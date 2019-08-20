﻿Shader "test/KeywordTest"
{
    Properties{
        [KeywordEnum(RED,GREEN,BLUE)]
        _COLOR("COLOR KEYWORD", Float) = 0
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile _COLOR_RED _COLOR_GREEN _COLOR_BLUE

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = float4(0, 0, 0, 1);
                #ifdef _COLOR_RED
                    col = float4(1, 0, 0, 1);
                #elif _COLOR_GREEN
                    col = float4(0, 1, 0, 1);
                #elif _COLOR_BLUE
                    col = float4(0, 0, 1, 1);
                #endif
                return col;
            }
            ENDCG
        }
    }
}