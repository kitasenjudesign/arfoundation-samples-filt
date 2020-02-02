Shader "RDSystem/Update"
{
    Properties
    {
        _CamTex("_CamTex", 2D) = "black" {}
        _StencilTex("_StencilTex", 2D) = "black" {}
        _Du("Diffusion (u)", Range(0, 1)) = 1
        _Dv("Diffusion (v)", Range(0, 1)) = 0.4
        _Feed("Feed", Range(0, 0.1)) = 0.05
        _Kill("Kill", Range(0, 0.1)) = 0.05
        [Toggle] _Invert("_Invert", Float) = 0

    }

    CGINCLUDE

    #include "UnityCustomRenderTexture.cginc"
    #include "../util/StencilUV.hlsl"

    half _Du, _Dv;
    half _Feed, _Kill;
    sampler2D _CamTex;
    sampler2D _StencilTex;
    float _Invert;

    half4 frag(v2f_customrendertexture i) : SV_Target
    {
        float tw = 1 / _CustomRenderTextureWidth;
        float th = 1 / _CustomRenderTextureHeight;

        float2 uv = i.globalTexcoord;
        float4 duv = float4(tw, th, -tw, 0);

        half2 q = tex2D(_SelfTexture2D, uv).xy;

        float2 stencilUV = GetStencilUV( uv );
        float4 col = tex2D(_StencilTex, stencilUV);      

        if(_Invert==1) col.x = 1-col.x;//max(col.x,q.x);
        
        //uv.y -= 0.001;

        half2 dq = -q;
        dq += tex2D(_SelfTexture2D, uv - duv.xy).xy * 0.05;
        dq += tex2D(_SelfTexture2D, uv - duv.wy).xy * 0.20;
        dq += tex2D(_SelfTexture2D, uv - duv.zy).xy * 0.05;
        dq += tex2D(_SelfTexture2D, uv + duv.zw).xy * 0.20;
        dq += tex2D(_SelfTexture2D, uv + duv.xw).xy * 0.20;
        dq += tex2D(_SelfTexture2D, uv + duv.zy).xy * 0.05;
        dq += tex2D(_SelfTexture2D, uv + duv.wy).xy * 0.20;
        dq += tex2D(_SelfTexture2D, uv + duv.xy).xy * 0.05;

        half ABB = q.x * q.y * q.y;

        float4 cam = tex2D(_CamTex, uv);  
        float nn = (cam.r+cam.g+cam.b)*0.333;
        nn += _Time.x + col.x;
        nn = frac(nn);
        nn = floor(nn*8)/8;

        float dvv = _Dv + 0.6*nn;
        float feed = _Feed + 0.005*nn;
        float kill = _Kill + 0.005*nn;

        q += float2(dq.x * _Du - ABB + feed * (1 - q.x),
                    dq.y * dvv + ABB - (kill + _Feed) * q.y);

        return half4(saturate(q), 0, 0);
    }

    ENDCG
        /*



    half4 frag(v2f_customrendertexture i) : SV_Target
    {
        return 0;
    }

    EDNCG
        */

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            Name "Update"
            CGPROGRAM
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            ENDCG
        }
    }
}
