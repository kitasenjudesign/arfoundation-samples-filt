using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FilterBtn : MonoBehaviour
{
    [SerializeField] private Image _dot;
    private System.Action<int> _callback;
    private Button _btn;
    private int _index = 0;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    public void Init(int idx, System.Action<int> callback){

        
        if(_btn == null ){
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener( _onClick );
        }

        _index = idx;
        _callback = callback;
        _dot.gameObject.SetActive(false);
    }

    public void SetActiveDot(bool b){
        _dot.gameObject.SetActive(b);
    }


    private void _onClick(){

        _callback( _index );

    }

}
