using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Balls : MonoBehaviour {

    [SerializeField] protected Mesh _mesh;
    //[SerializeField] protected Shader _shader;
    [SerializeField] protected Material _mat;
    [SerializeField] protected Camera _camera;
    protected Matrix4x4[] _matrices;
    protected Vector4[] _colors;
    protected MaterialPropertyBlock _propertyBlock;
    protected int _count = 400;


    private BallData[] _data;
    public const int MAX = 1023;
    private bool _isInit = false;

    void Start(){
        
        if(_isInit)return;
        _isInit=true;

        _propertyBlock = new MaterialPropertyBlock();
        _matrices = new Matrix4x4[MAX];
        _data = new BallData[MAX];
        _colors = new Vector4[MAX];
        //_uvs = new Vector4[MAX];
        //_mat = new Material(_shader);
        //_mat.enableInstancing=true;
        //for (int i = 0; i < _count; i++)
        //{
        int numX = 10;
        int numY = 12;

        var pos = _camera.transform.position;

        Debug.Log("----");
        Debug.Log(pos);
        //var pos = Vector3.zero;


        _count=0;
        for(int i=0;i<numX;i++){
            for(int j=0;j<numY;j++){
                _matrices[_count] = Matrix4x4.identity;
                _data[_count] = new BallData();

                var amp = 1.5f + 10f * Random.value;
                var rad = Random.value * 2f * Mathf.PI;
                _data[_count].pos.x = pos.x + amp * Mathf.Cos(rad);
                _data[_count].pos.y = pos.y + 2f * (Random.value - 0.5f);
                _data[_count].pos.z = pos.z + amp * Mathf.Sin(rad);

                var s = 0.2f + 0.4f*Random.value;
                _data[_count].rot = Quaternion.Euler(0,0,0);
                _data[_count].scale = new Vector3(s,s,s);
                //_uvs[_count] = SpriteUV.GetUv(Mathf.FloorToInt(Random.value*6),4,4);
                _colors[_count] = new Vector4(
                    Random.value,
                    Random.value,
                    Random.value,
                    1f
                );
                _count++;
            }
        }

        //}
        
    }


    void Update(){


        //transform.position = _camera.transform.position + _camera.transform.forward;
        if(!_isInit)return;

        for (int i = 0; i < _count; i++)
        {
            _matrices[i].SetTRS( 
                _data[i].pos,
                _data[i].rot,
                _data[i].scale
            );
            //_matrices[i] = transform.localToWorldMatrix * _matrices[i];

        }

        _propertyBlock.SetVectorArray("_Color", _colors);
        //_propertyBlock.SetVectorArray("_Uv", _uvs);

        Graphics.DrawMeshInstanced(
                _mesh, 
                0, 
                _mat, 
                _matrices, 
                _count, 
                _propertyBlock, 
                ShadowCastingMode.Off, 
                false, 
                gameObject.layer
        );

    }

}