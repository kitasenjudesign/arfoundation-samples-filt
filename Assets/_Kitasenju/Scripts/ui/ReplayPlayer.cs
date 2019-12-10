using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_IOS
using NatShareU;
#endif
using UnityEngine.XR.ARFoundation;
using UnityEngine.Video;
using DG.Tweening;
using TMPro;
using NatCorder.Examples;
using VoxelBusters.InstagramKit;


public class ReplayPlayer : MonoBehaviour
{
    [SerializeField] private MyReplayCam _replayCam;
    [SerializeField] private MyRecordBtn _recordBtn;
    [SerializeField,Space(10)] private Button _saveBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _instaBtn;

    [SerializeField] private ReplayVideoScreen _videoScreen;

    [SerializeField,Space(10)] private ARSession _arSession;
    [SerializeField] public VideoPlayer _videoPlayer;
    [SerializeField] public TMP_Text _savedTxt;
    [SerializeField] private ImageCapture _imageCapture;
    [SerializeField] private AudioSource _videoAudio;    
    private string _path = "";
    private bool _isMoving = false;
    private bool _isImageMode = false;
    private Texture2D _capturedImage;

    // Start is called before the first frame update
    void Awake()
    {

        
        _saveBtn.onClick.AddListener( _onClickSave );
        _closeBtn.onClick.AddListener( _onClickClose );
        _instaBtn.onClick.AddListener( _onClickInsta );
        gameObject.SetActive(false);
        _savedTxt.gameObject.SetActive(false);
        //_recordBtn._staticImageCaptureCallback = ShowImage;



    }

    private void _onClickInsta(){
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);
        StoryContent content = new StoryContent (_path, true);
        InstagramKitManager.Share(content, null);
    }

    public void ShowReplay(string path){

        //多き目の振動
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe02);

        //replayを開始する
        _path = path;
        gameObject.SetActive(true);
        _isImageMode=false;
        
        //animation
        _videoScreen.Show();

        //arを切る
        if(_arSession!=null){
            _arSession.enabled=false;
        }

        var enumerator =  startPreview(path);
        while (enumerator.MoveNext())
        {
            //object v = e.Current;
            //System.WriteLine(v);
        }

        _videoScreen.HideCover();
        Debug.Log("finish all");
    }


    public void ShowImage(){
        //画像用に作ったが今は使わない
        _isImageMode=true;
        _imageCapture.Capture(_ShowImage2);
        
    }

    private void _ShowImage2(Texture2D tex){
        _capturedImage=tex;
        gameObject.SetActive(true);
        //_rawImage.gameObject.SetActive(true);
        //_rawImage.texture=tex;
        //animation
        //_rectTrans.localPosition = new Vector3(0,-Screen.height,0);
        //_rectTrans.DOLocalMoveY(0,0.5f);

    }

    private void _saveVideo(){
        
        //保存する
        _savedTxt.gameObject.SetActive(true);
        
        #if UNITY_IOS
            if(_isImageMode){
                NatShare.SaveToCameraRoll(_capturedImage);
            }else{
                NatShare.SaveToCameraRoll(_path);
                
            }
        #endif

        //_hideVideo();
        _hideBtn();
        Invoke("_hideVideo",0.4f);
    }

    private void _onClickSave(){

        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);
        _saveVideo();

    }

    private void _onClickClose(){

        _hideBtn();
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);
        Invoke("_hideVideo",0.2f);
        //_hideVideo();
    }


    private void _hideBtn(){

        //暗くする
            //_rawImage.DOColor(new Color(0f,0f,0f,1f),0.5f)
                //.SetDelay(0.1f)
                //.SetEase(Ease.Linear);

        var saveBtnRect = _saveBtn.GetComponent<RectTransform>();
        var closeBtnRect = _closeBtn.GetComponent<RectTransform>();
        var instaBtnRect = _instaBtn.GetComponent<RectTransform>();
        saveBtnRect.DOScale(Vector3.zero,0.5f);
        closeBtnRect.DOScale(Vector3.zero,0.5f);
        instaBtnRect.DOScale(Vector3.zero,0.5f);
        
    }

    private void _hideVideo(){

        //_videoPlayer.prepareCompleted -= _onPrepare;
        _savedTxt.gameObject.SetActive(false);
        _videoPlayer.Pause();

        if(_isMoving)return;
        _isMoving=true;

        if(_arSession){
            _arSession.enabled=true;
        }

        _videoScreen.Hide(_onHideVideo);
       
    }

    void _onHideVideo(){
        _recordBtn.SetBtnActive(true);
        _isMoving=false;
        gameObject.SetActive(false);
    }

    //height 直す。


    public IEnumerator startPreview (string path){
        Debug.Log("Playing Preview: " + path);

        _path = path;

        _videoPlayer.source = VideoSource.Url;
        _videoPlayer.url = path;

        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _videoPlayer.EnableAudioTrack(0, true);
        _videoPlayer.SetTargetAudioSource(0, _videoAudio);
        _videoPlayer.Prepare ();
        
        //Wait until Movie is prepared
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!_videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Movie");
            yield return waitTime;
            break;
        }

        Debug.Log("Done Preparing Movie");

        //Assign the Texture from Movie to RawImage to be displayed

        var saveBtnRect = _saveBtn.GetComponent<RectTransform>();
        var closeBtnRect = _closeBtn.GetComponent<RectTransform>();
        var instaBtnRect = _instaBtn.GetComponent<RectTransform>();
        saveBtnRect.localScale=Vector3.zero;
        closeBtnRect.localScale=Vector3.zero;
        instaBtnRect.localScale=Vector3.zero;
        saveBtnRect.DOScale(Vector3.one,0.5f).SetDelay(0.3f);
        closeBtnRect.DOScale(Vector3.one,0.5f).SetDelay(0.4f);
        instaBtnRect.DOScale(Vector3.one,0.5f).SetDelay(0.4f);

        _videoPlayer.enabled=true;
        _videoPlayer.Play();
        //audioSource.Play();

        //yield return new WaitForEndOfFrame();
    }

    void _onPrepare(VideoPlayer v){

    }

    // Update is called once per frame
    void Update()
    {
        
        if (_videoPlayer.isPrepared) {
            _videoScreen.UpdateScreen( _videoPlayer.texture );
            //_rawImage.texture = _videoPlayer.texture;
        }

    }
}
