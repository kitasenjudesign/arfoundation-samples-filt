using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class RenderTexManager
{

    private bool _isInit = false;

    public RenderTexStocker _slitscanColor;
    public RenderTexStocker _slitscanStencil;
    public RenderTexture _renderTex;
    public RenderTexture _inputTex;
    public RenderTexture _outputTex;//
    public RenderTexture _outputTexTmp;//
    private int _count = 0;

    public void Init(){

        if( _isInit ) return;
        _isInit = true;

        
        _inputTex = new RenderTexture(
            Screen.width,
            Screen.height,
            0
        );
        _outputTex = new RenderTexture(
            Screen.width,
            Screen.height,
            0
        );
        _outputTexTmp = new RenderTexture(
            Screen.width,
            Screen.height,
            0
        );

        _slitscanColor = new RenderTexStocker(
            480,//640,
            640,//480,
            RenderTextureFormat.ARGB32
        );
        _slitscanStencil = new RenderTexStocker(
            720,//960,
            960,//720,
            RenderTextureFormat.R8
        );

    }

    public RenderTexture GetNextColorTexture(){
        
        Init();
        _count++;
        return _slitscanColor.GetFrame(_count);

    }


}