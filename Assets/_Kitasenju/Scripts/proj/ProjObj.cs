using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityEngine.XR.iOS;

//using DG.Tweening;

public class ProjObj : ProjBase {

	//[SerializeField] private UnityARVideo _arVideo;
	private MeshRenderer _renderer;
	private MaterialPropertyBlock _propertyBlock;

	private Vector3 _rotSpeed;
	private Vector3 _rot;
	private Matrix4x4 _modelMat;
	private Matrix4x4 _tMat;
	private Matrix4x4 _projMat;
	private Matrix4x4 _viewMat;
	private RenderTexture _renderTexture;
	private bool _isInit=false;
	
	private float _scaleY = 0f;
	// Use this for initialization
	void Awake () {

	}

	private void _Init(){

		if(_isInit)return;
		_isInit=true;

		_tMat = new Matrix4x4(
			new Vector4(0.5f,0,0,0),//m00,m10,m20,m30
			new Vector4(0,0.5f,0,0),//m01,m11,m21,m31
			new Vector4(0,0,1f,0),//m02,m12,m22,m32
			new Vector4(0.5f,0.5f,0,1f)//m03,m13,m23,m33
		);

		_rotSpeed = new Vector3(
			1.5f*(Random.value-0.5f),
			1.5f*(Random.value-0.5f),
			1.5f*(Random.value-0.5f)
		);

		_rot = transform.rotation.eulerAngles;

		_propertyBlock = new MaterialPropertyBlock();
		_renderer =GetComponent<MeshRenderer>();
		
		_renderTexture = new RenderTexture(
			Mathf.FloorToInt(Screen.width*0.5f),
			Mathf.FloorToInt(Screen.height*0.5f),0
		);

		//transform.localScale=new Vector3(
		//	0.5f,0.5f,0.5f
		//);

	}

    /**
        RenderTextureにsrcTexを書き込む
     */
	public override void Capture(RenderTexture srcTex){

		_Init();

		//gameObject.SetActive(false);
        if(_renderTexture == null){
            _renderTexture = DataManager.Instance.texStorage.GetNextColorTexture();  //new RenderTexture(Screen.width,Screen.height,0);                  
        }

		Graphics.Blit( srcTex, _renderTexture );/////
		_propertyBlock.SetTexture("_MainTex", _renderTexture);

	}

	/**
		カメラのマトリクスを渡す
	 */
	public override void Init(
		Matrix4x4 projMat, Matrix4x4 viewMat, float baseScale, Vector3[] positions=null	
	){
		
		_Init();

		//Debug.Log("SetMat");
		_projMat = projMat;
		_viewMat = viewMat;

		gameObject.SetActive(true);

		//_mat = _renderer.localToWorldMatrix;
		_modelMat = transform.localToWorldMatrix;
		if(_propertyBlock!=null){

			_propertyBlock.SetMatrix("_modelMat", _modelMat );
			_propertyBlock.SetMatrix("_tMat", 		_tMat );
			_propertyBlock.SetMatrix("_projMat", 	_projMat);//_projectionCam.projectionMatrix );
			_propertyBlock.SetMatrix("_viewMat", 	_viewMat);
			_propertyBlock.SetTexture("_MainTex", _renderTexture );

		}

		if(_renderer!=null){
			_renderer.SetPropertyBlock(_propertyBlock);
		}
		
	}

    public override void Hide(){
		transform.DOScale(Vector3.zero,1f).SetEase(Ease.Linear);
	}	

	// Update is called once per frame
	void Update () {

		if(_propertyBlock!=null){
			_propertyBlock.SetTexture("_MainTex", _renderTexture );
		}

		if(_renderer!=null) _renderer.SetPropertyBlock(_propertyBlock);
		//transform.Rotate(_rotSpeed);

	}

}
