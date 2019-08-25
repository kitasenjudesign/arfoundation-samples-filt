/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	[RequireComponent(typeof(EventTrigger))]
	public class RecordButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

		public Image button, countdown;
		public UnityEvent onTouchDown, onTouchUp;
		private bool pressed;
		private bool isRecording = false;

		private const float MaxRecordingTime = 10f; // seconds
		public System.Action _staticImageCaptureCallback;
		private float _startTime = 0;
		private void Start () {
			Reset();
		}

		private void Reset () {
			// Reset fill amounts
			if (button) button.fillAmount = 1.0f;
			if (countdown) countdown.fillAmount = 0.0f;
		}

		void IPointerDownHandler.OnPointerDown (PointerEventData eventData) {
			// Start counting
			_startTime=Time.time;
			StartCoroutine (Countdown());
		}

		void IPointerUpHandler.OnPointerUp (PointerEventData eventData) {
			// Reset pressed
			if(Time.time - _startTime<0.2f){
				_staticImageCaptureCallback();
			}

			pressed = false;
			//2byou mimanだったら
		}

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
	}
}