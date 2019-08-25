

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
    using DG.Tweening;

	[RequireComponent(typeof(EventTrigger))]
	public class MyRecordBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

		public Image button, countdown;
		public UnityEvent onTouchDown, onTouchUp;
		private bool pressed;
		//private bool isRecording = false;
        private bool _isRecording = false;
		private const float MaxRecordingTime = 10f; // seconds
		//public System.Action _staticImageCaptureCallback;
		private float _startTime = 0;
        [SerializeField] private NatCorder.Examples.MyReplayCam _replayCam;
        [SerializeField] private Image circle;
        [SerializeField] private Image sq;

		private void Start () {
			Reset();
		}

		private void Reset () {
			// Reset fill amounts
            if( sq ) sq.enabled=false;
            if( circle ) circle.enabled = true; 
			if (button) button.fillAmount = 1.0f;
			if (countdown) countdown.fillAmount = 0.0f;
		}

		void IPointerDownHandler.OnPointerDown (PointerEventData eventData) {
            Debug.Log("start");
			// Start counting

            if(_isRecording){
                
                _stopRec();
            }else{
                if( sq ) sq.enabled=true;
                if( circle ) circle.enabled = false; 

                var rect = sq.GetComponent<RectTransform>();
                rect.transform.localScale = Vector3.one*1.2f;
                rect.DOScale(Vector3.one,0.5f).SetEase(Ease.OutSine);


                _isRecording=true;
                _startTime=Time.time;
                _replayCam.StartRecording();
            }
            
			//StartCoroutine (Countdown());
		}

		void IPointerUpHandler.OnPointerUp (PointerEventData eventData) {
            Debug.Log("end");
			// Reset pressed
			pressed = false;
			//2byou mimanだったら
		}

        private void _stopRec(){
            _isRecording=false;
            _replayCam.StopRecording();
            
            Reset();
        }

        void Update(){

            if(_isRecording){
                var ratio = (Time.time - _startTime) / MaxRecordingTime;
                
				countdown.fillAmount = ratio;
				button.fillAmount = 1f - ratio;

                if(ratio>1f){
                    _stopRec();
                }                
            }

        }


        /* 
		private IEnumerator Countdown () {
			
			pressed = true;
			// First wait a short time to make sure it's not a tap
			yield return new WaitForSeconds(0.2f);




			if (!pressed) yield break;
			// Start recording
			if (onTouchDown != null) onTouchDown.Invoke();
			// Animate the countdown
			float startTime = Time.time, ratio = 0f;
			while (pressed && (ratio = (Time.time - startTime) / MaxRecordingTime) < 1.0f) {
				countdown.fillAmount = ratio;
				button.fillAmount = 1f - ratio;
				yield return null;
			}
			// Reset
			Reset();
			// Stop recording
			if (onTouchUp != null) onTouchUp.Invoke();

		}
        */
	}
