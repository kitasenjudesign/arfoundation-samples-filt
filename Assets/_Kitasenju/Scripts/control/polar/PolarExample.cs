using UnityEngine;
using System.Collections;

public class PolarExample : MonoBehaviour
{
    public ComputeShader shader;

    RenderTexture tex;

    [SerializeField] private Texture2D _src;
    [SerializeField] private Texture2D _maskTex;

    void Start ()
    {

        tex = new RenderTexture(512, 512, 0);
        tex.enableRandomWrite = true;
        tex.Create();
        Graphics.Blit(_src,tex);

        shader.SetFloat("w", tex.width);
        shader.SetFloat("h", tex.height);
        shader.SetTexture(0, "tex", tex);//出力用
        shader.SetTexture(0,"baseTex",_src);
        shader.SetTexture(0,"maskTex",_maskTex);

        shader.Dispatch(0, tex.width/8, tex.height/8, 1);

    }

    void OnGUI()
    {
        int xx = Screen.width/2;
        int yy = Screen.height/2;
        int ww = 500;
        int hh = 500;

        GUI.DrawTexture(new Rect(0,0,ww,hh), tex, ScaleMode.StretchToFill);
    }

    void OnDestroy()
    {
        tex.Release();
    }
}