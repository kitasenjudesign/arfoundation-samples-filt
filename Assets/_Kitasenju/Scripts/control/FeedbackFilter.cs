using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FeedbackFilter : FilterBase
{
    
    [SerializeField] public Material _material;
    [SerializeField] private RenderTexMaker _texMaker;    

    protected bool _invert = false;
    public override void Show(EffectControlMain main){
        
        base.Show(main);
        _texMaker.enabled=true;
        _main.SetImageEffect(_material);
        UpdateFilter();
        
    }

    public override void Hide(){
        
        _texMaker.enabled = false;
        base.Hide();

    }


    public override void SetInvert(bool b){

        _invert = b;
        UpdateFilter();

    }

    public override void UpdateFilter(){

        //Debug.Log(_invert);
        Texture2D humanStencil  = _main._humanBodyManager.humanStencilTexture;
        Texture2D humanDepth    = _main._humanBodyManager.humanDepthTexture;

        //invertをセット
        _material.SetFloat("_Invert",_invert?1f:0);
        //main texture
        _main.SetCamToMainTex(_material,_hasBlur);
        _material.SetTexture("_DepthTex", humanDepth );

        if(_main._menu){
            _material.SetVector("_Th", 
                new Vector4(
                    _main._menu._slider1.value,
                    _main._menu._slider2.value,
                    0,
                    0
                )
            );
        }
        
        _material.SetTexture("_StencilTex", humanStencil );
        _material.SetTexture("_MainTex2",_texMaker._tex);//背景用

    }

    public override Material GetMaterial(){
        return _material;
    }


}