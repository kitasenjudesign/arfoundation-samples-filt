﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ToggleBtn : MonoBehaviour
{
    [SerializeField] private EffectControlMain _main;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    private bool _Invert = false;
    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(_onChange);
    }

    void _onChange(){
        
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);

        _Invert = !_Invert;
        _main.SetInvert(_Invert);

        var rect = _image.GetComponent<RectTransform>();
        rect.DOLocalRotate( new Vector3(0,0,_Invert?180f:0), 0.5f );

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
