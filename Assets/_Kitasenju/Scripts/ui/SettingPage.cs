using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingPage : MonoBehaviour
{

    [SerializeField] private SettingToggleBtn srcBtn;

    [SerializeField] private List<string> _list;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private CanvasGroup _canvasGroup;

    private bool _isInit = false;
    private List<SettingToggleBtn> _btns;

    void Start(){
        
        Init();

    }

    public void Show(){

        _canvasGroup.alpha=0;
        _canvasGroup.DOFade(1f,0.5f).SetDelay(0.1f);

    }

    public void Init(){

        if(_isInit)return;
        _isInit=true;

        _btns = new List<SettingToggleBtn>();

        var callbacks = new System.Action<SettingToggleBtn>[]{
            _onClickMic,
            _onClickVideoQualty,
            _onClickResoluation
        };

        for(int i=0;i<_list.Count;i++){

            var n = Instantiate(srcBtn,transform,false);
            _btns.Add(n);

            var pos = n.GetComponent<RectTransform>().localPosition;
            pos.x = 0;
            pos.y = ( (float)i - ((float)_list.Count-1f)/2f ) * -300f;// + 300f;
            n.GetComponent<RectTransform>().localPosition = pos;

            var aa = _list[i].Split(',');
            
            n.Init( aa, callbacks[i] );

        }

       srcBtn.gameObject.SetActive(false);

        _settingBtn.onClick.AddListener(_goSetting);

    }

    void _goSetting(){

        #if UNITY_IPHONE
            string url = MyNativeBindings.GetSettingsURL();
            Debug.Log("the settings url is:" + url);
            Application.OpenURL(url);
        #endif

    }

    void _onClickMic(SettingToggleBtn btn){
        
        Debug.Log("_onClickMic");
        Params.usingMicrophone = btn.selected==0 ? true : false;
        
        if(Params.usingMicrophone){
            var m = Params.SetMic();

            /*
            if(m==null){
                btn.selected=0;
                btn.UpdateText();
                Params.usingMicrophone=false;
            }*/

        }

    }

    void _onClickVideoQualty(SettingToggleBtn btn){

        Debug.Log("_onClickVideoQualty");
        Params.videoQuality = btn.selected;

    }

    void _onClickResoluation(SettingToggleBtn btn){
    
        Debug.Log("onClick appsolu");
        Params.videoQuality = btn.selected;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
