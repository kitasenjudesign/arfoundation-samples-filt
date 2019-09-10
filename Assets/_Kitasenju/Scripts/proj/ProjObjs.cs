using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ProjObjs : MonoBehaviour {

	//[SerializeField] private ProjDrawMeshes _src;//prefab
	[SerializeField] private ProjBase[] _src;//prefab
	private List<ProjBase> _list;
	[SerializeField] private Camera _projectionCam;
	[SerializeField] private Camera _camera;
	//private RenderTexture _captureTestTex;
	private ProjBase _current;
	private int _count = 0;
    private EffectControlMain _main;
    [SerializeField] private RenderTexMaker _camTexMaker;
	private bool _isInit = false;

    [SerializeField] protected Material _simpleMaskMat;
	private RenderTexture _renderTex;
	private GUIStyle _style;
	private float _scale=1f;

	public void Init (EffectControlMain main) {

		_main = main;
		HideAll();

		if(_isInit)return;
		_isInit=true;

		_list = new List<ProjBase>();
		_renderTex = new RenderTexture(1024,1024,0);

		for(int i=0;i<_src.Length; i++){
			_src[i].gameObject.SetActive(false);
		}
	
		//すでにあったら消す


	}

	
   private void OnGUI()
    {


        if(_style==null){
            _style = new GUIStyle();
            _style.fontSize = 40;
            _style.normal.textColor = Color.red;                
        }

        var str = ""+_scale;

        //Debug.Log(timeCode=="");
        GUI.Label(
            new Rect(50,350, 200, 100),
            str,
            _style
        );

	}
	
	public void HideAll(){
		if(_list!=null){
			for(int i=0;i<_list.Count;i++){
				_list[i].Hide();
			}
		}

	}


	// Update is called once per frame
	void Update () {
		

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)// || touch.phase==TouchPhase.Stationary)
            {
				MakeObj();
			}
		}

        if (Input.touchCount == 3)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)// || touch.phase==TouchPhase.Stationary)
            {			
            	_Remove();
			}
		}



		if(Input.GetKeyDown(KeyCode.Space)){
			MakeObj();
		}

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			_Remove();
		}

	}

	//多すぎたら消すとか

	void _Remove(){
		
		Debug.Log("_Remove!!!!");
		for(int i=0;i<_list.Count;i++){
			_list[i].Kill();
			Destroy(_list[i].gameObject);
		}
		_list = new List<ProjBase>();
	
	}

	public void MakeObj(){

		_current = Instantiate( _src[_count%_src.Length], transform, false );
        _current.gameObject.SetActive(true);
		_count++;

		Debug.Log("MakeObj " + _current);
		_list.Add(_current);

		//多すぎると削除
		if(_list.Count>6){
			var tgt = _list[0];
			tgt.Kill();
			_list.RemoveAt(0);
			Destroy(tgt.gameObject);
		}

		_current.gameObject.SetActive(false);//自分は消す
        
		//背景を透明にしたいとき
			/*
			Texture2D humanStencil  = _main._humanBodyManager.humanStencilTexture;
			_simpleMaskMat.SetTexture("_StencilTex",humanStencil);
			_simpleMaskMat.SetTexture("_MainTex",_camTexMaker._tex);
			Graphics.Blit(
				_camTexMaker._tex,_renderTex,_simpleMaskMat
			);
			_current.Capture(_renderTex );
			*/

		//通常
			_current.Capture(
				_camTexMaker._tex
			);//相手と背景をキャプチャ

		//0.05sec後に表示する
		Invoke("_SetPos",0.05f);
	}

	void _SetPos(){

		Debug.Log("_SetPos!!!!");

		var projMat = _projectionCam.projectionMatrix;
		var viewMat = _projectionCam.worldToCameraMatrix;
		
		_current.transform.position = _camera.transform.position + _camera.transform.forward*(0.4f + 0.4f * Random.value);
		_current.transform.LookAt(_projectionCam.transform.position);
		_current.gameObject.SetActive(true);
		
		//初期化
		_scale = 0.15f + 0.1f * Random.value;
		_current.Init(
			projMat,
			viewMat,
			_scale
		);

		//三個消す
		if(_count%5==0){
			for(int i=0;i<_list.Count;i++){
				_list[i].Hide();
			}
		}

	}

}
