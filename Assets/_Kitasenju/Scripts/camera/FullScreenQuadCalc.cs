using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenQuadCalc : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private MeshFilter _quad;//対象quad
    [SerializeField] private float _z = 10;//奥行き
    [SerializeField,Space(10)] private GameObject _p0;//テスト用
    [SerializeField] private GameObject _p1;//テスト用
    [SerializeField] private GameObject _p2;//テスト用
    [SerializeField] private GameObject _p3;//テスト用
    

    void Start()
    {
        var verts = _quad.mesh.vertices;
        var b = new Bounds();
        b.size = new Vector3(1000f,1000f,1000f);
        _quad.mesh.bounds = b;
        Debug.Log( verts[0] );
        Debug.Log( verts[1] );
        Debug.Log( verts[2] );
        Debug.Log( verts[3] );
        //(-0.5, -0.5, 0.0)
        //(0.5, -0.5, 0.0)
        //(-0.5, 0.5, 0.0)
        //(0.5, 0.5, 0.0)
    }

    // Update is called once per frame
    void Update()
    {
        var camera = _camera;
        

        var zz = _z;
        Vector3 p0 = camera.ScreenToWorldPoint( new Vector3(0, 0, zz));
        Vector3 p1 = camera.ScreenToWorldPoint( new Vector3(camera.pixelWidth,0, zz));
        Vector3 p2 = camera.ScreenToWorldPoint( new Vector3(camera.pixelWidth, camera.pixelHeight, zz));
        Vector3 p3 = camera.ScreenToWorldPoint( new Vector3(0,camera.pixelHeight, zz));
        
        //test
        if(_p0) _p0.transform.position = p0;
        if(_p1) _p1.transform.position = p1;
        if(_p2) _p2.transform.position = p2;
        if(_p3) _p3.transform.position = p3;

        var verts = _quad.mesh.vertices;
        verts[0] = p0;
        verts[1] = p1;
        verts[2] = p3;
        verts[3] = p2;

        //_quad.transform.position = camera.transform.position + camera.transform.forward * _z;
        //_quad.transform.LookAt( _camera.transform.position );

        

        _quad.mesh.vertices = verts;
    }

}
