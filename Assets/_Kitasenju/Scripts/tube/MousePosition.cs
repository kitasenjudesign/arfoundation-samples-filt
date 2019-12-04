using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition {

    public List<Vector3> _positions;
    private int NUM = 23;
    private Vector3 _oldPos;

    public void Init(int nn){

        NUM = nn;
        _oldPos = Vector3.zero;
        _positions = new List<Vector3>();
        for(int i=0;i<NUM;i++){
            _positions.Add(Vector3.zero);
        }

    }

    public void Upadate(Camera camera){

        if ( Input.GetMouseButton (0) || Input.touchCount > 0) {
            
            //Debug.Log ("スクリーン座標" + Input.mousePosition);
            #if UNITY_EDITOR

                Vector3 screen_point = Input.mousePosition;

            #else
                
                Touch touch = Input.GetTouch(0);
                Vector3 screen_point = new Vector3(
                    touch.position.x,
                    touch.position.y,
                    0
                );

            #endif

            
            screen_point.z = 1.0f;

            var sub = screen_point - _oldPos;
            if( sub.magnitude > 20f){

                _oldPos = screen_point;

                var pos = camera.ScreenToWorldPoint (screen_point);

                if(_positions.Count>=NUM)_positions.RemoveAt(_positions.Count-1);
                _positions.Insert(0,pos);

            }

            //Debug.Log ("ワールド座標" + camera.ScreenToWorldPoint (screen_point));

        }

    }

}