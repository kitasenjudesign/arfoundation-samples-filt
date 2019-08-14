using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    [SerializeField] private SlitScan2 _src;
    [SerializeField] private Material _outputMat;
    private List<SlitScan2> _list;
    [SerializeField] private List<Rotater> _rotaters;
    [SerializeField] private int _numCamera = 15;
    private float _count = 0;
    private bool _isDown = false;
    // Start is called before the first frame update
    void Start()
    {
        _list = new List<SlitScan2>();
        int num = _numCamera;
        for(int i=0;i<num;i++){
            var amp = 5f;
            var c = Instantiate(_src,transform,false);
            c.transform.localPosition = new Vector3(
                amp * Mathf.Cos( (float)i / (float)num * 2f * Mathf.PI ),
                0,
                amp * Mathf.Sin( (float)i / (float)num * 2f * Mathf.PI )
            );
            c.transform.LookAt(Vector3.zero);
            _list.Add(c);
        }
        _src.gameObject.SetActive(false);
    }   

    void OnGUI(){

    }


    // Update is called once per frame
    void Update()
    {
        
        _outputMat.SetTexture("_MainTex",_list[Mathf.FloorToInt(_count)%_list.Count]._renderTex);
        //_outputMat.SetTexture("_MainTex",_list[0]._renderTex);
        
        //_list[_count%_list.Count]._renderTex);
        //_count = (_list.Count-1) * ( 0.5f + 0.5f * Mathf.Sin(Time.realtimeSinceStartup*10f) );
        _count+=0.2f;
         
        //if( Time.realtimeSinceStartup%4f<2f ){

        if( Input.GetKeyDown(KeyCode.Space) ){
            _isDown = !_isDown;
        }

        if( _isDown ){
            //_count+=0.4f;
            for(int i=0;i<_rotaters.Count;i++){
                _rotaters[i].spdScale=0;
            }
            for(int i=0;i<_list.Count;i++){
                _list[i].isActive=false;
            }
        }else{
            for(int i=0;i<_rotaters.Count;i++){
                var scl = 5f;//Random.value < 0.3f ? 5.1f : 1.2f;
                _rotaters[i].spdScale=1f;
            }   
            for(int i=0;i<_list.Count;i++){
                _list[i].isActive=true;
            }


        }

    }
}
