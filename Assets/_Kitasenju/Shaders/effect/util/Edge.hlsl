float GetEdge(sampler2D tex, float2 uv, float2 duv){


                half2 uv0 = uv + half2(-duv.x, -duv.y);
                half2 uv1 = uv + half2(0, -duv.y);
                half2 uv2 = uv + half2(duv.x, -duv.y);
                half2 uv3 = uv + half2(-duv.x, 0);
                half2 uv4 = uv + half2(0, 0);
                half2 uv5 = uv + half2(duv.x, 0);
                half2 uv6 = uv + half2(-duv.x, duv.y);
                half2 uv7 = uv + half2(0, duv.y);
                half2 uv8 = uv + half2(duv.x, duv.y);

                half3 col0 = tex2D(tex, uv0);
                half3 col1 = tex2D(tex, uv1);
                half3 col2 = tex2D(tex, uv2);
                half3 col3 = tex2D(tex, uv3);
                half3 col4 = tex2D(tex, uv4);
                half3 col5 = tex2D(tex, uv5);
                half3 col6 = tex2D(tex, uv6);
                half3 col7 = tex2D(tex, uv7);
                half3 col8 = tex2D(tex, uv8);
 
//#ifdef Sobel
//                float cgx = col0 * -1 + col2 * 1 + col3 * -2 + col5 * 2 + col6 * -1 + col8 * 1;
//                float cgy = col0 * -1 + col1 * -2 + col2 * -1 + col6 * 1 + col7 * 2 + col8 * 1;
//                float cg = sqrt(cgx * cgx + cgy * cgy);
//#elif Prewitt
//                float cgx = col0 * -1 + col2 * 1 + col3 * -1 + col5 * 1 + col6 * -1 + col8 * 1;
//                float cgy = col0 * -1 + col1 * -1 + col2 * -1 + col6 * 1 + col7 * 1 + col8 * 1;
//                float cg = sqrt(cgx * cgx + cgy * cgy);
//#elif RobertsCross
                float3 cg1 = col8 - col0;
                float3 cg2 = col6 - col2;
                float cg = sqrt(dot(cg1, cg1) + dot(cg2, cg2));
//#endif
    return cg;

}