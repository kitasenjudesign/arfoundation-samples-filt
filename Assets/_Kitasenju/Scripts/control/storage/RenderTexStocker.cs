using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTexStocker
{

    public const int MAX = 60;
    private List<RenderTexture> _frames;
    private RenderTexture _outputTex;
    //private Texture2D _inputTex;

    public RenderTexStocker(
        int w, 
        int h, 
        RenderTextureFormat format
    ){
        //new RenderTexture(w,h,0,format);
        _frames = new List<RenderTexture>();

        int ww = w;//Mathf.FloorToInt( Screen.width / 2 );
        int hh = h;//Mathf.FloorToInt( Screen.height / 2 );

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


    public RenderTexture GetFrame(int idx){
        return _frames[idx];
    }


}
