using UnityEngine;

public class CopyMatrixCam : MonoBehaviour
{

    [SerializeField] private Camera _refCam;

    private Camera _thisCam;
    private GUIStyle _style;

    void Start(){
        _thisCam = GetComponent<Camera>();
    }

    /*
    private void OnGUI()
    {
        if(_style==null){
            _style = new GUIStyle();
            _style.fontSize = 40;
            _style.normal.textColor = Color.red;                
        }

        var str = 
        transform.position.x + "\n" + 
        transform.position.y + "\n" + 
        transform.position.z;

        //Debug.Log(timeCode=="");
        GUI.Label(
            new Rect(50,350, 200, 100),
            str,
            _style
        );
        
    }*/


    void Update(){
        _thisCam.projectionMatrix = _refCam.projectionMatrix;
    }

}