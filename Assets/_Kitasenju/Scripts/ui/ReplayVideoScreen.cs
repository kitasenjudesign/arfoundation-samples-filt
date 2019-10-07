using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ReplayVideoScreen : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private Image _cover;

    private RectTransform _rectTrans;
    // Start is called before the first frame update
    public void Start(){
        
        

        var rt = _rawImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2 (
            Mathf.CeilToInt( (float)Screen.width/(float)Screen.height*2436f ), 
            2436//Screen.height
        );

        rt = _cover.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2 (
            Mathf.CeilToInt( (float)Screen.width/(float)Screen.height*2436f ), 
            2436//Screen.height
        );

        _cover.gameObject.SetActive(true);


    }


    public void Show(){
        
        _rectTrans = GetComponent<RectTransform>();

        gameObject.SetActive(true);
        _rectTrans.localPosition = new Vector3(0,-Screen.height,0);
        _rectTrans.DOLocalMoveY(0,0.5f);

        _cover.gameObject.SetActive(true);
        _cover.color = new Color(0,0,0,1f);

    }

    public void HideCover(){

        _cover.DOColor(new Color(0,0,0,0),0.5f).SetDelay(0.3f);
        
    }

    public void Hide(System.Action callback){

        _rectTrans.DOLocalMoveY(-Screen.height,0.5f)
            .OnComplete(()=>{
                callback();
            })
            .SetEase(Ease.InSine)
            .SetDelay(0.2f);

    }


    public void UpdateScreen(Texture texture){
        _rawImage.texture = texture;
    }

}
