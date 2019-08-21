using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodTouches;

public class TouchTester : MonoBehaviour
{

    [SerializeField] private RectTransform _filterMenu;
    private float _yy = 0;
    private Vector3 past = new Vector3();
    private Vector3 velocity= new Vector3();
    private bool _isDrag = false;
    [SerializeField,Range(0,1f)] private float _speed = 0.3f;
    [SerializeField] private float _width = 2500f;

    // Start is called before the first frame update
    void Awake()
    {
        past = GodTouch.GetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
			//if (ClickLongPressDrag.IsRunning) return; // 他のサンプルが動作してる時は無効

            if(_isDrag){
                var now = GodTouch.GetPosition();
                velocity += _speed * (now - past);
                past = GodTouch.GetPosition();
            }

			// タッチを検出して動かす
			var phase = GodTouch.GetPhase ();

            if (phase == GodPhase.Began) 
			{
                _isDrag=true;
                past = GodTouch.GetPosition();
				//startPos = Move.position;
			}
            else if (phase == GodPhase.Moved) 
			{
				//完全についてくる
                //_filterMenu.position = GodTouch.GetPosition();                
//				Move.position += GodTouch.GetDeltaPosition(); 
			}
            else if (phase == GodPhase.Ended) 
			{
				//Move.position = startPos;
                _isDrag=false;
			}
            

            

            velocity *= 0.95f;
            

            var p = _filterMenu.localPosition;
            p.x += velocity.x;

            if(p.x>0){
                p.x += (0-p.x)/10f;
                velocity.x=0;
            }
            if(p.x<-_width){
                p.x += (-_width-p.x)/10f;
                velocity.x=0;
            }

            _filterMenu.localPosition = p;

    }
}
