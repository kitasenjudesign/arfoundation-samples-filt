using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ToggleBtn : MonoBehaviour
{
    [SerializeField] private EffectControlMain _main;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    private bool _Invert = false;
    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(_onChange);
    }

    void _onChange(){
        
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);

        _Invert = !_Invert;
        _main.SetInvert(_Invert);

        var rect = _image.GetComponent<RectTransform>();
        rect.DOLocalRotate( new Vector3(0,0,_Invert?180f:0), 0.5f );

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
        
    }
}
