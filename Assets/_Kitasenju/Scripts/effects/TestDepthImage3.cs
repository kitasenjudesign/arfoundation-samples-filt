using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TestDepthImage3 : MonoBehaviour
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
    private int _matIndex = 0;
    private Material _currentMat;
    private RenderTexture _oldTex;
    void Start(){ 
        _oldTex = new RenderTexture(512,256,0);
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

        Texture2D humanStencil = m_HumanBodyManager.humanStencilTexture;
        Texture2D humanDepth = m_HumanBodyManager.humanDepthTexture;

        m_RawImage.texture = humanStencil;//stencilを使う場合


        //CameraTexに書き出す
        Graphics.Blit(null, _cameraTex, _arBackground.material);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _updateMat();
        }

        //過去テクスチャを加味して結果に反映する
        _currentMat.SetTexture("_OldTex",_oldTex);
        _currentMat.SetTexture("_MainTex", _cameraTex );
        _currentMat.SetTexture("_DepthTex", humanDepth );

        _currentMat.SetFloat("_DepthTh", _slider.value );
        _currentMat.SetTexture("_StencilTex", humanStencil );

        //結果を過去テクスチャに書き込む
        Graphics.Blit(null,_oldTex,_currentMat);

    }

    private void _updateMat(){

        _currentMat = _materials[_matIndex % _materials.Length];
        _targetImg.material = _currentMat;
        _matIndex++;

    }

}
