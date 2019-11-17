using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Button))]
public class FilterBtn : MonoBehaviour
{
    [SerializeField] private Image _dot;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _text;

    private System.Action<int> _callback;
    private Button _btn;
    private int _index = 0;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    public void Init(
        int idx,
        Sprite texture,
        System.Action<int> callback
    ){

        
        if(_btn == null ){
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener( _onClick );
        }

        _index = idx;
        _text.text = _index.ToString("D2");
        _callback = callback;

        if(texture!=null){
            _icon.sprite = texture;
        }

        _dot.gameObject.SetActive(false);
    }

    public void SetActiveDot(bool b){
        _dot.gameObject.SetActive(b);
        _text.gameObject.SetActive(b);
    }


    public void _onClick(){

        _callback( _index );

    }

}
