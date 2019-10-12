using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FirstFade : MonoBehaviour
{
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _image.DOFade(0,0.5f).SetDelay(0.4f).SetEase(Ease.Linear).OnComplete(_onHoge);
    }

    void _onHoge(){
        gameObject.SetActive(false);
    }

}
