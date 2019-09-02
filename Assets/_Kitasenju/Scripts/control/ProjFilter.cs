using UnityEngine;
using UnityEngine.XR.ARFoundation;
using NatCorder.Examples;

public class ProjFilter : FilterBase
{
    
    [SerializeField,Space(10)] public Material _fullBgMaterial;
    [SerializeField] private bool _invert = false;
    [SerializeField] private ProjObjs _projObjs;

    [SerializeField] private GameObject _fullQuad;
    [SerializeField] private MyReplayCam _replayCam;
    [SerializeField] private RenderTexMaker _texMaker;
    [SerializeField] private FullScreenQuadByShader _myFullScreen;
    private bool _flag = false;

    public override void Show(EffectControlMain main){

        _fullQuad.gameObject.SetActive(false);
        _texMaker.enabled=true;
        base.Show(main);
        //_main.SetImageEffect(_material);
        _projObjs.Init( _main );
        UpdateFilter();

    }

    public override void Hide(){
        
        _fullQuad.gameObject.SetActive(true);
        _texMaker.enabled=false;
        gameObject.SetActive(false);

    }

    public override void SetInvert(bool b){

        _invert = b;
        UpdateFilter();

    }

    public override void UpdateFilter(){
        
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)// || touch.phase==TouchPhase.Stationary)
            {
                if(Random.value<0.5f){
				    _flag = !_flag;
                }
			}
		}

        if(!_flag){
            _main.SetCamToMainTex( _fullBgMaterial );
        }else{
            _fullBgMaterial.SetTexture("_MainTex",_texMaker._tex);
        }

    }




}