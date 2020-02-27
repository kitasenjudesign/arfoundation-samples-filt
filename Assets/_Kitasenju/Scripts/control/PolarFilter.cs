using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PolarFilter : SimpleFilter
{

    [SerializeField] private Shader _texMaker;//テクスチャ生成用
    private Material _texMakeMat;//テクスチャ生成用
    private RenderTexture _inputTex;
    private RenderTexture _outputTex;
    [SerializeField,Space(10)] ComputeShader _computeShader1;
    [SerializeField] ComputeShader _computeShader2;
    private bool flag = false;
    private ComputeShader _computeShader;


    public override void Show(EffectControlMain main){
        
        _computeShader = _computeShader1;

        base.Show(main);
        _main.SetImageEffect(_material);
        UpdateFilter();
        _main.ShowInfo();
        Invoke("_onHideInfo",3f);
    }

    private void _onHideInfo(){
        _main.HideInfo();        
    }
    

    public override void UpdateFilter(){

        if(_texMakeMat==null){

            var ss = 0.5f;
            var ww = Mathf.FloorToInt( Screen.width * ss );
            var hh = Mathf.FloorToInt( Screen.height * ss );

            _inputTex = new RenderTexture(ww,hh,0);
            
            _outputTex = new RenderTexture(ww,hh,0);
            _outputTex.enableRandomWrite = true;
            _outputTex.antiAliasing=2;
            _outputTex.Create();

            _texMakeMat = new Material(_texMaker);

        }

        //istouch

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if( touch.position.y > Screen.height*0.333f){

                if (touch.phase == TouchPhase.Began)// || touch.phase==TouchPhase.Stationary)
                {
                    _change();
                }
            }
        }




        //Debug.Log("----1");
        //Debug.Log(_invert);
        Texture2D humanStencil  = _main._humanBodyManager.humanStencilTexture;
        Texture2D humanDepth    = _main._humanBodyManager.humanDepthTexture;


        var mainTex = _main.UpdateCamTex();
        _texMakeMat.SetFloat("_Invert",_invert?1f:0);
        _texMakeMat.SetTexture("_MainTex"　,mainTex );//メイン画像
        _texMakeMat.SetTexture("_StencilTex", humanStencil);//マスク画像
        Graphics.Blit( null,_inputTex,_texMakeMat);
        

        //compute shader
        _computeShader.SetFloat("isInvert",_invert?1f:0 );
        _computeShader.SetFloat("w", _outputTex.width);
        _computeShader.SetFloat("h", _outputTex.height);        
        _computeShader.SetTexture(0, "tex", _outputTex);//出力用
        _computeShader.SetTexture(0,"baseTex",_inputTex);//入力、黒い色を無視し同心円状に。
        //shader.SetTexture(0,"maskTex",_maskTex);
        //Debug.Log("----4");
        _material.SetTexture("_MainTex",_outputTex);

        _computeShader.Dispatch(0, _outputTex.width/8, _outputTex.height/8, 1);


    }

    private void _change(){

        flag = !flag;
        
        if(_computeShader2!=null){
            
            if(flag){
                _computeShader = _computeShader2;
            }else{
                _computeShader = _computeShader1;
            }

        }
        
    }


}