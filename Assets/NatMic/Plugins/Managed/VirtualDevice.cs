/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic {

    using UnityEngine;
    using System;
    using Internal;

    /// <summary>
    /// A virtual audio device backed by a Unity audio component
    /// </summary>
    [Doc(@"VirtualDevice")]
    public sealed class VirtualDevice : IAudioDevice, IDisposable {

        #region --Client API--
        /// <summary>
        /// Create a virtual audio device backed by an AudioSource
        /// </summary>
        /// <param name="audioSource">Backing AudioSource for this audio device</param>
        [Doc(@"VirtualDeviceCtorSource")]
        public VirtualDevice (AudioSource audioSource) : this(audioSource as Component) {}

        /// <summary>
        /// Create a virtual audio device backed by an AudioListener
        /// </summary>
        /// <param name="audioListener">Backing AudioListener for this audio device</param>
        [Doc(@"VirtualDeviceCtorListener")]
        public VirtualDevice (AudioListener audioListener) : this(audioListener as Component) {}

        /// <summary>
        /// Dispose the virtual device and release resources
        /// </summary>
        [Doc(@"VirtualDeviceDispose")]
        public void Dispose () {
            VirtualDeviceAttachment.Destroy(attachment);
        }
        #endregion


        #region --IAudioDevice--
        /// <summary>
        /// Is the device currently recording?
        /// </summary>
        [Doc(@"IsRecording")]
        public bool IsRecording {
            get { return processor != null; }
        }

        /// <summary>
        /// Start recording from the audio device
        /// </summary>
        /// <param name="requestedSampleRate">Requested sample rate</param>
        /// <param name="requestedChannelCount">Requested channel count</param>
        /// <param name="processor">Delegate to receive audio sample buffers</param>
        [Doc(@"StartRecording")]
        public void StartRecording (int requestedSampleRate, int requestedChannelCount, IAudioProcessor processor) {
            this.processor = processor;
        }

        /// <summary>
        /// Stop recording from the audio device
        /// </summary>
        [Doc(@"StopRecording")]
        public void StopRecording () {
            this.processor = null;
        }
        #endregion


        #region --Operations--

        private readonly VirtualDeviceAttachment attachment;
        private readonly int sampleRate;
        private volatile IAudioProcessor processor;

        private VirtualDevice (Component component) {
            this.attachment = component.gameObject.AddComponent<VirtualDeviceAttachment>();
            this.attachment.handler = OnSampleBuffer;
            this.sampleRate = AudioSettings.outputSampleRate;
        }

        private void OnSampleBuffer (float[] data, int channels) {
            if (IsRecording)
                try {
                    processor.OnSampleBuffer(data, sampleRate, channels, AudioDevice.CurrentTimestamp);
                } catch (Exception ex) {
                    Debug.LogError("NatMic Error: VirtualDevice processor raised exception: "+ex);
                }
        }

        public override string ToString () {
            return attachment.gameObject.ToString() + "(GameObject)";
        }

        private class VirtualDeviceAttachment : MonoBehaviour {
            public Action<float[], int> handler;
            private void OnAudioFilterRead (float[] data, int channels) { handler(data, channels); }
        }
        #endregion
    }
}