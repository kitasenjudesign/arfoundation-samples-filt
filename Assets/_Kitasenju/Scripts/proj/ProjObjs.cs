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
	private RenderTexture _captureTestTex;
	private ProjBase _current;
	private int _count = 0;
    private EffectControlMain _main;
    [SerializeField] private RenderTexture _camTex;

	public void Init (EffectControlMain main) {

		_main = main;

		_list = new List<ProjBase>();

		for(int i=0;i<_src.Length; i++){
			_src[i].gameObject.SetActive(false);
		}


		_captureTestTex = new RenderTexture(Screen.width,Screen.height,0);
		
	}
	

	private void OnGUI()
	{
		//float nn = 0.1f;
        /*
		GUI.DrawTexture(
            new Rect(0, 0, Mathf.FloorToInt(Screen.width*nn),Mathf.FloorToInt(Screen.height*nn)), 
            _captureTestTex, 
            ScaleMode.StretchToFill,
            false
        ); 
		GUI.DrawTexture(
            new Rect(0, Mathf.FloorToInt(Screen.height*nn), Mathf.FloorToInt(Screen.width*nn),Mathf.FloorToInt(Screen.height*nn)), 
            _main._humanBodyManager, 
            ScaleMode.StretchToFill,
            false
        );*/

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

	
		//instansiate
		_current = Instantiate( _src[_count%_src.Length], transform, false );
      
        _current.gameObject.SetActive(true);
		_count++;

		_list.Add(_current);

		//多すぎると削除
		if(_list.Count>20){
			var tgt = _list[0];
			tgt.Kill();
			_list.RemoveAt(0);
			Destroy(tgt.gameObject);
		}

		_current.gameObject.SetActive(false);//自分は消す
        
        //if(_main._arBackground.material){
        //    Graphics.Blit(null,_main._camTex,_main._arBackground.material);
        //}
		_current.Capture(_camTex);//相手と背景をキャプチャ

		//0.05sec後に表示する
		Invoke("_SetPos",0.05f);
	}

	void _SetPos(){

		Debug.Log("_SetPos!!!!");

		var projMat = _projectionCam.projectionMatrix;
		var viewMat = _projectionCam.worldToCameraMatrix;
		
		//_current.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
		_current.transform.position = _camera.transform.position + _camera.transform.forward*(1f + 3f * Random.value);
		_current.transform.LookAt(_projectionCam.transform.position);
		_current.gameObject.SetActive(true);
		
		//初期化
		_current.Init(
			projMat,
			viewMat,
			(Random.value>0.5f) ? 0.2f : 0.9f
		);

		//三個消す
		//if(_count%3==0){
			//for(int i=0;i<_list.Count;i++){
			//	_list[i].Hide();
			//}
		//}

	}

}
