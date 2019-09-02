using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjDrawData {

    public Vector3 pos = new Vector3(
        5f * ( Random.value-0.5f ),
        5f * ( Random.value-0.5f ),
        5f * ( Random.value-0.5f )       
    );
    public Quaternion rot =Quaternion.Euler(0,0,0);
    public Vector3 scale = new Vector3(0.1f,0.1f,0.1f);
    public Vector3 scaleSpeed = new Vector3(0,0,0);
    public Vector4 uv = new Vector4();
    public Matrix4x4 modelMat;
    public Matrix4x4 projMat;
    public Matrix4x4 viewMat;
    public Vector3 velocity;
    public Vector3 basePos;
    private int count=0;

    public void Init(){
        count = Mathf.FloorToInt(Random.value * 100f);
        velocity = Vector3.zero;//
        /* new Vector3(
            (Random.value-0.5f) * 0.02f,
            (Random.value-0.5f) * 0.02f,
            (Random.value-0.5f) * 0.02f
        );*/
        //modelMat = Matrix4x4.identity;

    }

    public void Update(){
        
        scale += scaleSpeed;
        if(scale.x < 0){
            scale.Set(0,0,0);
        }

        //位置を動かすかどうか

        pos += velocity;

        count++;
        if( count > 120){
            count = 0;
            pos = basePos;
        }

    }




}