using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeBrush : BrushBase{

    private Mesh _mesh;
    [SerializeField] private Mesh _srcMesh;
    [SerializeField] private Vector3[] _vertices;
    [SerializeField] private Vector4[] _tangents;
    [SerializeField] private List<Vector3> _positions;
    [SerializeField] private Vector3 _pos;
    [SerializeField] private float _debugLength = 1f;
    [SerializeField] private float _width = 1f;
    private Vector3[] _normals;
    private Vector3[] _binormals;
    [SerializeField] private Camera _camera;
    private MousePosition _mouse;
    //private MouseUtil _mouse;

    private const int W = 150;//80;
    private const int H = 10;//20;
    private ProjObj _projObj;

    void Start(){

        _mesh = MeshUtil.CloneMesh(_srcMesh,"test");

        _width = _width*(0.1f + 0.9f*Random.value);

        var bounds = new Bounds();
        bounds.size = new Vector3(1000f,1000f,1000f);
        _mesh.bounds = bounds;
        //_mesh = MeshUtil.GetNewMesh(_mesh,MeshTopology.Lines);

        GetComponent<MeshFilter>().mesh = _mesh;

        _mouse = new MousePosition();
        _mouse.Init(W+5);
        //_mouse = new MouseUtil(0.1f + 0.1f * Random.value, 0.8f + 0.1f *Random.value, 3, null);

        _vertices   = _mesh.vertices;
        _binormals  = new Vector3[ _mesh.normals.Length ];

        for(int i=0;i<_vertices.Length;i++){

            int idxX = i % (W+1);
            int idxY = Mathf.FloorToInt((float)i/(float)(W+1));

            float rx = (float)idxX/(float)W;
            float ry = (float)idxY/(float)H;

            _vertices[i] = new Vector3(
                1f * Mathf.Cos( -rx * Mathf.PI * 2f + Mathf.PI*0.5f ),
                1f * Mathf.Sin( -rx * Mathf.PI * 2f + Mathf.PI*0.5f ),
                ry
            );
            
        }
        _mesh.vertices = _vertices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();

        _normals    = _mesh.normals;
        _tangents   = _mesh.tangents;
        //_tangents   = new Vector3[_mesh.tangents.Length]; 
        var tangents = _mesh.tangents;

        Debug.Log("-------");
        Debug.Log(_vertices.Length);
        Debug.Log( (W+1) * (H+1) );

        _projObj = GetComponent<ProjObj>();

        /* 
        for(int i=0;i<_normals.Length;i++){
            //_normals[i] = 
            var t = tangents[i];
            _tangents[i] = new Vector3(
                t.x,t.y,t.z
            );
            var tangent = tangents[i];
            var normal = _normals[i];

            Debug.Log(i + " _ " + _normals.Length);
            _binormals[i] = Vector3.Cross(tangent, normal);
        }*/


    }



    //計算する
    void Update(){

        //http://karaagedigital.hatenablog.jp/entry/2016/09/22/201900

        bool updating = true;
        if(_projObj && !_projObj.updatingPos){
            updating = false;
        }

        if(updating){
            _mouse.Upadate(_camera);
            _positions = _mouse._positions;
        }


        for(int i=0;i<W+1;i++){
            
            var v = _positions[i] - _positions[i+1];
            _tangents[i] = v;

        }


        //_mouse.Update();
        //_mouse.UpdateTangents();

        //_positions = _mouse.positions;
        //_pos = _mouse.position;
       
        //if(_mouse.positions.Count<22) return;


        _vertices   = _mesh.vertices;
        int idx = 0;
        for(int i=0;i<W+1;i++){

            //tangent（接線）を取得
            var tangent = _tangents[i];

            //tangentを使って、ノーマルを計算準備
            var tx = Mathf.Abs(tangent.x);
            var ty = Mathf.Abs(tangent.y);
            var tz = Mathf.Abs(tangent.z);

            //ノーマルを計算準備
            var normal = new Vector3();
            var min = float.MaxValue;
            if (tx <= min) {
                min = tx;
                normal.Set(1, 0, 0);
            }
            if (ty <= min) {
                min = ty;
                normal.Set(0, 1, 0);
            }
            if (tz <= min) {
                normal.Set(0, 0, 1);
            }            

            var vec = Vector3.Cross(tangent, normal).normalized;
            
            //最終的なノーマル計算
            normal = Vector3.Cross(tangent, vec);

            //バイノーマルは、接戦と法線のクロス
            var binormal = Vector3.Cross(tangent, normal);
            
            normal.Normalize();
            binormal.Normalize();

            var basePos = _positions[i];

            for(int j=0; j<H+1; j++){

                float rad = 1f * (float)j / (float)H * (Mathf.PI * 2f);
                float cos = Mathf.Cos(rad + Mathf.PI * 0.5f);
                float sin = Mathf.Sin(rad + Mathf.PI * 0.5f); 

                //normal と binormal 方向に。
                Vector3 v = _width * (cos * normal + sin * binormal).normalized;

                _vertices[idx] = basePos + v;
                idx++;
                //var v = (cos * N + sin * B).normalized;
                //p + radius * v

            }

            //_vertices[i] = new Vector3(
            //    1f * Mathf.Cos( -rx * Mathf.PI * 2f + Mathf.PI*0.5f ),
            //    1f * Mathf.Sin( -rx * Mathf.PI * 2f + Mathf.PI*0.5f ),
            //    ry
            //);            

            //normalとばいノーマルを表示する

            //drawしてみようまず
            /*
            Debug.DrawLine(
                _mouse.positions[i+0],
                _mouse.positions[i+1],
                Color.white
            );

            Debug.DrawLine(
                _mouse.positions[i+0],
                _mouse.positions[i+0] + normal * _debugLength ,
                Color.blue
            );

            Debug.DrawLine(
                _mouse.positions[i+0],
                _mouse.positions[i+0] + binormal * _debugLength ,
                Color.red
            );
             */

        }
        _mesh.vertices = _vertices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();


        /* 
        //奥行き
            for(int i=0;i<_vertices.Length;i++){

                int idxX = i % 21;
                int idxY = Mathf.FloorToInt((float)i/21f);


                //copy previus
                _normals[i] = _normals[i - 1];
                _binormals[i] = _binormals[i - 1];

                //var rot = _mouse.rotations[i]

                //_mouse.positions[ i ];
                var axis = Vector3.Cross(tangents[i - 1], tangents[i]);
                
                //tangentを計算して、、ノーマルを計算する


            }
        */

    }


}