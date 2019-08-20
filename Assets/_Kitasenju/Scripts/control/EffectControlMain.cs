using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


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



    // Start is called before the first frame update
    void Start()
    {
        _filters = new List<FilterBase>();
        foreach (Transform child in transform)
        {
            
            _filters.Add( child.GetComponent<FilterBase>() );

        }
        _filterMenu.Init( _filters );
        _next();
    }

    public void SetFilter(int idx){
        
        for(int i=0;i<_filters.Count;i++){
            _filters[i].Hide();
        }
        _index = idx;
        _currentFilter = _filters[_index % _filters.Count];
        _currentFilter.Show( this );
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Texture2D humanStencil  = _humanBodyManager.humanStencilTexture;
        //Texture2D humanDepth    = _humanBodyManager.humanDepthTexture;
        if(Input.GetKeyDown(KeyCode.Space)){
            _next();
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

    void _next(){
        
        for(int i=0;i<_filters.Count;i++){
            _filters[i].Hide();
        }
        
        _currentFilter = _filters[_index % _filters.Count];
        _currentFilter.Show( this );
        _index++;

    }

    public void SetImageEffect(Material mat){
        
        _imageEffect.material = mat;

    }


}
