using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BallData {

    public Vector3 pos = new Vector3(
        5f * ( Random.value-0.5f ),
        5f * ( Random.value-0.5f ),
        5f * ( Random.value-0.5f )       
    );
    public Quaternion rot =Quaternion.Euler(0,0,0);
    public Vector3 scale = new Vector3(1f,1f,1f);

    public Vector4 uv = new Vector4();
    private Vector3 offset = new Vector3();
    private Vector3 basePos = new Vector3();

    public void Init(){

        basePos.x = pos.x;
        basePos.y = pos.y;
        basePos.z = pos.z;

    }


    public void Update(){
        
        offset.y = 0.2f * Mathf.Sin( Time.realtimeSinceStartup + basePos.x );
        pos = basePos + offset;

    }

}