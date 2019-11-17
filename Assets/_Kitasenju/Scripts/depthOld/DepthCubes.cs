using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class DepthCubes : DepthCubesBase {

//depthを適当に渡すi,j
//texも適当に。

    private DepthCubeData[] _data;
    [SerializeField] private Texture2D _depthTex;
    [SerializeField] private Texture2D _colorTex;
    
    [SerializeField] EffectControlMain _main;
    [SerializeField] private Camera _camera;

    private int _index = 0;
    float _stencilAspect;//

    void Start(){

        _propertyBlock = new MaterialPropertyBlock();
        _matrices = new Matrix4x4[_count];
        _data = new DepthCubeData[_count];
        _colors = new Vector4[_count];

        for (int i = 0; i < _count; i++)
        {
            
            _matrices[i] = Matrix4x4.identity;
            _data[i] = new DepthCubeData();//
            _data[i].rot = Quaternion.Euler(0,0,0);
            _data[i].scale = new Vector3(0.03f,0.03f,0.03f);

            _colors[i] = new Vector4(
                Random.value,
                Random.value,
                Random.value,
                1f
            );

        }

        


    }


    Vector2 GetStencilUV( Vector2 uv ){//, float oy=0.002){

        Vector2 stencilUV = new Vector2(
            1-uv.y,
            1-uv.x
        );

        float bai = 1 / _stencilAspect;//1.62;
        //float bai = 1/1.333333;

        float offsetY = (1-bai)/2;//+oy;
        stencilUV.y = stencilUV.y*bai + offsetY;

        return stencilUV;

    }
    private void OnGUI(){
            GUI.DrawTexture(
                new Rect(0, 0, 300, 200), 
                _depthTex, 
                ScaleMode.StretchToFill,
                false
            );      

            if(_main){
                GUI.DrawTexture(
                    new Rect(0, 200, 300, 200), 
                    _main._camTex, 
                    ScaleMode.StretchToFill,
                    false
                );        
            }            

            if(_depthTex!=null){
                
                var w = Mathf.FloorToInt( _depthTex.width * 0.5f );
                var h = Mathf.FloorToInt( _depthTex.height * 0.5f );

                GUI.Label(new Rect(0, 400, 200, 200), 
                    _depthTex.GetPixel(w,h).ToString() 
                );

            }
    }

    void UpdatePixel(){

        for(int i=0;i<10;i++){

            float ratioX = Random.value;
            float ratioY = Random.value;

            var uv = new Vector2(ratioX,ratioY);//GetStencilUV(new Vector2(ratioX,ratioY));

            float xx = ( uv.x * (float)_depthTex.width );
            float yy = ( uv.y * (float)_depthTex.height );

            var depth = _depthTex.GetPixel( 
                Mathf.FloorToInt(xx),
                Mathf.FloorToInt(yy)
            );
            var col = _depthTex.GetPixel( 
                Mathf.FloorToInt(xx),
                Mathf.FloorToInt(yy)
            );

            if( depth.r != 0 ){

                //Debug.Log( Camera.main );
                if(_camera==null) _camera = Camera.main;

                var p = _camera.ScreenToWorldPoint(
                    new Vector3(
                        uv.x * (float)Screen.width,
                        uv.y * (float)Screen.height,
                        1f//depth.r
                    )
                );
                _data[_index % _data.Length].pos = p;
                _colors[_index % _colors.Length] = col;

                _index++;

            }

        }

    }


    void Update(){
        
        if(_main!=null){

            Texture2D humanStencil  = _main._humanBodyManager.humanStencilTexture;
            Texture2D humanDepth    = _main._humanBodyManager.humanDepthTexture;

            
            _depthTex = humanStencil;           
            _mat.SetTexture("_MainTex", _main._camTex);
            
            _stencilAspect 
                = ((float)Screen.height/(float)Screen.width) / (_depthTex.width/_depthTex.height);//なんか逆

            UpdatePixel();
        }

        if(_camera!=null){

            transform.position = _camera.transform.position + _camera.transform.forward;

        }

        

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