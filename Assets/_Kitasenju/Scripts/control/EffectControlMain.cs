using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using DG.Tweening;
using TMPro;


public class EffectControlMain : MonoBehaviour
{
    [SerializeField] public ARCameraBackground _arBackground;
    [SerializeField] public AROcclusionManager _humanBodyManager;
    //[SerializeField,Space(10)] private ImageEffectBase _imageEffect;
    [SerializeField] private FullScreenQuad _fullScreenQuad;
    [SerializeField] private UnityEngine.Rendering.PostProcessing.PostProcessLayer _postEffect;

    [SerializeField,Space(10)] public List<FilterBase> _filters;
    [SerializeField,Space(10)] public Menu _menu;
    [SerializeField] private FilterMenu _filterMenu;

    private FilterBase _currentFilter;
    [SerializeField] private int _index = 0;
    public RenderTexture _camTex;
    [SerializeField] private bool _Invert = false;
    [SerializeField] private ToggleBtn _toggleBtn;
    [SerializeField] private TextMeshProUGUI _info;
    [SerializeField,Space(10)] private BlurTexture _blurTexture;
    private int _count = 0;
    
    private GUIStyle _style;
    public static EffectControlMain Instance;

    // Start is called before the first frame update
    void Start()
    {
        _camTex=new RenderTexture( 
            Screen.width, Screen.height, 16
        );
        //_camTex.enableRandomWrite = true;

        _filters = new List<FilterBase>();
        foreach (Transform child in transform)
        {
            if( child.gameObject.activeSelf ){ 
                _filters.Add( child.GetComponent<FilterBase>() );
            }
        }
        _filterMenu.Init( _filters );
        //_next();
        
        Instance = this;

        //SetFilter(0);
        
        /*
        var mat = _currentFilter.GetMaterial();
        Debug.Log(">>>>" + mat);
        if(mat){
            mat.SetFloat("_Brightness",0);
            mat.DOFloat(1f,"_Brightness",0.5f).SetEase(Ease.Linear).SetDelay(0.4f);
        }*/     
        

        
    }

    
    
    void OnGUI(){

        if(_camTex){
        GUI.DrawTexture(new Rect(0,0,200,100),_camTex);
        }

    }


    public void SetFilter(int idx){
        
        Debug.Log("SetFilter " + idx + "/" + _filters.Count);

        for(int i=0;i<_filters.Count;i++){
            _filters[i].Hide();
        }
        _index = idx;

        _currentFilter = _filters[_index % _filters.Count];
        _currentFilter.Show( this );
        _toggleBtn.SetActive( _currentFilter._hasInvert );

        _postEffect.enabled = _currentFilter._hasBloom;

        ShowInfo();

        SetInvert(_Invert);
    }

    // Update is called once per frame
    void Update()
    {


        
        Texture2D humanStencil  = _humanBodyManager.humanStencilTexture;
        Texture2D humanDepth    = _humanBodyManager.humanDepthTexture;

        if(humanStencil){

            Params.SetStencilAspect(
                humanStencil.width,humanStencil.height
            );
            
        }


        var subsystem = _humanBodyManager.subsystem;
        if (subsystem == null)
        {
            //"Human Segmentation not supported."
            return;
        }
        
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            //_next();
        }

        if(_currentFilter){
            
            _currentFilter.UpdateFilter(
                //_humanBodyManager,
                //_menu        
            );
        }

    }

    public void SetPause(bool b){

        

    }


    public void SetInvert(bool b){
        
        _Invert = b;
        _currentFilter.SetInvert(_Invert);

    }


    public void SetCamToMainTex(Material mat, bool isBlur=false){
        if(_arBackground.material){
            
            //Graphics.Blit(null,_camTex,_arBackground.material);//
            UpdateCamTex();
            if(mat) mat.SetTexture("_MainTex",_camTex);

            if(isBlur){
                if(mat) mat.SetTexture("_BlurTex",_blurTexture.UpdateBlur(_camTex));
            }

        }
    }

    public RenderTexture UpdateCamTex(){
        if(_arBackground.material) Graphics.Blit(null,_camTex,_arBackground.material);
        return _camTex;
    }

    public void SetBlurTex(Material mat){

    }


    public void SetImageEffect(Material mat){
        
        //_imageEffect.material = mat;
        _fullScreenQuad.SetMaterial(mat);
    }

    public void ShowInfo(){
        
        if( _currentFilter.info == "" ){
            _info.gameObject.SetActive( false );
        }else{
            _info.text = _currentFilter.info;
            _info.gameObject.SetActive( true );
        }

    }

    public void HideInfo(){
        _info.gameObject.SetActive( false );
    }

}
