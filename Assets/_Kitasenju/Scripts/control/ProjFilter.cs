using UnityEngine;
using UnityEngine.XR.ARFoundation;
using NatCorder.Examples;

public class ProjFilter : FilterBase
{
    
    [SerializeField] public Material _material;
    [SerializeField] private bool _invert = false;
    [SerializeField] private ProjObjs _projObjs;
    [SerializeField] private Camera _camera1;
    [SerializeField] private Camera _camera2;
    [SerializeField] private GameObject _fullQuad;
    [SerializeField] private MyReplayCam _replayCam;


    public override void Show(EffectControlMain main){

        _camera1.enabled=true;
        _camera2.enabled=false;
        _replayCam.cam = _camera1;
        _fullQuad.gameObject.SetActive(false);

        base.Show(main);
        _main.SetImageEffect(_material);
        _projObjs.Init( _main );
        UpdateFilter();
    }
    public override void Hide(){
        
        _replayCam.cam = _camera2;
        _camera1.enabled=false;
        _camera2.enabled=true;
        _fullQuad.gameObject.SetActive(true);

        gameObject.SetActive(false);

    }

    public override void SetInvert(bool b){

        _invert = b;
        UpdateFilter();

    }

    public override void UpdateFilter(){

        //


    }




}