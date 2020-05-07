/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/
/*
    #if UNITY_EDITOR
	using UnityEditor;
	#endif
    using UnityEngine;
    using NatCorder.Clocks;
    using NatCorder.Inputs;
    using NatShareU;
    using NatMic;
    using NatCorder;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.XR.ARFoundation;

    public class MyReplayCamArashi : MonoBehaviour, IAudioProcessor {

 
        [Header("Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;

        [Header("Microphone")]
        public bool recordMicrophone;
        public AudioSource microphoneSource;

        private MP4Recorder videoRecorder;
        private IClock recordingClock;
        private CameraInput cameraInput;
        private AudioInput audioInput;

        [SerializeField,Space(10)] private ReplayPlayer _player;
        [SerializeField] private RawImage rawImage;
        [SerializeField,Space(10)] public Camera cam;
        private string _path = "";


		private int _sampleRate = 44100;
		private int _channelCount = 1;
        private AudioDevice audioDevice;

        private GUIStyle _style;

        void Awake(){
            //_manager.
            //videoPlayer.enabled=false;
        }     



        //public void SetSize(Vector2Int size){
        //    videoWidth = size.x;
        //    videoHeight = size.y;
        //}



        public void StartRecording () {

            recordMicrophone = Params.usingMicrophone;

            var size = Params.GetVideoSize();
            videoWidth  = size.x;
            videoHeight = size.y;       
            //Params.SetScreenSizeByQuality();            

            // Start recording
            recordingClock = new RealtimeClock();
            videoRecorder = new MP4Recorder(
                videoWidth,
                videoHeight,
                30,
                _sampleRate,
                _channelCount,
                //recordMicrophone ? AudioSettings.outputSampleRate : 0,
                //recordMicrophone ? (int)AudioSettings.speakerMode : 0,
                OnReplay,
                Params.GetVideoBitrate()
            );

            // Create recording inputs
            cameraInput = new CameraInput(videoRecorder, recordingClock, cam);//Camera.main);
            if (recordMicrophone) {
                
                //Invoke("StartMicrophone",0.3f);
                Invoke("StartMicrophone",0.1f);

            }
            
        }

        private void StartMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(


    		audioDevice = Params.audioDevice;
	    	audioDevice.StartRecording(_sampleRate, _channelCount, this);            
            audioInput = new AudioInput(
                videoRecorder, recordingClock, microphoneSource, false//true
            );
            #endif
        }

	public void OnSampleBuffer (float[] sampleBuffer, int sampleRate, int channelCount, long timestamp) {
		// Send sample buffers directly to the video recorder for recording
        for(int i=0;i<sampleBuffer.Length;i++){
            sampleBuffer[i] = sampleBuffer[i] * Params.microphoneVolume;
        }
		videoRecorder.CommitSamples(sampleBuffer, recordingClock.Timestamp);
	}

    public void StopRecording () {

            ///Params.ResetScreenSize();

        // Stop the recording inputs
        if (recordMicrophone) {
            StopMicrophone();
            audioInput.Dispose();
        }
        cameraInput.Dispose();
        // Stop recording
        videoRecorder.Dispose();
    }

    private void StopMicrophone () {
        #if !UNITY_WEBGL || UNITY_EDITOR
        audioDevice.StopRecording();
        //Microphone.End(null);
        //microphoneSource.Stop();
        #endif
    }


    private void OnReplay (string path) {

        Debug.Log(path);
        _player.ShowReplay(path);
        
    }

        

    void Update() {

    }


}*/
