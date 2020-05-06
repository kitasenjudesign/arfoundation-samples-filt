using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLight : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //var cam = Camera.main;
        

        if(_cam){

            var tgt = _cam.transform.position + _cam.transform.forward;
            transform.position = _cam.transform.position + _cam.transform.up;
            transform.LookAt( tgt, _cam.transform.up );

        }

    }
}
