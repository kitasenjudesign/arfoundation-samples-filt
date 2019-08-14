using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TestDepthImage4slit : MonoBehaviour
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
    [SerializeField,Space(20)] private Shader _slitRecShader;
    private Material _slitRecMat;

    private int _matIndex = 0;
    private Material _currentMat;
    private bool _isInit = false;

    private SimpleSlitScan _slitscanColor;
    private SimpleSlitScan _slitscanStencil;



    void Start(){ 
        _slitRecMat = new Material(_slitRecShader);
        _updateMat();
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
                RenderTextureFormat.ARGB32,
                _slitRecMat
            );

            //初期化

        }else{
            //スリットスキャンをアップデート

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _updateMat();
            }

            var camTex = _slitscanColor.Update(_cameraTex);
            //var stencilTex = _slitscanStencil.Update(humanStencil);

            _currentMat.SetTexture("_MainTex", _cameraTex );
            _currentMat.SetTexture("_MainSlitTex", camTex);
            _currentMat.SetTexture("_StencilTex", humanStencil );//stencilTex );


            //_currentMat.SetTexture("_DepthTex", humanDepth );
            _currentMat.SetFloat("_DepthTh", _slider.value );

        }




    }

    private void _updateMat(){

        _currentMat = _materials[_matIndex % _materials.Length];
        _targetImg.material = _currentMat;
        _matIndex++;

    }

}
