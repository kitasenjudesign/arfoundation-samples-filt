using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSlitScanSample : MonoBehaviour
{
    [SerializeField] private Shader _shader;
    [SerializeField] private RenderTexture _inputTex;
    private SimpleSlitScan _slitscan;
    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _material = new Material(_shader);
        _slitscan = new SimpleSlitScan(
            _inputTex.width,
            _inputTex.height,
            _inputTex.format,
            _material
        );

    }

    // Update is called once per frame
    void Update()
    {
        
        var output = _slitscan.Update( _inputTex );
        GetComponent<MeshRenderer>().material.mainTexture = output;

    }
}
