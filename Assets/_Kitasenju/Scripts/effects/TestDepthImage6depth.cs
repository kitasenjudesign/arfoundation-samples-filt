using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TestDepthImage6depth : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The ARHumanBodyManager which will produce frame events.")]
    ARHumanBodyManager m_HumanBodyManager;

     /// <summary>
    /// Get or set the <c>ARHumanBodyManager</c>.
    /// </summary>
    public ARHumanBodyManager humanBodyManager
    {
        get { return m_HumanBodyManager; }
        set { m_HumanBodyManager = value; }
    }

    [SerializeField]
    RawImage m_RawImage;

    /// <summary>
    /// The UI RawImage used to display the image on screen.
    /// </summary>
    public RawImage rawImage
    {
        get { return m_RawImage; }
        set { m_RawImage = value; }
    }

    [SerializeField]
    Text m_ImageInfo;

    /// <summary>
    /// The UI Text used to display information about the image on screen.
    /// </summary>
    public Text imageInfo
    {
        get { return m_ImageInfo; }
        set { m_ImageInfo = value; }
    }


    [SerializeField] private ARCameraBackground _arBackground;
    [SerializeField] private Material[] _materials;
    [SerializeField] private RenderTexture _cameraTex;
    [SerializeField] private Image _targetImg;
    [SerializeField] private Slider _slider;
    [SerializeField] private Slider _slider2;

    [SerializeField,Space(20)] private Shader _slitRecShader;
    [SerializeField] private Shader _fillShader;
    [SerializeField] private Shader _depthMapMakeShader;

    private Material _slitRecMat;
    private Material _fillMat;
    private int _matIndex = 0;
    private Material _currentMat;
    private Material _depthMakeMat;

    private bool _isInit = false;

    private SimpleSlitScan _slitscanColor;
    private SimpleSlitScan _slitscanStencil;
    private SimpleSlitScan _slitscanDepth;

    private RenderTexture _renderTex;
    private RenderTexture _depthTex1;
    private RenderTexture _depthTex2;
    private bool _isPingPong=false;

    private GUIStyle style;
    private bool _isDebug = true;
    private bool _isActive = false;
    private float _ratio = 0;

    private RenderTexture _clearTex;

    void Start(){ 

        _renderTex      = new RenderTexture(512,256,0);

        _depthTex1      = new RenderTexture(256,192,0);
        _depthTex2      = new RenderTexture(256,192,0);

        _clearTex= new RenderTexture(256,192,0);

        _depthMakeMat   = new Material(_depthMapMakeShader);
        _slitRecMat     = new Material(_slitRecShader);
        _fillMat        = new Material(_fillShader);
        
        _updateMat();
    }

   private void OnGUI()
    {
        if(!_isDebug) return;

        if(style==null){
            style = new GUIStyle();
            style.fontSize = 50;
            style.normal.textColor = Color.white;                
        }

            GUI.Label(
                new Rect(100, 100, 500, 100), 
                "b1 "+ _fillMat.GetFloat("_Bai") + " " +
                "b2 "+ _fillMat.GetFloat("_Bai2")
                ,style
            );
        
            GUI.DrawTexture(
                new Rect(50, 50, 320, 240), 
                _isPingPong ? _depthTex1 : _depthTex2,
                ScaleMode.StretchToFill,
                false
            ); 
        
    }       



    void Update()
    {
        
        Texture2D humanStencil = m_HumanBodyManager.humanStencilTexture;
        Texture2D humanDepth = m_HumanBodyManager.humanDepthTexture;
        Graphics.Blit(null, _cameraTex, _arBackground.material);

        if(!_isInit){
            _isInit=true;
        
            _slitscanColor = new SimpleSlitScan(
                640,
                480,
                RenderTextureFormat.ARGB32,
                _slitRecMat
            );
            _slitscanStencil = new SimpleSlitScan(
                640,
                480,
                RenderTextureFormat.R8,
                _slitRecMat
            );
            _slitscanDepth = new SimpleSlitScan(
                256,
                192,
                RenderTextureFormat.RFloat,
                _slitRecMat
            );
            //初期化

        }else{
            //スリットスキャンをアップデート

            //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            //{
            //    _updateMat();
            //}

            //フレームを保存しておく
            _slitscanColor.UpdateFrame(_cameraTex);
            _slitscanStencil.UpdateFrame(humanStencil);
            _slitscanDepth.UpdateFrame(humanDepth);

            //そのうち２フレームを抜粋する
            var camIdx = new int[]{0,5,10,15,20,25,30,35,40,45,50,59};

            //一番背景に、カメラを塗る
            //Graphics.Blit( _cameraTex, _renderTex );

            //手前に、数フレームぶん、マスクした人を貼り付ける

            if(_isActive){
                _ratio+=0.04f;
            }else{
                _ratio-=0.04f;
            }

            if(_ratio < 0) _ratio = 0;
            if(_ratio > 1f) _ratio = 1f;

            /*
            A.rとB.rを比較する
            大きい方の gにidをいれる
            大きい値を rに入れる
            を繰り返し、一枚のテクスチャdepthを作る

            Idに応じて、テクスチャを得る
            depthのIdだったら描画、それ以外はclip
            を繰り返す            
            */

            //出力を二個用意してピンポンする


            //clearする

           

            Graphics.Blit( _clearTex,_depthTex1 );
            Graphics.Blit( _clearTex,_depthTex2 );

            var inputDepth = _depthTex1;
            var outputDepth = _depthTex2;
            var len =camIdx.Length;

            for(int i=0;i<len;i++){

                inputDepth = _isPingPong ? _depthTex1 : _depthTex2;
                outputDepth = _isPingPong ? _depthTex2 : _depthTex1;

                var frame1 = _slitscanDepth.GetFrame( camIdx[i] );
                var r1 = (float)i/(float)camIdx.Length;

                _depthMakeMat.SetTexture("_Depth",inputDepth);
                _depthMakeMat.SetTexture("_Depth1",frame1);
                //_depthMakeMat.SetTexture("_Depth2",frame2);

                _depthMakeMat.SetVector("_Th",new Vector4(r1,0,0,0));

                Graphics.Blit( null, outputDepth, _depthMakeMat );
                _isPingPong = !_isPingPong;

            }



            //ピンポンしたテクスチャにoutputDepthに応じてfillしていく
            
            for(int i=0;i<len;i++){
                var r1 = (float)i/(float)camIdx.Length;
                var r2 = ( (float)(i+1f) ) / (float)camIdx.Length;

                //値を塗る
                _fillMat.SetTexture("_DepthTex",outputDepth);
                _fillMat.SetTexture("_MainColTex", _slitscanColor.GetFrame(camIdx[i]) );
                _fillMat.SetTexture("_StencilTex", _slitscanStencil.GetFrame(camIdx[i]) );
                _fillMat.SetVector("_Th", new Vector4(r1,r2,0,0)  );

                Graphics.Blit( null, _cameraTex, _fillMat );

            }   
            _fillMat.SetFloat("_Bai",_slider.value);
            _fillMat.SetFloat("_Bai2",_slider2.value);
            
            _targetImg.material.SetTexture("_MainTex", _cameraTex);

            //_currentMat.SetTexture("_DepthTex", humanDepth );
            //_currentMat.SetFloat("_DepthTh", _slider.value );

            //


        }

        _slider.gameObject.SetActive(  _isDebug );
        _slider2.gameObject.SetActive( _isDebug );

        if( Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began ){
            _isActive = !_isActive;
        }

        if (Input.touchCount >= 2 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _isDebug = !_isDebug;
        }

    }

    private void _updateMat(){

        _currentMat = _materials[_matIndex % _materials.Length];
        _targetImg.material = _currentMat;
        _matIndex++;

    }

}
