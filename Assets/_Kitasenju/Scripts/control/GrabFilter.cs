using UnityEngine;
using UnityEngine.XR.ARFoundation;
using NatCorder.Examples;
using DG.Tweening;

public class GrabFilter : FilterBase
{
    
    [SerializeField,Space(10)] public Material _fullBgMaterial;
    [SerializeField] private bool _invert = false;
    //[SerializeField] private ProjObjs _projObjs;

    [SerializeField] private GameObject _fullQuad;

    //[SerializeField] private MyReplayCam _replayCam;
    [SerializeField] private RenderTexMaker _texMaker;
    [SerializeField] private FullScreenQuadByShader _myFullScreen;
    [SerializeField] private CameraMotionVector _camMotion;
    private int _count = 0;
    private bool _flag = false;

    public override void Show(EffectControlMain main){

        DataManager.Instance.InitTexStorage();
        _fullQuad.gameObject.SetActive(false);
        //_texMaker.enabled=true;
        base.Show(main);
        //_main.SetImageEffect(_material);
        //_projObjs.Init( _main );
        UpdateFilter();

    }

    public override void Hide(){
        
        _fullQuad.gameObject.SetActive(true);
        //_texMaker.enabled=false;
        gameObject.SetActive(false);

    }

    public override void SetInvert(bool b){

        _invert = b;
        UpdateFilter();

    }

    public override void UpdateFilter(){
       
        _main.SetCamToMainTex( _fullBgMaterial );
        _fullBgMaterial.SetTexture("_MainTex2",_texMaker._tex);//背景用
        _fullBgMaterial.SetVector("_Motion",_camMotion._distance);

    }




}