using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DuplicationFilter : FilterBase
{
    
    [SerializeField,Space(20)] private Shader _slitRecShader;
    [SerializeField] private Shader _fillShader;
    [SerializeField] private Shader _outputShader;
    private Material _slitRecMat;
    private Material _fillMat;
    private Material _outputMat;
    private bool _isInit = false;
    private SimpleSlitScan _slitscanColor;
    private SimpleSlitScan _slitscanStencil;
    private RenderTexture _renderTex;
    private float _ratio = 0;
    private RenderTexture _inputTex;
    private RenderTexture _outputTex;//


    public override void Show(EffectControlMain main){
        
        base.Show(main);
        

    }

    private void _Init(){

        //初期化が必要な時
        //Debug.Log( _main._arBackground );
        //Debug.Log("init >> " + _main._arBackground.material);

    
        _renderTex      = new RenderTexture(512,256,0);
        _slitRecMat     = new Material( _slitRecShader );
        _fillMat        = new Material( _fillShader );
        _outputMat = new Material( _outputShader );
        //_updateMat();

        _inputTex = new RenderTexture(
            Screen.width,
            Screen.height,
            0
        );
        _outputTex = new RenderTexture(
            Screen.width,
            Screen.height,
            0
        );

        _slitscanColor = new SimpleSlitScan(
            640,
            480,
            RenderTextureFormat.ARGB32,
            _slitRecMat
        );
        _slitscanStencil = new SimpleSlitScan(
            960,
            720,
            RenderTextureFormat.R8,
            _slitRecMat
        );

    }

    public override void UpdateFilter()
    {
        
        Texture2D humanStencil  = _main._humanBodyManager.humanStencilTexture;
        //Texture2D humanDepth    = _main._humanBodyManager.humanDepthTexture;

        if(!_isInit){

            _isInit=true;
            _Init();

        }else{
            
            //カメラテックスに背景素材を書き込む
            if( _main._arBackground.material ){
                Graphics.Blit(
                    null,
                    _inputTex,
                    _main._arBackground.material
                );
            }

            //フレームを保存する、これをコピーさせておく
            _slitscanColor.UpdateFrame(_inputTex);
            _slitscanStencil.UpdateFrame(humanStencil);

            //そのうち２フレームを抜粋する
            var camIdx = new int[]{0,5,10,15,20,25,30,35,40,45,50,59};
            var stencilIdx = new int[]{0,5,10,15,20,25,30,35,40,45,50,59};

            //一番背景に、カメラを塗る
            //Graphics.Blit( _cameraTex, _renderTex );

            //手前に、数フレームぶん、マスクした人を貼り付ける
            _ratio=1f;
            var len = Mathf.FloorToInt( camIdx.Length * _ratio );
            Graphics.Blit( _inputTex, _outputTex );

            for(int i=0;i<len;i++){

                _fillMat.SetTexture("_MainColTex", _slitscanColor.GetFrame(camIdx[len-i-1]) );
                _fillMat.SetTexture("_StencilTex", _slitscanStencil.GetFrame(stencilIdx[len-i-1]) );

                Graphics.Blit( null, _outputTex, _fillMat );

            }

            _outputMat.SetTexture( "_OutputTex", _outputTex );

        }

         _main.SetImageEffect(_outputMat);

    }

}