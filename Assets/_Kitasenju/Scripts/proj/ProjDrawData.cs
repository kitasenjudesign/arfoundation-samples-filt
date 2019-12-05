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
    public Quaternion rot = Quaternion.Euler(0,0,0);
    public Vector3 deg = new Vector3();
    public Vector3 scale = new Vector3(0.1f,0.1f,0.1f);
    public Vector3 scaleSpeed = new Vector3(0,0,0);
    public Vector3 degSpeed = new Vector3(0,0,0);
    public Vector3 degRandom = new Vector3(0,0,0);

    public Vector4 uv = new Vector4();
    public Matrix4x4 modelMat;
    public Matrix4x4 projMat;
    public Matrix4x4 viewMat;
    public Vector3 velocity;
    public Vector3 basePos;
    private int count=0;
    private float _pastTime;
    private bool _isRot = false;

    public void Init(bool isRotAnim){

        count = Mathf.FloorToInt(Random.value * 100f);
        velocity = Vector3.zero;//
        
        deg = Vector3.zero;
        rot = Quaternion.identity;

        degRandom = new Vector3(
            1f * (Random.value - 0.5f ),
            1f * (Random.value - 0.5f ),
            1f * (Random.value - 0.5f )
        );
        if(isRotAnim){
            degRandom = Vector3.zero;
        }


        degSpeed = new Vector3();

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

        ////// 回転
        #if VJ

            if( MicFFT.Instance ){

                if( MicFFT.Instance.subValues[3] > 0.1f && Time.realtimeSinceStartup - _pastTime>0.33f ){
                    _pastTime = Time.realtimeSinceStartup;
                    degSpeed += 10f * degRandom;
                }

                degSpeed *= 0.99f; 
                deg += degSpeed;
                rot = Quaternion.Euler( deg );
                                
                //velocity.y = 0.001f + 0.1f * MicFFT.Instance.values[3];
            }

        #else

            deg += degRandom;
            rot = Quaternion.Euler( deg );

        #endif


        pos += velocity;

        count++;
        if( count > 120){
            count = 0;
            pos = basePos;
        }

    }




}