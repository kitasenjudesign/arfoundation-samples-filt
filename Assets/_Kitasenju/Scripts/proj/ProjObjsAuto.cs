using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ProjObjsAuto : ProjObjs {

	private float _time = 0;

	// Update is called once per frame
	void Update () {
		
		if( Time.realtimeSinceStartup - _time > 0.5f){
			
			_time=Time.realtimeSinceStartup;
			MakeObj2();

		}

		
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
			if( touch.position.y < Screen.height*0.333f) return;

            if (touch.phase == TouchPhase.Began)// || touch.phase==TouchPhase.Stationary)
            {
				MakeObj2();
			}
		}

        if (Input.touchCount == 3)
        {
            Touch touch = Input.GetTouch(0);
			if( touch.position.y < Screen.height*0.333f) return;

            if (touch.phase == TouchPhase.Began)// || touch.phase==TouchPhase.Stationary)
            {			
            	_Remove();
			}
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			MakeObj2();
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

	public void MakeObj2(){

		_current = Instantiate( _src[_count%_src.Length], transform, false );
        _current.gameObject.SetActive(true);
		_count++;

		Debug.Log("MakeObj " + _current);
		_list.Add(_current);

		//多すぎると削除
		if(_list.Count>20){
			var tgt = _list[0];
			tgt.Kill();
			_list.RemoveAt(0);
			Destroy(tgt.gameObject);
		}

			Texture2D humanStencil  = _main._humanBodyManager.humanStencilTexture;
			_simpleMaskMat.SetTexture("_StencilTex",humanStencil);
			_simpleMaskMat.SetTexture("_MainTex",_camTexMaker._tex);
			Graphics.Blit(
				_camTexMaker._tex,_renderTex,_simpleMaskMat
			);
			_current.Capture(  _renderTex );
			

		//0.05sec後に表示する
		Invoke("_SetPos2",0.05f);
	}

	void _SetPos2(){

		Debug.Log("_SetPos!!!!");

		var projMat = _projectionCam.projectionMatrix;
		var viewMat = _projectionCam.worldToCameraMatrix;
		
		_current.transform.position = _camera.transform.position + _camera.transform.forward*(0.4f + 0.4f * Random.value);
		//_current.transform.LookAt(
		//	_projectionCam.transform.position,
		//	new Vector3(0.3f*Random.value,1f,0.3f*Random.value)
		//);
		_current.transform.localRotation = Quaternion.Euler(
			360f * Random.value,
			360f * Random.value,
			360f * Random.value
		);
		_current.gameObject.SetActive(true);
		
		//初期化
		_scale = 0.1f + 0.1f * Random.value;
		if(_count%3==0) _scale = 0.2f + 0.2f * Random.value;

		_current.Init(
			projMat,
			viewMat,
			_scale
		);

	}

}
