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

public class ReplayPlayer : MonoBehaviour
{

    [SerializeField] private Button _saveBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private RawImage _rawImage;

    [SerializeField,Space(10)] private ARSession _arSession;
    [SerializeField] public VideoPlayer _videoPlayer;
    private string _path = "";
    private RectTransform _rectTrans;

    // Start is called before the first frame update
    void Awake()
    {

        _rectTrans = GetComponent<RectTransform>();
        _saveBtn.onClick.AddListener( _saveVideo );
        _closeBtn.onClick.AddListener( _hideVideo );
        gameObject.SetActive(false);

    }




    public void ShowReplay(string path){
        //replayを開始する
        _path = path;
        gameObject.SetActive(true);


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

    private void _saveVideo(){
        #if UNITY_IOS
        NatShare.SaveToCameraRoll(_path);
        #endif

        _hideVideo();
    }

    private void _hideVideo(){
        
        if(_arSession){
            _arSession.enabled=true;
        }
        gameObject.SetActive(false);
    }

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
        _rawImage.texture = _videoPlayer.texture;

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
