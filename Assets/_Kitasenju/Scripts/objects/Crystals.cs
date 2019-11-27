using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Crystals : MonoBehaviour {

    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] protected Camera _camera;

    private bool _isInit = false;

    void Start(){

        if(_isInit)return;
        _isInit=true;

        for(int i=0;i<50;i++){

            var obj = GameObject.Instantiate( _crystalPrefab,transform,false);
            obj.gameObject.SetActive(true);

            var pos = _camera.transform.position;

            var amp = 1.5f + 10f * Random.value;
            var rad = Random.value * 2f * Mathf.PI;
            var pp = new Vector3(
                pos.x + amp * Mathf.Cos(rad),
                pos.y + 2f * (Random.value - 0.5f),
                pos.z + amp * Mathf.Sin(rad)
            );

            obj.transform.position = pp;

        }

    }

}