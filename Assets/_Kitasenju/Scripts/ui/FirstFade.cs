using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FirstFade : MonoBehaviour
{
    private Image _image;
    private Tween _tween;
    // Start is called before the first frame update
    void Start()
    {
        FadeIn(0.4f);
    }

    public void FadeIn(float delay){
        
        _tween?.Kill();

        gameObject.SetActive(true);
        _image = GetComponent<Image>();
        _image.color = new Color(0,0,0,1f);
        _tween = _image.DOFade(0,0.5f).SetDelay(delay).SetEase(Ease.Linear).OnComplete(_onHoge);
        
    }

    void _onHoge(){
        gameObject.SetActive(false);
    }

}
