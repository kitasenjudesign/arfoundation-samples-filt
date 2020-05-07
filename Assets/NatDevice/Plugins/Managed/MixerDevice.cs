/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices {

    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Internal;

    /// <summary>
    /// Audio device that mixes game audio with microphone audio.
    /// </summary>
    [Doc(@"MixerDevice")]
    public sealed class MixerDevice : IAudioDevice, IDisposable {

        #region --Client API--
        /// <summary>
        /// Device unique ID.
        /// </summary>
        [Doc(@"UniqueID")]
        public string uniqueID => $"({audioDevice.uniqueID}:{attachment.gameObject.name})";

        /// <summary>
        /// Audio sample rate.
        /// </summary>
        [Doc(@"SampleRate")]
        public int sampleRate => AudioSettings.outputSampleRate;

        /// <summary>
        /// Audio channel count.
        /// </summary>
        [Doc(@"ChannelCount")]
        public int channelCount => (int)AudioSettings.speakerMode;

        /// <summary>
        /// Is the device running?
        /// </summary>
        [Doc(@"Running")]
        public bool running => audioDevice.running;

        /// <summary>
        /// Create a mixer device that mixes audio from an audio device with audio from an `AudioSource`.
        /// </summary>
        /// <param name="audioDevice">Audio device.</param>
        /// <param name="audioSource">In-game audio source.</param>
        [Doc(@"MixerDeviceCtorAudioSource")]
        public MixerDevice (IAudioDevice audioDevice, AudioSource audioSource) : this(audioDevice, audioSource.gameObject) { }

        /// <summary>
        /// Create a mixer device that mixes audio from an audio device with audio from an `AudioListener`.
        /// </summary>
        /// <param name="audioDevice">Audio device.</param>
        /// <param name="audioListener">In-game audio listener.</param>
        [Doc(@"MixerDeviceCtorAudioListener")]
        public MixerDevice (IAudioDevice audioDevice, AudioListener audioListener) : this(audioDevice, audioListener.gameObject) { }

        /// <summary>
        /// Start running.
        /// </summary>
        /// <param name="@delegate">Delegate to receive sample buffers.</param>
        [Doc(@"StartRecording")]
        public void StartRunning (SampleBufferDelegate @delegate) {
            attachment.@delegate = (sampleBuffer, timestamp) => {
                lock ((stagingBuffer as ICollection).SyncRoot)
                    stagingBuffer.AddRange(sampleBuffer);
            };
            var copyBuffer = new float[4096];
            audioDevice.StartRunning((sampleBuffer, timestamp) => {
                // Copy
                lock ((stagingBuffer as ICollection).SyncRoot) { // CHECK // Indices are unsafe // Seek to match times
                    stagingBuffer.CopyTo(0, copyBuffer, 0, sampleBuffer.Length);
                    stagingBuffer.RemoveRange(0, sampleBuffer.Length);
                }
                // Mix
                for (var i = 0; i < sampleBuffer.Length; i++)
                    sampleBuffer[i] = (float)Math.Tanh(sampleBuffer[i] + copyBuffer[i]);
                // Send to delegate
                @delegate(sampleBuffer, timestamp);
            });
        }

        /// <summary>
        /// Stop running.
        /// </summary>
        [Doc(@"StopRunning")]
        public void StopRunning () {
            // Stop running
            attachment.@delegate = null;
            audioDevice.StopRunning();
            // Clear staging buffer
            stagingBuffer.Clear();
        }

        /// <summary>
        /// Dispose the device and release resources.
        /// </summary>
        [Doc(@"MixerDeviceDispose")]
        public void Dispose () => MixerDeviceAttachment.Destroy(attachment);
        #endregion


        #region --Operations--

        private readonly IAudioDevice audioDevice;
        private readonly MixerDeviceAttachment attachment;
        private readonly List<float> stagingBuffer;

        private MixerDevice (IAudioDevice audioDevice, GameObject gameObject) {
            // Save state
            this.audioDevice = audioDevice;
            this.attachment = gameObject.AddComponent<MixerDeviceAttachment>();
            this.stagingBuffer = new List<float>();
            // Configure audio format
            if (audioDevice is AudioDevice device) {
                device.sampleRate = AudioSettings.outputSampleRate;
                device.channelCount = (int)AudioSettings.speakerMode;
            }            
        }

        public bool Equals (IMediaDevice other) => other != null && other is MixerDevice && other.uniqueID == uniqueID;

        public override string ToString () => $"MixerDevice ({uniqueID})";

        private class MixerDeviceAttachment : MonoBehaviour {
            public SampleBufferDelegate @delegate;
            private void OnAudioFilterRead (float[] data, int channels) => @delegate?.Invoke(data, 0L);
        }
        #endregion
    }
}