using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PostEffect : MonoBehaviour
{
    
    [SerializeField] private Shader _shader;
    [SerializeField] private ARHumanBodyManager occlusionManager;
    [SerializeField] private ARCameraBackground _arBackground;
    private Material _material;
    private RenderTexture _camTex;

    void Awake()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
        _camTex = new RenderTexture(960,720,0);
        _material = new Material(_shader);
    }

    
    private void OnGUI()
    {
        if(occlusionManager!=null){
            
            GUI.DrawTexture(
                new Rect(0, 0, 200, 200), 
                occlusionManager.humanStencilTexture, 
                ScaleMode.StretchToFill,
                false
            );
            GUI.DrawTexture(
                new Rect(0, 200, 200, 200), 
                occlusionManager.humanDepthTexture, 
                ScaleMode.StretchToFill,
                false
            );

        }
    }


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(occlusionManager!=null){
            _material.SetTexture("_StencilTex",occlusionManager.humanStencilTexture);
            _material.SetTexture("_DepthTex",occlusionManager.humanDepthTexture);
            Params.SetStencilAspect(
                occlusionManager.humanStencilTexture.width,
                occlusionManager.humanStencilTexture.height
            );            
        }

        
        Graphics.Blit(source, destination, _material);
    }

    void Update(){

        //Debug.Log(_arBackground.material);
        //from iPhone camera
        if(_arBackground!=null){
            if(_arBackground.material!=null) Graphics.Blit(null, _camTex, _arBackground.material);
            _material.SetTexture("_CamTex1", _camTex);   
        }

    }

}