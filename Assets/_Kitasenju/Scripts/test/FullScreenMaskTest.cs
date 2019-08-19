using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;




public class FullScreenMaskTest : MonoBehaviour
{

    [SerializeField] private ARHumanBodyManager _humanBodyManager;
    [SerializeField] private Material _quadMat;
    [SerializeField] private ARCameraBackground _arBackground;
    [SerializeField] private ARCameraManager _cameraManager;
    [SerializeField] private string _name;
    private Material _arBgMat;
    private RenderTexture _renderTexture;

    // Start is called before the first frame update
    void Start()
    {
       
        _renderTexture = new RenderTexture(Screen.width,Screen.height,0);
        
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_arBackground);
        //Debug.Log(_humanBodyManager);
        
        if(_cameraManager){
            _name = _cameraManager.shaderName;
        }

        //書き込む
        if( _arBackground.material != null ){

            Graphics.Blit(
                null,
                _renderTexture,
                _arBackground.material
            );

        }

        //quadに渡す
        _quadMat.SetTexture("_MainTex", _renderTexture);

        if(_humanBodyManager.humanStencilTexture){
            _quadMat.SetTexture("_StencilTex", _humanBodyManager.humanStencilTexture);
        }

    }
}
