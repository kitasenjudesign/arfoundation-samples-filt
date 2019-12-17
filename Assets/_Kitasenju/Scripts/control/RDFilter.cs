using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class RDFilter : FilterBase
{
    
    [SerializeField] public Material _material;
    [SerializeField] public Material _rdUpdaterMat;
    protected bool _invert = false;
    public override void Show(EffectControlMain main){
        
        base.Show(main);
        _main.SetImageEffect(_material);
        UpdateFilter();
    }

    private void OnGUI()
    {
            
            GUI.DrawTexture(
                new Rect(0, 0, 562/2, 1218/2), 
                _main._camTex, 
                ScaleMode.StretchToFill,
                false
            );
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
        _rdUpdaterMat.SetTexture("_CamTex", _main._camTex );
        _rdUpdaterMat.SetTexture("_StencilTex", humanStencil );

    }

    public override Material GetMaterial(){
        return _material;
    }


}