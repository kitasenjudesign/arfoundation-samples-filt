using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TestDepthImage2 : MonoBehaviour
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

    [SerializeField] RawImage m_RawImage;

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
    [SerializeField,Space(10)] private Material[] _materials;
    [SerializeField] private RenderTexture _cameraTex;

    [SerializeField] private Slider _slider;
    private int _matIndex = 0;
    private Material _currentMat;

    [SerializeField,Space(10)] private Image _targetImg;
    [SerializeField] private MeshRenderer _meshRenderer;    
    [SerializeField] private ImageEffectBase _imageEffect;

    void Start(){ 
        _updateMat();
    }

    void LogTextureInfo(StringBuilder stringBuilder, string textureName, Texture2D texture)
    {
        stringBuilder.AppendFormat("texture : {0}\n", textureName);
        if (texture == null)
        {
            stringBuilder.AppendFormat("   <null>\n");
        }
        else
        {
            stringBuilder.AppendFormat("   format : {0}\n", texture.format.ToString());
            stringBuilder.AppendFormat("   width  : {0}\n", texture.width);
            stringBuilder.AppendFormat("   height : {0}\n", texture.height);
            stringBuilder.AppendFormat("   mipmap : {0}\n", texture.mipmapCount);
        }
    }

    void Update()
    {
        var subsystem = m_HumanBodyManager.subsystem;
        if (subsystem == null)
        {
            if (m_ImageInfo != null)
            {
                m_ImageInfo.text = "Human Segmentation not supported.";
            }
            return;
        }

        StringBuilder sb = new StringBuilder();
        Texture2D humanStencil = m_HumanBodyManager.humanStencilTexture;
        Texture2D humanDepth = m_HumanBodyManager.humanDepthTexture;
        //LogTextureInfo(sb, "stencil", humanStencil);
        //LogTextureInfo(sb, "depth", humanDepth);

        if (m_ImageInfo != null)
        {
            m_ImageInfo.text = sb.ToString();
        }
        else
        {
            Debug.Log(sb.ToString());
        }

        // To use the stencil, be sure the HumanSegmentationStencilMode property on the ARHumanBodyManager is set to a
        // non-disabled value.
        m_RawImage.texture = humanStencil;//stencilを使う場合

        // To use the depth, be sure the HumanSegmentationDepthMode property on the ARHumanBodyManager is set to a
        /// non-disabled value.
        //m_RawImage.texture = eventArgs.humanDepth;
        

        //humanStencilを使用
      


        //カメラのテクスチャを使用
        //Graphics.Blit(null, m_MyRenderTexture, _arBackground.material);
        //_arBackground.material;

        //上記の二つを混ぜ 
        //_outputMat

        //CameraTexに書き出す
        //Graphics.Blit(null, _cameraTex, _arBackground.material);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _updateMat();
        }

        //_currentMat.SetTexture("_MainTex", _cameraTex );
        _currentMat.SetTexture("_DepthTex", humanDepth );
        _currentMat.SetFloat("_DepthTh", _slider.value );
        _currentMat.SetTexture("_StencilTex", humanStencil );

    }

    private void _updateMat(){

        _currentMat = _materials[_matIndex % _materials.Length];
        
        if(_targetImg)       _targetImg.material = _currentMat;
        if(_meshRenderer)   _meshRenderer.sharedMaterial = _currentMat;
        if(_imageEffect)    _imageEffect.material = _currentMat;

        _matIndex++;

    }

}
