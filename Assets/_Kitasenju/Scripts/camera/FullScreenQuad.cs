using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FullScreenQuad : MonoBehaviour
{
    private MeshRenderer _renderer;
    
    public void SetMaterial( Material mat ){
        if(_renderer==null){
            _renderer = GetComponent<MeshRenderer>();
        }
        _renderer.sharedMaterial = mat;
    }    

}
