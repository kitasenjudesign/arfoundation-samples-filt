using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenQuad : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private MeshFilter _quad;//対象quad
    [SerializeField] private float _z = 10;//奥行き
    [SerializeField,Space(10)] private GameObject[] _debugPoint;//テスト用
    

    void Start()
    {
        var verts = _quad.mesh.vertices;
        var b = new Bounds();
        b.size = new Vector3(1000f,1000f,1000f);
        
        _quad.mesh.bounds = b;

        //Debug.Log( verts[0] );
        //Debug.Log( verts[1] );
        //Debug.Log( verts[2] );
        //Debug.Log( verts[3] );
        
        //(-0.5, -0.5, 0.0)
        //(0.5, -0.5, 0.0)
        //(-0.5, 0.5, 0.0)
        //(0.5, 0.5, 0.0)
    }

    public void SetMaterial( Material mat ){
        
        _quad.GetComponent<MeshRenderer>().sharedMaterial = mat;

    }
    void OnPreRender(){
        LateUpdate();
    }
    void OnPostRender(){
        LateUpdate();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        var camera = _camera;
        
        var zz = _z;
        Vector3 p0 = camera.ScreenToWorldPoint( new Vector3(0, 0, zz));
        Vector3 p1 = camera.ScreenToWorldPoint( new Vector3(camera.pixelWidth,0, zz));
        Vector3 p2 = camera.ScreenToWorldPoint( new Vector3(camera.pixelWidth, camera.pixelHeight, zz));
        Vector3 p3 = camera.ScreenToWorldPoint( new Vector3(0,camera.pixelHeight, zz));
        
        //test
        if(_debugPoint.Length>0){
            if(_debugPoint[0]) _debugPoint[0].transform.position = p0;
            if(_debugPoint[1]) _debugPoint[1].transform.position = p1;
            if(_debugPoint[2]) _debugPoint[2].transform.position = p2;
            if(_debugPoint[3]) _debugPoint[3].transform.position = p3;
        }

        var verts = _quad.mesh.vertices;
        verts[0] = p0;
        verts[1] = p1;
        verts[2] = p3;
        verts[3] = p2;
        _quad.mesh.vertices = verts;
    }

}
