using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FilterBtn : MonoBehaviour
{

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

    }

    private void _onClick(){

        _callback( _index );

    }

}
