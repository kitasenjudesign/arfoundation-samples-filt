using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSlitScan
{

    public const int MAX = 60;
    private List<RenderTexture> _frames;
    private RenderTexture _outputTex;
    //private Texture2D _inputTex;
    private Material _material;

    public SimpleSlitScan(
        int w, 
        int h, 
        RenderTextureFormat format, 
        Material mat
    ){

        _material = mat;

        //new RenderTexture(w,h,0,format);
        _frames = new List<RenderTexture>();

        int ww = Mathf.FloorToInt( Screen.width / 2 );
        int hh = Mathf.FloorToInt( Screen.height / 2 );

        _outputTex = new RenderTexture(
            ww,hh,0,format
        );
        

        for(int i=0;i<MAX;i++){
            _frames.Add( new RenderTexture(ww,hh,0,format) );
        }
    }

    //コピーするだけ
    public void UpdateFrame(Texture inputTex){

        var tgt =_frames[_frames.Count-1];
        Graphics.Blit(inputTex,tgt);
        _frames.RemoveAt(_frames.Count-1);
        _frames.Insert(0,tgt);

    }

    public RenderTexture Update(Texture inputTex){

        //新しいのを追加し、古いのを押し出していく
        UpdateFrame(inputTex);

        //描画
        for(int i=0;i<_frames.Count;i++){
            
            var num = (float)_frames.Count;
            var start = (float)i / num;
            var end  = ( (float)i+1f ) / num;

            _material.SetTexture("_MainTex",_frames[i]);
            _material.SetFloat("_Index",(float)i/(float)_frames.Count);
            _material.SetVector("_Pos",new Vector4(start,end,0,0) );
            Graphics.Blit(null,_outputTex,_material);//出力

        }

        return _outputTex;

    }

    public RenderTexture GetFrame(int idx){
        return _frames[idx];
    }


}
