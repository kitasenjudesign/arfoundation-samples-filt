using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Main : MonoBehaviour
{
    private Vector2Int baseScale = new Vector2Int();
    private int _index = 0;
    [SerializeField] private GameObject NGPanel;
    [SerializeField] private ARSession _arSession;
    [SerializeField] private ARSessionOrigin _arSessionOrigin;
    private GUIStyle _style;

    // Start is called before the first frame update
    void Awake()
    {
        
        Debug.Log(Application.version);
        Debug.Log(Screen.width + " " + Screen.height);


        baseScale=new Vector2Int(Screen.width,Screen.height);
        //Screen.height

        if( !DeviceChecker.GetAvailable() ){
            //使用不能
            NGPanel.gameObject.SetActive(true);
        }else{
            //いける
            _arSession.gameObject.SetActive(true);
            _arSessionOrigin.gameObject.SetActive(true);
        }

        //初期化
        Params.Init();

    }


    
    /*
    void OnGUI(){

        if(_style==null){
            _style = new GUIStyle();
            _style.fontSize = 50;
            _style.normal.textColor = Color.white; 
        }

        GUI.Label(
            new Rect(50, 50, 500, 100), 
            "" + Params.microphoneVolume,
            _style
        );
    }
    */

    #if DEMO
    
    void OnApplicationPause (bool pauseStatus)
    {
        if (pauseStatus) {
            //Debug.Log("applicationWillResignActive or onPause");
            Application.Quit();
        } else {
            //Debug.Log("applicationDidBecomeActive or onResume");
        }
    }

    #endif


    // Update is called once per frame
    void Update()
    {
        
        Params.SetRotation();
        
        /*if (Input.touchCount >= 2){
            
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                
                var scales = new float[]{
                    1f,2f,3f,4f,5f
                };
                var ss = scales[_index%scales.Length];
                Params.microphoneVolume=ss;
                _index++;

            }

        }*/

    }



}
