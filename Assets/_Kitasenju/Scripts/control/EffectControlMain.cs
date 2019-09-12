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
    [SerializeField] public ARHumanBodyManager _humanBodyManager;
    [SerializeField,Space(10)] private ImageEffectBase _imageEffect;
    [SerializeField] private FullScreenQuad _fullScreenQuad;

    [SerializeField,Space(10)] private List<FilterBase> _filters;
    [SerializeField,Space(10)] public Menu _menu;
    [SerializeField] private FilterMenu _filterMenu;

    private FilterBase _currentFilter;
    private int _index = 0;
    public RenderTexture _camTex;
    [SerializeField] private bool _Invert = false;
    [SerializeField] private ToggleBtn _toggleBtn;
    [SerializeField] private TextMeshProUGUI _info;
    private int _count = 0;
    
    private GUIStyle _style;

    // Start is called before the first frame update
    void Start()
    {
        _camTex=new RenderTexture( Screen.width, Screen.height, 0);

        _filters = new List<FilterBase>();
        foreach (Transform child in transform)
        {
            if( child.gameObject.activeSelf ){ 
                _filters.Add( child.GetComponent<FilterBase>() );
            }
        }
        _filterMenu.Init( _filters );
        //_next();
        
        SetFilter(0);
        
        var mat = _currentFilter.GetMaterial();

        //mat.SetFloat("_Brightness",0);
        Debug.Log(">>>>" + mat);
        if(mat){
            mat.SetFloat("_Brightness",0);
            mat.DOFloat(1f,"_Brightness",0.5f).SetEase(Ease.Linear).SetDelay(0.4f);
        }        
    }

    
    /*
    void OnGUI(){

        if(_style==null){
            _style = new GUIStyle();
            _style.fontSize = 40;
            _style.normal.textColor = Color.red; 
        }

            GUI.Label(
                new Rect(100, 100, 500, 100), 
                ""+_count,
                _style
            );
        //GUI.DrawTexture(new Rect(0,0,100,100),_webcamTex);
        //GUI.DrawTexture(new Rect(0,0,200,100),_data[0].renderTexture);
    }*/


    public void SetFilter(int idx){
        
        for(int i=0;i<_filters.Count;i++){
            _filters[i].Hide();
        }
        _index = idx;

        _currentFilter = _filters[_index % _filters.Count];
        _currentFilter.Show( this );
        _toggleBtn.SetActive( _currentFilter._hasInvert );
        
        _info.gameObject.SetActive( _currentFilter.textId>=0 );
        SetInvert(_Invert);
    }

    // Update is called once per frame
    void Update()
    {


        /*
        Texture2D humanStencil  = _humanBodyManager.humanStencilTexture;
        Texture2D humanDepth    = _humanBodyManager.humanDepthTexture;

        if(humanStencil){

            var w = humanStencil.width;
            var h = humanStencil.height;
            
            var cols =humanStencil.GetPixels();

            for(int i=0;i<cols.Length;i++){
                if( cols[i].r > 0.1f ){
                    _count++;
                }
            }

        }*/


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



    public void SetInvert(bool b){
        
        _Invert = b;
        _currentFilter.SetInvert(_Invert);

    }


    public void SetCamToMainTex(Material mat){
        if(_arBackground.material){
            Graphics.Blit(null,_camTex,_arBackground.material);//
            mat.SetTexture("_MainTex",_camTex);
        }
        
    }

    public void SetImageEffect(Material mat){
        
        //_imageEffect.material = mat;
        _fullScreenQuad.SetMaterial(mat);
    }

    public void HideInfo(){
        _info.gameObject.SetActive( false );
    }

}
