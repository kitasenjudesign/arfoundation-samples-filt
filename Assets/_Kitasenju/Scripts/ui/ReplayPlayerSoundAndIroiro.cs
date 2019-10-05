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

public class ReplayPlayerSoundAndIroiro : MonoBehaviour
{
    [SerializeField] private MyReplayCam _replayCam;
    [SerializeField] private MyRecordBtn _recordBtn;
    [SerializeField] private Button _saveBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private RawImage _rawImage;

    [SerializeField,Space(10)] private ARSession _arSession;
    [SerializeField] public VideoPlayer _videoPlayer;
    [SerializeField] public TMP_Text _savedTxt;
    [SerializeField] private ImageCapture _imageCapture;
    [SerializeField] private AudioSource _videoAudio;
    [SerializeField] private Texture2D _blackTex;
    private string _path = "";
    private RectTransform _rectTrans;
    private bool _isMoving = false;
    private bool _isImageMode = false;
    private Texture2D _capturedImage;
    private bool _isPlaying = false;
    private bool _showing = false;

    // Start is called before the first frame update
    void Awake()
    {

        _rectTrans = GetComponent<RectTransform>();
        _saveBtn.onClick.AddListener( _onClickSave );
        _closeBtn.onClick.AddListener( _onClickClose );
        gameObject.SetActive(false);
        _savedTxt.gameObject.SetActive(false);
        //_recordBtn._staticImageCaptureCallback = ShowImage;

        var rt = _rawImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2 (
            Mathf.CeilToInt( (float)Screen.width/(float)Screen.height*2436f ), 
            2436//Screen.height
        );

    }




    public void ShowReplay(string path){

        Debug.Log("ShowReplay " + Random.value);

        _showing = false;
        _isPlaying=false;

        //多き目の振動
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe02);//3);

        //replayを開始する
        _path = path;
        gameObject.SetActive(true);
        _isImageMode=false;
        
        //animation、動かす
        _rectTrans.localPosition = new Vector3(0,-Screen.height,0);
        _rectTrans.DOLocalMoveY(0,0.5f).SetEase(Ease.OutSine);

        //arを切る
        if(_arSession!=null){
            _arSession.enabled=false;
        }

        //ボタン消す
        _saveBtn.gameObject.SetActive(false);
        _closeBtn.gameObject.SetActive(false);
        
        //黒くする
        _rawImage.gameObject.SetActive(true);
        _rawImage.color = new Color(0,0,0,1f);//色
        _rawImage.texture = _blackTex;        

        //動画の再生
        _videoPlayer.source = VideoSource.Url;
        _videoPlayer.url = path;
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _videoPlayer.EnableAudioTrack(0, true);
        _videoPlayer.SetTargetAudioSource(0, _videoAudio);
        _videoAudio.volume = 1f;
        
        _videoPlayer.prepareCompleted += _OnPrepareCompleted;
		_videoPlayer.Prepare ();
        /*
        var enumerator =  startPreview(path);
        while (enumerator.MoveNext())
        {
            //object v = e.Current;
            //System.WriteLine(v);
        }*/



        Debug.Log("finish all");
    }

    private void _OnPrepareCompleted(VideoPlayer v){
        Debug.Log("_OnPrepareCompleted");
        _rawImage.color = new Color(0,0,0,1f);//色
        _videoPlayer.prepareCompleted -= _OnPrepareCompleted;
        DOVirtual.DelayedCall(0.5f,_OnPrepare2);

    }

    private void _OnPrepare2(){

        Debug.Log("_OnPrepare2");

        _saveBtn.gameObject.SetActive(true);
        _closeBtn.gameObject.SetActive(true);

        //ボタン表示
        var saveBtnRect = _saveBtn.GetComponent<RectTransform>();
        var closeBtnRect = _closeBtn.GetComponent<RectTransform>();
        saveBtnRect.localScale=Vector3.zero;
        closeBtnRect.localScale=Vector3.zero;        
        saveBtnRect.DOScale(Vector3.one,0.5f).SetDelay(0.3f);
        closeBtnRect.DOScale(Vector3.one,0.5f).SetDelay(0.4f);

        _videoPlayer.started += _OnStart;
        _videoPlayer.Play();
        

    }

    private void _OnStart(VideoPlayer v){
        
        _videoPlayer.started -= _OnStart;
        _isPlaying=true;

    }

    public void ShowImage(){
        //画像用に作ったが今は使わない
        _isImageMode=true;
        _imageCapture.Capture(_ShowImage2);
        
    }

    private void _ShowImage2(Texture2D tex){
        _capturedImage=tex;
        gameObject.SetActive(true);
        _rawImage.gameObject.SetActive(true);
        _rawImage.texture=tex;
        //animation
        _rectTrans.localPosition = new Vector3(0,-Screen.height,0);
        _rectTrans.DOLocalMoveY(0,0.5f);
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
        saveBtnRect.DOScale(Vector3.zero,0.5f);
        closeBtnRect.DOScale(Vector3.zero,0.5f);

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

        _rectTrans.DOLocalMoveY(-Screen.height,0.5f).OnComplete(()=>{

            _recordBtn.SetBtnActive(true);
            _isMoving=false;
            _videoPlayer.Stop();
            gameObject.SetActive(false);
            
        }).SetEase(Ease.InSine).SetDelay(0.2f);

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

        _videoAudio.volume = 1f;
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
        
        //_rawImage.texture = _videoPlayer.texture;

        //_videoPlayer.enabled=true;
        //_videoPlayer.Play();
        //audioSource.Play();

        //yield return new WaitForEndOfFrame();
    }


    // Update is called once per frame
    void Update()
    {
        if(_isPlaying){
            
            _rawImage.texture = _videoPlayer.texture;

            if(!_showing){

                _rawImage.color = new Color(0,0,0,1f);//色
                _rawImage.DOColor(new Color(1f,1f,1f,1f),0.5f)
                    .SetDelay(0.3f)
                    .SetEase(Ease.Linear);

                _showing=true;

            }  

        }else{

            _rawImage.texture = _blackTex;

        }

    }
}
