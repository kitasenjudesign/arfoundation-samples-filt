using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjDrawMeshes : DrawMeshInstancedBase {

    //[SerializeField] protected Shader _shader;
    protected ProjDrawData[] _data;
    public const int MAX = 1023;

    protected Matrix4x4[] _modelMats2;

    protected RenderTexture _renderTexture;//これが２個あればいいのか
    
    protected Vector3[] _positions;

    void Awake(){
        
    }

    public override void Init(
        Matrix4x4 projMat, Matrix4x4 viewMat, float baseScale, Vector3[] positions=null
    ){

        if(!_isInit){

            _propertyBlock = new MaterialPropertyBlock();        
            _modelMats = new Matrix4x4[MAX];
            _modelMats2 = new Matrix4x4[MAX];
            _viewMats = new Matrix4x4[MAX];
            _projMats = new Matrix4x4[MAX];
            _data = new ProjDrawData[MAX];
                  
        }

        _positions = positions;

        _isInit=true;
        _count = Mathf.FloorToInt( 100f + 100f * Random.value );//Mathf.FloorToInt( 60+30*Random.value );
        

        int meshIndex = Mathf.FloorToInt(Random.value*_meshes.Length);
        _mesh = _meshes[meshIndex];

        var scl = Random.value <0.75f ? 1f : 4f;//_scales[meshIndex];
        var isRot = Random.value < 0.3f ? true : false;
        var isRotAnim = Random.value < 0.5f ? true : false;

        //_count = 300;
        for(int i=0;i<_count;i++){

            _modelMats[i] = Matrix4x4.identity;
            _modelMats2[i] = Matrix4x4.identity;
            _viewMats[i] = viewMat;
            _projMats[i] = projMat;

            _data[i] = new ProjDrawData();
            _data[i].Init(isRotAnim);

            _data[i].pos.x = 1.4f * (Random.value - 0.5f);
            _data[i].pos.y = 1.4f * (Random.value - 0.5f);
            _data[i].pos.z = - 0.5f - 1.5f * Random.value;
            _data[i].basePos = new Vector3(
                _data[i].pos.x,
                _data[i].pos.y,
                _data[i].pos.z
            );

            var rr = isRot ? 1f : 0;
            _data[i].rot = Quaternion.Euler(
                (Random.value-0.5f)*360f * rr,
                (Random.value-0.5f)*360f * rr,
                (Random.value-0.5f)*360f * rr
            );         
                

            //var ss = scl * (0.06f + baseScale*(0.5f+0.5f*Random.value) );
            var ss = scl * (0.06f + baseScale*(1f) );
            
            _data[i].scale = new Vector3(
                ss,ss,ss
            );


        }
        
    }

    protected virtual void Update(){

        if(_modelMats==null) return;
        if(!_isInit) return;
        //Debug.Log(_modelMats.Length);

        float scaleTotal = 0;
        for (int i = 0; i < _count; i++)
        {
            //TRS
            _data[i].Update();
            _modelMats[i].SetTRS( 
                _data[i].basePos,
                Quaternion.identity,
                //_data[i].rot,
                _data[i].scale
            );
            _modelMats2[i].SetTRS( 
                _data[i].pos,
                _data[i].rot,
                _data[i].scale
            );            
            _modelMats[i] = transform.localToWorldMatrix * _modelMats[i];
            _modelMats2[i] = transform.localToWorldMatrix * _modelMats2[i];

            
            scaleTotal += _data[i].scale.x;

        }

        if(scaleTotal==0) return;

		//_mat.SetTexture("_MainTex", _renderTexture);
        _propertyBlock.SetTexture("_MainTex", _renderTexture);

		_propertyBlock.SetMatrixArray("_ModelMat", _modelMats);
		_propertyBlock.SetMatrixArray("_ProjMat", 	_projMats);//_projectionCam.projectionMatrix );
		_propertyBlock.SetMatrixArray("_ViewMat", 	_viewMats);
        //_propertyBlock.SetMatrixArray("_ModelMat", _modelMats);

        Graphics.DrawMeshInstanced(
                _mesh, 
                0, 
                _mat, //material
                _modelMats2, //matrix
                _count, 
                _propertyBlock, 
                ShadowCastingMode.Off, 
                false,
                gameObject.layer
        );

    }

    /**
        RenderTextureにsrcTexを書き込む
     */
    public override void Capture(RenderTexture srcTex){

        if(_renderTexture == null){

            Debug.Log(DataManager.Instance);
            Debug.Log(DataManager.Instance.texStorage);

            _renderTexture = DataManager.Instance.texStorage.GetNextColorTexture(); 
            
            //new RenderTexture(
            //    Mathf.FloorToInt( Screen.width * 0.5f ),
            //    Mathf.FloorToInt( Screen.height * 0.5f ),
            //    0
            //);               
        }

        //if( Random.value < 0.5f ){
    		Graphics.Blit( srcTex, _renderTexture );
        //}else{
        //    _renderTexture = srcTex;            
        //}
    }    

    public override void Hide(){
        
        if(_data==null)return;
        for (int i = 0; i < _data.Length; i++)
        {
            if( _data[i] != null ){
                _data[i].scaleSpeed =  new Vector3(
                    -_data[i].scale.x/100f,
                    -_data[i].scale.y/100f,
                    -_data[i].scale.z/100f
                );
            }
        }

    }

    public override void Kill(){

        //if(_renderTexture!=null){
        //    _renderTexture.Release();
        //}
        //DestroyImmediate(_mat);

    }


}