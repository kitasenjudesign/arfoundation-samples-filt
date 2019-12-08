/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

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

    public class MyReplayCam : MonoBehaviour, IAudioProcessor {

        /**
        * ReplayCam Example
        * -----------------
        * This example records the screen using a `CameraRecorder`.
        * When we want mic audio, we play the mic to an AudioSource and record the audio source using an `AudioRecorder`
        * -----------------
        * Note that UI canvases in Overlay mode cannot be recorded, so we use a different mode (this is a Unity issue)
        */

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

        /*
        void OnGUI(){
            if(_style==null){
                _style = new GUIStyle();
                _style.fontSize = 40;
                _style.normal.textColor = Color.white;                
            }
        
            var str = "recMic" + recordMicrophone + "\n";
            str += "w" + videoWidth + "\n";
            str += "h" + videoHeight + "\n";
            str += "q" + Params.videoQuality + " " + Params.GetVideoBitrate();

            //Debug.Log(timeCode=="");
            GUI.Label(
                new Rect(10,50, 200, 100),
                str,
                _style
            );
        }*/

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
            /* 
            // Create a microphone clip
            microphoneSource.clip = Microphone.Start(null, true, 60, 48000);
            while (Microphone.GetPosition(null) <= 0) ;
            // Play through audio source
            microphoneSource.timeSamples = Microphone.GetPosition(null);
            microphoneSource.loop = true;
            microphoneSource.Play();
            */

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


}
