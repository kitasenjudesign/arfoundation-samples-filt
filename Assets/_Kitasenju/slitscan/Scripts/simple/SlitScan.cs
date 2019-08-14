using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlitScan : MonoBehaviour
{

    private List<RenderTexture> frames;
    private const int MAX = 100;
    [SerializeField,Range(1,100)]private float _numFrame = MAX;
    private WebCamTexture _webcamTex;
    private int _index = 0;
    [SerializeField] private Material _mat;
    [SerializeField] private RenderTexture _renderTex;
    [SerializeField] private Texture[] _maps;
    private int _mapIndex = 0;
    private bool _isBackCamera = false;

    private int[] _shaderIdTh = {
        Shader.PropertyToID("_Th1"),
        Shader.PropertyToID("_Th2"),
        Shader.PropertyToID("_Th3"),
        Shader.PropertyToID("_Th4"),
        Shader.PropertyToID("_Th5")
    };
    private int[] _shaderIdTex = {
        Shader.PropertyToID("_MainTex1"),
        Shader.PropertyToID("_MainTex2"),
        Shader.PropertyToID("_MainTex3"),
        Shader.PropertyToID("_MainTex4"),
        Shader.PropertyToID("_MainTex5")
    };

    void Start(){

        Screen.SetResolution(Screen.width / 2, Screen.height / 2, FullScreenMode.FullScreenWindow);

        _setCamera(true);

        frames = new List<RenderTexture>();

        int ww = Mathf.FloorToInt( Screen.width / 2 );
        int hh = Mathf.FloorToInt( Screen.height / 2 );

        for(int i=0;i<MAX;i++){
            frames.Add( new RenderTexture(ww,hh,0) );
        }

    }


    public void ToggleCamera(){
        _setCamera( !_isBackCamera );
    }

    private void _setCamera(bool isBack){
        _isBackCamera=isBack;
        if(_webcamTex){
            _webcamTex.Stop();
        }
        var idx = isBack ? 0 : 1;
        WebCamDevice[] devices = WebCamTexture.devices;
        _webcamTex = new WebCamTexture(devices[idx].name, 960, 540, 60);
        _webcamTex.Play();
    }

    public void SetDuration(float ratio){
        _numFrame = Mathf.FloorToInt( ratio*MAX );
        if(_numFrame<1)_numFrame=1;
    }

    public void NextEffect(){
        _mapIndex++;
    }

    public void PrevEffect(){
        _mapIndex--;
        if(_mapIndex<0)_mapIndex=_maps.Length-1;
    }


    void Update(){
        
        //camera rot
        //transform.localEulerAngles = new Vector3(0, 0, -1 * (float) _webcamTex.videoRotationAngle);
        //transform.localRotation = Quaternion.Euler(0,0,_webcamTex.videoRotationAngle+180f);

        //update frames
        var tgt =frames[frames.Count-1];
        Graphics.Blit(_webcamTex,tgt);
        frames.RemoveAt(frames.Count-1);
        frames.Insert(0,tgt);

        //update rendertex
        for(int i=0;i<_numFrame;i+=5){

            UpdateFrame(i+0,_shaderIdTh[0],_shaderIdTex[0]);
            UpdateFrame(i+1,_shaderIdTh[1],_shaderIdTex[1]);
            UpdateFrame(i+2,_shaderIdTh[2],_shaderIdTex[2]);
            UpdateFrame(i+3,_shaderIdTh[3],_shaderIdTex[3]);
            UpdateFrame(i+4,_shaderIdTh[4],_shaderIdTex[4]);

            _mat.SetTexture("_MainTex",_renderTex);
            Graphics.Blit(null,_renderTex,_mat);//出力

        }

        //change time displacement map
        #if UNITY_EDITOR
            if(Input.touchCount>=1){
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    NextEffect();
                }
            }

            if(Input.GetKeyDown(KeyCode.Space)){
                    NextEffect();
            }
        #endif

        _mat.SetTexture("_Displacement",_maps[_mapIndex%_maps.Length]);


    }

    void UpdateFrame(int i, int thName, int imgName){
            
        if(frames[i]==null)return;
        if(i>=frames.Count)return;
        
        var f = frames[i];

        var min = (float)i / (float)_numFrame;
        var max = ((float)i+1f) / (float)_numFrame;

        _mat.SetVector(thName,new Vector4(min,max,0,0));
        _mat.SetTexture(imgName, f);

    }

}