using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class DrawMeshInstancedBase : ProjBase {

    [SerializeField] protected Mesh _mesh;
    [SerializeField] protected Mesh[] _meshes;
    //[SerializeField] protected float[] _scales;
    [SerializeField] protected Material _mat;
    protected Vector4[] _colors;
    protected MaterialPropertyBlock _propertyBlock;
    protected int _count = 400;//300;

    protected Matrix4x4[] _modelMats;
    protected Matrix4x4[] _baseModelMats;


    protected Matrix4x4[] _viewMats;
    protected Matrix4x4[] _projMats;
    protected bool _isInit = false;
    protected void DrawMesh(){

        /*
        Graphics.DrawMeshInstanced(
                _mesh, 
                0, 
                _mat, 
                _matrices, 
                _count, 
                _propertyBlock, 
                ShadowCastingMode.Off, 
                false, 
                gameObject.layer
        );*/

    }


}