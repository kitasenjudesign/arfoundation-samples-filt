using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LayerData {

    public Vector3 pos = new Vector3(
        5f * ( Random.value-0.5f ),
        5f * ( Random.value-0.5f ),
        5f * ( Random.value-0.5f )       
    );
    public Vector3 scale = new Vector3(1f,1f,1f);
    public RenderTexture renderTexture;
    public MaterialPropertyBlock block;
    public Matrix4x4 matrix;
    public Vector4 th;
    public GameObject gameObject;
    public MeshRenderer renderer;

    public LayerData(){

        renderTexture = new RenderTexture(
            Mathf.FloorToInt(Screen.width/2),
            Mathf.FloorToInt(Screen.height/2),
            0
        );

        block = new MaterialPropertyBlock();
        //matrix = Matrix4x4.identity;

    }

    public void Update(RenderTexture tex){

        //renderer.material.SetTexture("_MainTex",renderTexture);
        //renderer.material.SetVector("_Th",th);
        block.SetVector("_Th",th);
        block.SetTexture("_MainTex",tex);
        if(renderer!=null){
            renderer.SetPropertyBlock(block);
        }
    }

}