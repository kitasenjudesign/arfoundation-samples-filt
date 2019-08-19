using UnityEngine;

public class CopyMatrixCam : MonoBehaviour
{

    [SerializeField] private Camera _refCam;

    private Camera _thisCam;


    void Start(){
        
        _thisCam = GetComponent<Camera>();
        
    }


    void Update(){
        _thisCam.projectionMatrix = _refCam.projectionMatrix;
    }

}