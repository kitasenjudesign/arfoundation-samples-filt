using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodTouches;

public class TouchMoving : MonoBehaviour
{

    [SerializeField] private FilterMenu _filterMenu;
    [SerializeField] private RectTransform _container;
    private float _yy = 0;
    private Vector3 past = new Vector3();
    [SerializeField] public Vector3 velocity= new Vector3();
    private Vector3 startTapPos = new Vector3();
    private Vector3 startObjPos = new Vector3();
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
            //ドラッグしてる時
            if(_isDrag){
                velocity *= 0.8f;
                var now = GodTouch.GetPosition();
                velocity += _speed * (now - past);
                past = GodTouch.GetPosition();
            }

			// タッチを検出して動かす
			var phase = GodTouch.GetPhase ();

            if (phase == GodPhase.Began) 
			{
                _filterMenu.KillTween();
                _isDrag=true;
                past        = GodTouch.GetPosition();
                startTapPos    = GodTouch.GetPosition();
                startObjPos = _container.localPosition;
				//startPos = Move.position;
			}
            else if (phase == GodPhase.Moved) 
			{
				//完全についてくる
                //_filterMenu.position = GodTouch.GetPosition();                
//				Move.position += GodTouch.GetDeltaPosition(); 
                var pp = _container.localPosition;
                pp.x = startObjPos.x - (startTapPos-GodTouch.GetPosition()).x;
                _container.localPosition = pp;
			}
            else if (phase == GodPhase.Ended) 
			{
				//Move.position = startPos;
                _isDrag=false;
			}
            

            var p = _container.localPosition;

            //ドラッグしてない時、速度を反映させる
            if(!_isDrag){
                velocity *= 0.95f;
                p.x += velocity.x;

                //ドラッグしてない時　行き過ぎを補正する
                if(p.x>0){
                    p.x += (0-p.x)/10f;
                    velocity.x*=0.7f;
                }
                if(p.x<-_width){
                    p.x += (-_width-p.x)/10f;
                    velocity.x*=0.7f;
                }

            }



            _container.localPosition = p;

    }
}
