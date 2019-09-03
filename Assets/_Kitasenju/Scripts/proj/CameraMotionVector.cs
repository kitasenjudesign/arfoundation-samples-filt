using UnityEngine;
//using UnityEngine.XR.iOS;

public class CameraMotionVector : MonoBehaviour
{

    //[SerializeField] Material _quadMat;
    //[SerializeField] GameObject _quad;
    [SerializeField] Camera _camera;
    [SerializeField,Range(0,0.1f)] float _Limit;
    [SerializeField,Range(0,1f)] float _Speed;
    private Vector3 _oldPosition;
    private Vector3 _oldViewPosition;
    private Vector3 _currentViewPosition;
    private GUIStyle _style;
    private Vector3 _dir;
    public Vector4 _distance;
    [SerializeField] private float _Pow=1f;
    private int _count = 0;
    void Start(){
        _dir = new Vector4();
        _distance = new Vector4();
        _oldPosition=new Vector3();
        _check();

    }

    /*
    void OnGUI(){

        if(_style==null){
            _style = new GUIStyle();
            _style.fontSize = 30;
            _style.normal.textColor = Color.white;   
        }

        GUI.Label(new Rect(100, 300, 500, 200), "x"+_distance.x + "_y"+_distance.y,_style);
        GUI.Label(new Rect(100, 350, 500, 200), "x"+_oldViewPosition.x + "_y"+_oldViewPosition.y,_style);
        GUI.Label(new Rect(100, 400, 500, 200), "x"+_currentViewPosition.x + "_y"+_currentViewPosition.y,_style); 
    }*/

    void _check(){

        var tarPos = _camera.transform.position + _camera.transform.forward;
        _currentViewPosition = _camera.WorldToViewportPoint(tarPos);//_quad.transform.position);
        _oldViewPosition = _camera.WorldToViewportPoint(_oldPosition);

        var tgt = new Vector3(
            5f*( _currentViewPosition.x - _oldViewPosition.x ),
            5f*( _currentViewPosition.y - _oldViewPosition.y ),
            5f*( _currentViewPosition.z - _oldViewPosition.z )
        );
        var vv =new Vector2(tgt.x,tgt.y);
        
        //if(vv.magnitude>_Limit){
            //vv.Normalize();
            
            //if(isMotion){
            if(vv.magnitude>_Limit){
                _dir.x = Mathf.Sign(vv.x) * Mathf.Min( _Speed, Mathf.Pow( Mathf.Abs(vv.x), _Pow ) );
                _dir.y = Mathf.Sign(vv.y) * Mathf.Min( _Speed, Mathf.Pow( Mathf.Abs(vv.y), _Pow ) );
            }

            //}else{
            //    tgt.x=0;
            //    tgt.y=0;
            //}
            _distance.x += ( _dir.x - _distance.x) / 10f;
            _distance.y += ( _dir.y - _distance.y) / 10f;


        //}
        //_quadMat.SetVector("_Movement",_distance);

        _oldPosition = tarPos;//_quad.transform.position;
        _count++;

        //Invoke("_check",0.1f);
    }



    void Update(){
        

        _check();

    }


}