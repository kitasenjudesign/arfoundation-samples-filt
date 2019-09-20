using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenQuadByShader : MonoBehaviour
{

    private MeshFilter _meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        var mesh = GetComponent<MeshFilter>().mesh;//sharedMesh;
        mesh.bounds = new Bounds(
            Vector3.zero,
            new Vector3(10000f,10000f,10000f)
        );
        //GetComponent<MeshFilter>().sharedMesh=mesh;
        GetComponent<MeshFilter>().mesh=mesh;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
