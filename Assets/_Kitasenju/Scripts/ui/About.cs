using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class About : MonoBehaviour
{

    [SerializeField] private Image _mainImage;
    [SerializeField,Space(10)] private Button _aboutBtn;
    [SerializeField] private Button _webBtn;
    [SerializeField] private Button _instaBtn;
    [SerializeField] private Button _twBtn;

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




    
    //    Application.OpenURL("http://www.2121designsight.jp/program/audio_architecture/");        
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
