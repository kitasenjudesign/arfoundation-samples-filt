using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class About : MonoBehaviour
{

    [SerializeField] private Image _mainImage;
    [SerializeField,Space(10)] private Button _aboutBtn;
    [SerializeField] private Button _webBtn;
    [SerializeField] private Button _instaBtn;
    [SerializeField] private Button _twBtn;
    [SerializeField,Space(10)] private TextMeshProUGUI _ver;

    ///
    [SerializeField,Space(10)] private GameObject _page1;
    [SerializeField] private SettingPage _page2;    
    [SerializeField,Space(10)] private Button _page1Btn;
    [SerializeField] private Button _page2Btn;
    [SerializeField] private Image _underline1;
    [SerializeField] private Image _underline2;


    // Start is called before the first frame update
    void Start()
    {

        gameObject.SetActive(false);

        _aboutBtn.onClick.AddListener(_onShowAbout);
        
        _webBtn.onClick.AddListener(_goWeb);
        _instaBtn.onClick.AddListener(_goInsta);
        _twBtn.onClick.AddListener(_goTw);
        
        _webBtn.GetComponent<Image>().color=new Color(0,0,0,0);
        _instaBtn.GetComponent<Image>().color=new Color(0,0,0,0);
        _twBtn.GetComponent<Image>().color=new Color(0,0,0,0);
        

        //page1,2

        _page1Btn.GetComponent<Image>().color=new Color(0,0,0,0);
        _page2Btn.GetComponent<Image>().color=new Color(0,0,0,0);
        _page1Btn.onClick.AddListener(_goPage1);
        _page2Btn.onClick.AddListener(_goPage2);


        _ver.text = "ver " + Application.version;// + Application.buildGUID;


        if( _page2 != null ) _page2.Init();


        _goPage1();
    }

    
        

    

    private void _goPage1(){
        _page1.gameObject.SetActive(true);
        _page2.gameObject.SetActive(false);
        _underline1.gameObject.SetActive(true);
        _underline2.gameObject.SetActive(false);        
    }
    private void _goPage2(){
        _page1.gameObject.SetActive(false);
        _page2.gameObject.SetActive(true);
        _underline1.gameObject.SetActive(false);
        _underline2.gameObject.SetActive(true);        

    }

    public void Show(){

    }

    public void Hide(){
        
    }

    private void _onShowAbout(){

        var active = !gameObject.activeSelf;
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);
        gameObject.SetActive( active );
        if(active){
            _mainImage.color = new Color(1,1,1,0);
            _mainImage.DOColor(Color.white,0.5f).SetDelay(0.1f);
            _page2.Show();
        }        
        
    }

    private void _goWeb(){
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);
        Application.OpenURL("https://kitasenjudesign.com/");        
    
    }
    private void _goInsta(){
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);
        Application.OpenURL("https://instagram.com/kitasenjudesign");        
    
    }
    private void _goTw(){
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);
        Application.OpenURL("https://twitter.com/kitasenjudesign");        
    
    }

    // Update is called once per frame
    //void Update()
    //{  
    //}

}
