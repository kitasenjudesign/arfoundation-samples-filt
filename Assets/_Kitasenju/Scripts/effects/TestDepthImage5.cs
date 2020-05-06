using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TestDepthImage5 : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The ARHumanBodyManager which will produce frame events.")]
    AROcclusionManager m_HumanBodyManager;

     /// <summary>
    /// Get or set the <c>ARHumanBodyManager</c>.
    /// </summary>
    public AROcclusionManager humanBodyManager
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
    private Material _slitRecMat;
    private Material _fillMat;
    private int _matIndex = 0;
    private Material _currentMat;
    private bool _isInit = false;

    private SimpleSlitScan _slitscanColor;
    private SimpleSlitScan _slitscanStencil;

    private RenderTexture _renderTex;
    private GUIStyle style;
    private bool _isDebug = true;
    private bool _isActive = false;
    private float _ratio = 0;

    void Start(){ 

        _renderTex      = new RenderTexture(512,256,0);
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
                960,
                720,
                RenderTextureFormat.R8,
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

            //そのうち２フレームを抜粋する
            var camIdx = new int[]{0,5,10,15,20,25,30,35,40,45,50,59};
            var stencilIdx = new int[]{0,5,10,15,20,25,30,35,40,45,50,59};

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


            var len = Mathf.FloorToInt( camIdx.Length * _ratio );
            for(int i=0;i<len;i++){

                _fillMat.SetTexture("_MainColTex", _slitscanColor.GetFrame(camIdx[len-i-1]) );
                _fillMat.SetTexture("_StencilTex", _slitscanStencil.GetFrame(stencilIdx[len-i-1]) );

                Graphics.Blit( null, _cameraTex, _fillMat );

            }   
            _fillMat.SetFloat("_Bai",_slider.value);
            _fillMat.SetFloat("_Bai2",_slider2.value);
            
            _targetImg.material.SetTexture("_MainTex", _cameraTex);

            //_currentMat.SetTexture("_DepthTex", humanDepth );
            //_currentMat.SetFloat("_DepthTh", _slider.value );

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
