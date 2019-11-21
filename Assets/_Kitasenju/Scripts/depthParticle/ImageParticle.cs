using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class ImageParticle : MonoBehaviour{

    struct CubeData
    {
        public Vector3 position;
        public Vector3 velocity;
        public Vector4 color;
        public Vector3 basePos;
        public Vector2 uv;
        public float time;
    }

    //[SerializeField] private Mesh[] _meshes;
    [SerializeField] private Mesh _mesh;
    //[SerializeField] int _maxNum = 10000;
    [SerializeField]private int _num = 10000;
    [SerializeField,Range(0.001f,1f)] float _size;//_Size ("_Size", Range(0.04,0.1)) = 0.04

    int ThreadBlockSize = 64;//256;

    ComputeBuffer _cubeDataBuffer;
    ComputeBuffer _argsBuffer;
    private uint[] _args = new uint[5] { 0, 0, 0, 0, 0 };
    [SerializeField] ComputeShader _computeShader;
    [SerializeField] private Material _material;
    private MeshRenderer _renderer;
    private float _time = 0;
    private MaterialPropertyBlock _property;
    //private Vector4[] _positions;

    [SerializeField,Space(10)] private Texture _stencilTex;
    [SerializeField] private Texture _colorTex;
    [SerializeField] private Texture _depthTex;
    [SerializeField,Space(10)] private Camera _camera;
    [SerializeField,Range(0,1),Space(10)] private float _velocityRatio = 1;
    [SerializeField] private float _duration = 4f;

    private bool _isInit = false;

    void Start(){

        //Init();

    }

    public void Init(){

        //_num = num;
        //_num = num;
        //if(_num>_maxNum) _num = _maxNum;

        //_material = particleMat;
       // _mesh = _meshes[Mathf.FloorToInt(_meshes.Length*Random.value)];



        /////////////////////////
        if(_isInit)return;
        _isInit = true;




        if(_camera==null){
            _camera = Camera.main;
        }

        //_num = 10000;//Mathf.FloorToInt( _posMesh.vertexCount );
        _property = new MaterialPropertyBlock();
        _renderer = GetComponent<MeshRenderer>();

        //コンピュートバッファ用意
        _cubeDataBuffer = new ComputeBuffer(_num, Marshal.SizeOf(typeof(CubeData)));
        //おまじない（よくわからない）
        _argsBuffer = new ComputeBuffer(1, _args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);

        //----------初期値を設定。
        var dataArr = new CubeData[_num];
        //var vertices = _posMesh.vertices;
        //var uv = _posMesh.uv;

        for (int i = 0; i < _num; ++i){
            dataArr[i] = new CubeData();
            dataArr[i].position = new Vector3(
                0,100f,0
            );
            dataArr[i].basePos = new Vector3(
                Random.value,
                Random.value,
                Random.value
            );

            dataArr[i].velocity = new Vector3(
                0.01f * ( Random.value - 0.5f ),
                0.01f * ( Random.value - 0.5f ),
                0.01f * ( Random.value - 0.5f )
            );
            dataArr[i].time = Random.value*4f;
            dataArr[i].color = new Vector4(
                Random.value,
                Random.value,
                Random.value,
                1f
            );

            dataArr[i].uv = new Vector2(
                Random.value,
                Random.value
            );
        }

        //----------初期値をコンピュートバッファに入れる
        _cubeDataBuffer.SetData(dataArr);
        


        //GetComponent<MeshRenderer>().enabled=false;


    }


    public void SetTextures(Texture colTex, Texture stencilTex, Texture depthTex, Camera cam){
        
        _colorTex       = colTex;
        _stencilTex     = stencilTex;
        _depthTex       = depthTex;
        _camera         = cam;

    }

    void Update(){
        
        //computeShaderに値を渡す

        // ComputeShader
        _time += Time.deltaTime;
        if(_time>5f){
            _time = 0;//じかん
            //GetComponent<MeshRenderer>().enabled=false;
        }else if(_time>5f){
            //GetComponent<MeshRenderer>().enabled=true;
        }

        int kernelId = _computeShader.FindKernel("MainCS");
        //_computeShader.SetFloat("_Time", _time);

        // camera
        var cam = _camera;
        var camToWorld = cam.cameraToWorldMatrix;
		var projection = GL.GetGPUProjectionMatrix (cam.projectionMatrix, false);
		var inverseP = projection.inverse;

        _computeShader.SetFloat("_Near", cam.nearClipPlane );
        _computeShader.SetFloat("_Far", cam.farClipPlane );
        _computeShader.SetMatrix("_InvProjMat", inverseP);
		_computeShader.SetMatrix("_InvViewMat", camToWorld);

        _computeShader.SetFloat("_Duration",_duration);
        _computeShader.SetFloat("_VelocityRatio",_velocityRatio);


        if(_stencilTex!=null){
            _computeShader.SetTexture(0,"_StencilTex", _stencilTex);
            _computeShader.SetVector("_StencilTexSize", new Vector4(_stencilTex.width,_stencilTex.height));
        }
        if(_colorTex!=null){        
            _computeShader.SetTexture(0,"_ColorTex", _colorTex);
            _computeShader.SetVector("_ColorTexSize", new Vector4(_colorTex.width,_colorTex.height));
        }
        if(_depthTex!=null){
            _computeShader.SetTexture(0,"_DepthTex", _depthTex);
            _computeShader.SetVector("_DepthTexSize", new Vector4(_depthTex.width,_depthTex.height));
        }

        //_computeShader.SetVectorArray("_Positions", _positions);
        _computeShader.SetBuffer(kernelId, "_CubeDataBuffer", _cubeDataBuffer);
        _computeShader.Dispatch(kernelId, (Mathf.CeilToInt(_num / ThreadBlockSize) + 1), 1, 1);

        //おまじないパラメータ
        _args[0] = (uint)_mesh.GetIndexCount(0);
        _args[1] = (uint)_num;
        _args[2] = (uint)_mesh.GetIndexStart(0);
        _args[3] = (uint)_mesh.GetBaseVertex(0);
        _argsBuffer.SetData(_args);


        

        

        // GPU Instaicing
        _material.SetBuffer("_CubeDataBuffer", _cubeDataBuffer);//データを渡す
        //_material.SetVector("_DokabenMeshScale", this._DokabenMeshScale);
        _material.SetMatrix("_modelMatrix", transform.localToWorldMatrix );
        _material.SetFloat("_Size",_size);
        _material.SetFloat("_Duration", _duration);

        if(_colorTex!=null) _material.SetTexture("_MainTex", _colorTex);
        //_renderer.SetPropertyBlock(_property);

        //_material.SetVector("_Num",new Vector4(_numX,_numY,0,0));
        
        Graphics.DrawMeshInstancedIndirect(
            _mesh,
            0, 
            _material, 
            new Bounds(Vector3.zero, new Vector3(32f, 32f, 32f)), 
            _argsBuffer,//Indirectには必要なんか
            0,
            null,
            ShadowCastingMode.Off,
            //ShadowCastingMode.On,
            false
        );
        
        //gameObject.transform.Rotate(new Vector3(0.01f,0.005f,0));

    }

}