using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LEDSwitcher : MonoBehaviour
{
    [SerializeField] private FlashlightController _controller;
    [SerializeField] private Button _btn;
    private bool _isLight = false;
    //[SerializeField] private

    // Start is called before the first frame update
    void Start()
    {
        _btn.onClick.AddListener( _onClick );
    }

    private void _onClick(){
        
        _isLight = !_isLight;

        _controller.ChangeBrightness( 1f );

        if(_isLight) _controller.TurnOn();
        else _controller.TurnOff();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
