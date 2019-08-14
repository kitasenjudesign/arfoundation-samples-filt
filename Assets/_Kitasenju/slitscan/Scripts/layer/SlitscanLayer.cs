using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SlitscanLayer : MonoBehaviour {

    [SerializeField] protected Mesh _mesh;
    [SerializeField] protected GameObject _src;
    
    [SerializeField] protected Material _mat;
    protected List<LayerData> _data;
    protected int _count = 120;
    private WebCamTexture _webcamTex;
    private List<RenderTexture> _renderTexs;
    [SerializeField] private Texture[] _maps;
    private int _mapIndex = 0;

    void Start(){

        //webcam
        WebCamDevice[] devices = WebCamTexture.devices;
        _webcamTex = new WebCamTexture(devices[0].name, 960, 540, 30);
        _webcamTex.Play();


        //data
        _renderTexs = new List<RenderTexture>();
        _data = new List<LayerData>();

        for(int i=0;i<_count;i++){

            var g = Instantiate(_src,transform,false);
            g.transform.localPosition = new Vector3(
                0,
                0,
                (float)i*0.02f - (_count-1) * 0.02f / 2f
            );

            var d = new LayerData();
            d.gameObject = g;

            d.th.x = (float)i / (float)_count;//min
            d.th.y = ((float)i+1f) / (float)_count;//max
            _renderTexs.Add( d.renderTexture );
            /*
            d.matrix.SetTRS(
                new Vector3(0,0,(float)i*0.01f),
                Quaternion.Euler(0,0,0),
                Vector3.one
            );*/
            d.renderer = g.GetComponent<MeshRenderer>();//.SetPropertyBlock(d.block);
            //d.UpdateRender();


            _data.Add( d );
        }
        _src.gameObject.SetActive(false);

    }

    void OnGUI(){
        //GUI.DrawTexture(new Rect(0,0,100,100),_webcamTex);
        //GUI.DrawTexture(new Rect(0,0,200,100),_data[0].renderTexture);
    }

    void Update(){

        //capture
        Graphics.Blit(_webcamTex,_renderTexs[0]);
        //update ary
        var tgt = _renderTexs[0];
        _renderTexs.RemoveAt(0);
        _renderTexs.Add(tgt);

        //描画
        for(int i=0;i<_count;i++){
            _data[i].Update(_renderTexs[i]);
            //Graphics.DrawMesh(_mesh,_data[i].matrix,_mat,0,Camera.main,0,_data[i].block,false,false,false);
        }

        //change map
        if(Input.GetKeyDown(KeyCode.Space)){
            _mapIndex++;
        }
        _mat.SetTexture("_Displacement",_maps[_mapIndex%_maps.Length]);

    }

}