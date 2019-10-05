/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using System.IO;
    using Recorders;

    public class VoiceRecorder : MonoBehaviour {
        
        public AudioSource audioSource;
        private IAudioDevice audioDevice;
        private WAVRecorder audioRecorder;

        private void Start () {
            audioDevice = AudioDevice.GetDevices()[0];
            Debug.Log("Audio device: "+audioDevice);
        }

        public void ToggleRecording (Text buttonText) { // Invoked by UI
            if (!audioDevice.IsRecording) {
                // Create a WAV recorder to record the audio to a file
                #if UNITY_EDITOR
                var wavPath = Path.Combine(Directory.GetCurrentDirectory(), "recording.wav");
                #else
                var wavPath = Path.Combine(Application.persistentDataPath, "recording.wav");
                #endif
                audioRecorder = new WAVRecorder(wavPath);
                // Start recording
                audioDevice.StartRecording(44100, 1, audioRecorder);
                buttonText.text = @"Stop Recording";
            } else {
                // Stop recording
                audioDevice.StopRecording();
                buttonText.text = @"Start Recording";
                // Dispose the recorder
                audioRecorder.Dispose();
                Debug.Log("Saved recording to: "+audioRecorder.FilePath);
                // Play the recording in the scene
                Application.OpenURL(audioRecorder.FilePath);
            }
        }
    }
}