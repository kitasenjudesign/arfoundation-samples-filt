using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTexMaker : MonoBehaviour
{
    [SerializeField] public RenderTexture _tex;
    [SerializeField] private Shader _shader;
    private Material _mat;
    void Start(){

        _mat = new Material(_shader);

    }

    private void OnGUI()
    {
        /*
        GUI.DrawTexture(
            new Rect(0, 0, 200, 200), 
            _tex, 
            ScaleMode.StretchToFill,
            false
        );
        */
    }

   
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, _tex);//srcをtexにコピー
        Graphics.Blit(source, destination);
    }

}
