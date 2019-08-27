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

public class ReplayPlayer : MonoBehaviour
{
    [SerializeField] private RecordButton _recordBtn;
    [SerializeField] private Button _saveBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private RawImage _rawImage;

    [SerializeField,Space(10)] private ARSession _arSession;
    [SerializeField] public VideoPlayer _videoPlayer;
    [SerializeField] public TMP_Text _savedTxt;
    [SerializeField] private ImageCapture _imageCapture;
    private string _path = "";
    private RectTransform _rectTrans;
    private bool _isMoving = false;
    private bool _isImageMode = false;
    private Texture2D _capturedImage;

    // Start is called before the first frame update
    void Awake()
    {

        _rectTrans = GetComponent<RectTransform>();
        _saveBtn.onClick.AddListener( _saveVideo );
        _closeBtn.onClick.AddListener( _hideVideo );
        gameObject.SetActive(false);
        _savedTxt.gameObject.SetActive(false);
        _recordBtn._staticImageCaptureCallback = ShowImage;
    }




    public void ShowReplay(string path){
        //replayを開始する
        _path = path;
        gameObject.SetActive(true);
        _isImageMode=false;
        //animation
        _rectTrans.localPosition = new Vector3(0,-Screen.height,0);
        _rectTrans.DOLocalMoveY(0,0.5f);

        if(_arSession!=null){
            _arSession.enabled=false;
        }

        var enumerator =  startPreview(path);
        while (enumerator.MoveNext())
        {
            //object v = e.Current;
            //System.WriteLine(v);
        }
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
        _rawImage.gameObject.SetActive(true);
        _rawImage.texture=tex;
        //animation
        _rectTrans.localPosition = new Vector3(0,-Screen.height,0);
        _rectTrans.DOLocalMoveY(0,0.5f);

    }

    private void _saveVideo(){
        
        _savedTxt.gameObject.SetActive(true);
        
        #if UNITY_IOS
            if(_isImageMode){
                NatShare.SaveToCameraRoll(_capturedImage);
            }else{
                NatShare.SaveToCameraRoll(_path);
            }
        #endif

        //_hideVideo();
        Invoke("_hideVideo",0.5f);
    }

    private void _hideVideo(){
        
        _savedTxt.gameObject.SetActive(false);
        _videoPlayer.Pause();

        if(_isMoving)return;
        _isMoving=true;

        if(_arSession){
            _arSession.enabled=true;
        }

        _rectTrans.DOLocalMoveY(-Screen.height,0.5f).OnComplete(()=>{
            
            _isMoving=false;
            gameObject.SetActive(false);
            
        }).SetDelay(0.2f);

    }

    //height 直す。


    public IEnumerator startPreview (string path){
        Debug.Log("Playing Preview: " + path);

        _path = path;

        _videoPlayer.source = VideoSource.Url;
        _videoPlayer.url = path;

        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _videoPlayer.EnableAudioTrack(0, true);
        //videoPlayer.SetTargetAudioSource(0, audioSource);

        //Debug.Log("Set Audio Track to : " + audioSource);

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
        _rawImage.gameObject.SetActive(true);
        _rawImage.texture = _videoPlayer.texture;

        var saveBtnRect = _saveBtn.GetComponent<RectTransform>();
        var closeBtnRect = _closeBtn.GetComponent<RectTransform>();
        saveBtnRect.localScale=Vector3.zero;
        closeBtnRect.localScale=Vector3.zero;
        saveBtnRect.DOScale(Vector3.one,0.5f).SetDelay(0.3f);
        closeBtnRect.DOScale(Vector3.one,0.5f).SetDelay(0.4f);


        _videoPlayer.enabled=true;
        _videoPlayer.Play();
        //audioSource.Play();

        //yield return new WaitForEndOfFrame();
    }


    // Update is called once per frame
    void Update()
    {
        if (_videoPlayer.isPrepared) {
            _rawImage.texture = _videoPlayer.texture;
        }
    }
}
