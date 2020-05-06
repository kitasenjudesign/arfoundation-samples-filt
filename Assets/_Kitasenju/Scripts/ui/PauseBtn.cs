using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.XR.ARFoundation;


public class PauseBtn : MonoBehaviour
{
    [SerializeField] private EffectControlMain _main;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _image1;
    [SerializeField] private Sprite _image2;
    [SerializeField] private GameObject _pauseText;

    
    [Space(10)]

    [SerializeField] private ARSession _arsession;
    [SerializeField] private FirstFade _fader;
    private float _pastTime = 0;

    private bool _Invert = false;
    // Start is called before the first frame update
    void Start()
    {
        _pauseText.SetActive(false);
        _button.onClick.AddListener(_onChange);
    }

    void _onChange(){
        
        if(Time.realtimeSinceStartup - _pastTime<0.5f){
            return;
        }
        _pastTime=Time.realtimeSinceStartup;

        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);

        _Invert = !_Invert;
        //_main.SetInvert(_Invert);
        _arsession.enabled=!_arsession.enabled;
        
        if(_arsession.enabled) _fader.FadeIn(0.3f);
        //var rect = _image.GetComponent<RectTransform>();
        //rect.DOLocalRotate( new Vector3(0,0,_Invert?180f:0), 0.5f );

    }

    public void SetActive(bool b){

        
        if(b){
            _button.enabled=true;
            _image.raycastTarget = true;
            _image.color = Color.white;
        }else{
            _button.enabled=false;
            _image.raycastTarget = false;
            _image.color = new Color(1f,1f,1f,0.4f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        _image.sprite = _arsession.enabled ? _image1 : _image2; 
        _pauseText.SetActive( !_arsession.enabled );
        
    }
}
