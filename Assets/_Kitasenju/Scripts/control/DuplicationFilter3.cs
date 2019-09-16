using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DuplicationFilter3 : FilterBase
{
    
    //[SerializeField,Space(20)] private Shader _slitRecShader;
    [SerializeField] private Shader _fillShader;
    [SerializeField] private Shader _outputShader;
    [SerializeField,Range(0,1f)] private float _ratioZanzou = 1f;
    private Material _fillMat;
    private Material _outputMat;
    private bool _isInit = false;
    private RenderTexManager _texStorage;

    public override void Show(EffectControlMain main){
        
        base.Show(main);
        _fillMat        = new Material( _fillShader );
        _outputMat = new Material( _outputShader );
        //DataManager.Instance.InitTexStorage();
        //_texStorage = DataManager.Instance.texStorage;        
        //_main._arBackground.enabled=false;
        UpdateFilter();
        
    }

    private void _Init(){

        DataManager.Instance.InitTexStorage();
        _texStorage = DataManager.Instance.texStorage;

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
                    _texStorage._inputTex,
                    _main._arBackground.material
                );
            }

            //フレームを保存する、これをコピーさせておく
            _texStorage._slitscanColor.UpdateFrame(     _texStorage._inputTex);
            _texStorage._slitscanStencil.UpdateFrame(   humanStencil);

            //そのうち２フレームを抜粋する
            var camIdx = new int[]{0,5,10,15,20,25,30,35,40,45,50,59};
            var stencilIdx = new int[]{0,5,10,15,20,25,30,35,40,45,50,59};
            //var camIdx = new int[]{0,10,20,30,40,50,59};
            //var stencilIdx = new int[]{0,10,20,30,40,50,59};

            //一番背景に、カメラを塗る
            //Graphics.Blit( _cameraTex, _renderTex );

            //手前に、数フレームぶん、マスクした人を貼り付ける
            var len = Mathf.FloorToInt( camIdx.Length * _ratioZanzou );
            Graphics.Blit( 
                _texStorage._inputTex,
                _texStorage._outputTex
            );
            Graphics.Blit( 
                _texStorage._inputTex,
                _texStorage._outputTexTmp
            );            

            var flag = false;
            for(int i=0;i<len;i++){
                
                flag = !flag;

                _fillMat.SetTexture(
                    "_MainTex", 
                    flag ? _texStorage._outputTexTmp: _texStorage._outputTex
                );
                _fillMat.SetTexture("_MainColTex", _texStorage._slitscanColor.GetFrame(camIdx[len-i-1]) );
                _fillMat.SetTexture("_StencilTex", _texStorage._slitscanStencil.GetFrame(stencilIdx[len-i-1]) );
                _fillMat.SetFloat("_Strength", 1f - (float)i/( (float)len - 1f ) );

                Graphics.Blit( 
                    null,
                    flag ? _texStorage._outputTex : _texStorage._outputTexTmp,
                    //_texStorage._outputTex,
                    _fillMat
                );

            }

            _outputMat.SetTexture( "_OutputTex", _texStorage._outputTex );

        }

         _main.SetImageEffect(_outputMat);

    }

}