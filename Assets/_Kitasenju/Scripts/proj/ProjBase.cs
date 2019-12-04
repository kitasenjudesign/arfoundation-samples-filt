using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjBase : MonoBehaviour {

    public bool updatingPos = true;

    public virtual void Init(Matrix4x4 projMat, Matrix4x4 viewMat, float baseScale, Vector3[] positions=null){

    }
    
    public virtual void Capture(RenderTexture srcTex){

    }
    
    public virtual void Hide(){
        
    }

    public virtual void Kill(){
        
    }

}