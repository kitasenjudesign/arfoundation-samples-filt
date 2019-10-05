/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.Recorders {

    using UnityEngine;
    using System;
    using System.IO;
    using Internal;

    /// <summary>
    /// Recorder for recording audio to an AudioClip
    /// </summary>
    [Doc(@"ClipRecorder")]
    public sealed class ClipRecorder : IAudioRecorder {

        #region --Client API--
        /// <summary>
        /// Create an AudioClip recorder
        /// </summary>
        /// <param name="callback">Callback invoked with AudioClip once the recording is finished</param>
        [Doc(@"ClipRecorderCtor")]
        public ClipRecorder (Action<AudioClip> callback) {
            this.callback = callback;
            this.audioBuffer = new MemoryStream();
        }

        /// <summary>
        /// Stop writing and invoke any callbacks
        /// </summary>
        [Doc(@"Dispose")]
        public void Dispose () {
            if (audioBuffer.Length == 0) {
                audioBuffer.Dispose();
                return;
            }
            var byteSamples = audioBuffer.ToArray();
            var totalSampleCount = byteSamples.Length / sizeof(float); 
            var samples = new float[totalSampleCount];  
            var recordingName = string.Format("recording_{0}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
            Buffer.BlockCopy(byteSamples, 0, samples, 0, byteSamples.Length);
            audioBuffer.Dispose();
            audioBuffer = null;
            // Create audio clip
            var audioClip = AudioClip.Create(recordingName, totalSampleCount / channelCount, channelCount, sampleRate, false);
            audioClip.SetData(samples, 0);
            callback(audioClip);
        }
        #endregion


        #region --Operations--

        private readonly Action<AudioClip> callback;
        private MemoryStream audioBuffer;
        private int sampleRate, channelCount;

        void IAudioProcessor.OnSampleBuffer (float[] sampleBuffer, int sampleRate, int channelCount, long timestamp) {
            this.sampleRate = sampleRate;
            this.channelCount = channelCount;
            var byteSamples = new byte[Buffer.ByteLength(sampleBuffer)];
            Buffer.BlockCopy(sampleBuffer, 0, byteSamples, 0, byteSamples.Length);
            audioBuffer.Write(byteSamples, 0, byteSamples.Length);
        }
        #endregion
    }
}