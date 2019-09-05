using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class RandomFilter : FilterBase
{
    
    [SerializeField] public Material[] _materials;
    private Material _material;
    [SerializeField] private bool _invert = false;
    private int _index=0;
    public override void Show(EffectControlMain main){
        
        base.Show(main);
        _main.SetImageEffect(_materials[0]);
        
        _startLoop();
        UpdateFilter();
    }

    public override void Hide(){
        
        CancelInvoke("_startLoop");
        gameObject.SetActive(false);

    }

    private void _startLoop(){
        if(!gameObject.activeSelf) return;

        _material = _materials[ _index % _materials.Length ];
        _index++;
        //SetInvert(
        //    Random.value < 0.5f ? true : false
        //);
        _main.SetImageEffect(_material);
        Invoke("_startLoop",0.75f);

    }


    public override void SetInvert(bool b){

        _invert = b;
        
    }

    public override void UpdateFilter(){

        Texture2D humanStencil  = _main._humanBodyManager.humanStencilTexture;
        Texture2D humanDepth    = _main._humanBodyManager.humanDepthTexture;

        _material.SetFloat("_Invert",_invert?1f:0);

        _main.SetCamToMainTex(_material);
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

    }




}