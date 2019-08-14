using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDepthImage6test : MonoBehaviour
{
    [SerializeField] private Texture2D _baseTex;
    private RenderTexture _renderTexture1;
    private RenderTexture _renderTexture2;
    [SerializeField] private Shader _shader;
    private Material _mat;
    private bool _isPingPong = false;
    // Start is called before the first frame update
    void Start()
    {
        _mat = new Material(_shader);
        _renderTexture1=new RenderTexture(256,256,0);
        _renderTexture2=new RenderTexture(256,256,0);

        
        Graphics.Blit(_baseTex,_renderTexture1);
        Graphics.Blit(_baseTex,_renderTexture2);


    }

    // Update is called once per frame
    void Update()
    {
        //_matの色をだんだん薄くしていく
        var r1 = _isPingPong ? _renderTexture1 : _renderTexture2;
        var r2 = _isPingPong ? _renderTexture2 : _renderTexture1;

        //input
        _mat.SetTexture("_MainTex", r1);
                
        //output
        Graphics.Blit(null,r2,_mat);
        GetComponent<MeshRenderer>().material.mainTexture = r2;

        _isPingPong = !_isPingPong;
    }


}
