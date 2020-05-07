/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic {

    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Internal;

    /// <summary>
    /// Virtual device that mixes audio from two devices into one stream
    /// </summary>
    [Doc(@"MixerDevice")]
    public sealed class MixerDevice : IAudioDevice {

        #region --Client API--
        /// <summary>
        /// Create a mixer device that mixes audio from multiple audio devices
        /// </summary>
        /// <param name="sources">Source audio devices to mix audio from</param>
        [Doc(@"MixerDeviceCtor")]
        public MixerDevice (IAudioDevice sourceA, IAudioDevice sourceB) {
            this.sourceA = sourceA;
            this.sourceB = sourceB;
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
        /// Start recording from the audio device.
        /// All source devices MUST support the requested sample rate and channel count.
        /// </summary>
        /// <param name="requestedSampleRate">Requested sample rate</param>
        /// <param name="requestedChannelCount">Requested channel count</param>
        /// <param name="processor">Delegate to receive audio sample buffers</param>
        [Doc(@"StartRecording", @"MixerDeviceStartRecordingDescription")]
        public void StartRecording (int requestedSampleRate, int requestedChannelCount, IAudioProcessor processor) { 
            this.processor = processor;
            this.sampleBufferA = new List<float>();
            this.sampleBufferB = new List<float>();
            this.initialTimestampA = -1L;
            this.initialTimestampB = -1L;
            sourceA.StartRecording(requestedSampleRate, requestedChannelCount, new PassThroughProcessor(OnSampleBufferA));
            sourceB.StartRecording(requestedSampleRate, requestedChannelCount, new PassThroughProcessor(OnSampleBufferB));
        }

        /// <summary>
        /// Stop recording from the audio device
        /// </summary>
        [Doc(@"StopRecording")]
        public void StopRecording () {
            sourceA.StopRecording();
            sourceB.StopRecording();
            processor = null;
            sampleBufferA =
            sampleBufferB = null;
            initialTimestampA =
            initialTimestampB = -1L;
        }
        #endregion


        #region --Operations--

        private readonly IAudioDevice sourceA, sourceB;
        private volatile IAudioProcessor processor;
        private List<float> sampleBufferA;
        private List<float> sampleBufferB;
        private long initialTimestampA, initialTimestampB;

        private void OnSampleBufferA (float[] sampleBuffer, int sampleRate, int channelCount, long timestamp) {
            sampleBufferA.AddRange(sampleBuffer);
            initialTimestampA = initialTimestampA == -1L ? timestamp : initialTimestampA;
            lock ((sampleBufferB as ICollection).SyncRoot) {
                // Seek on first check
                if (initialTimestampA > 0 && initialTimestampB > 0) {
                    var delta = initialTimestampA - initialTimestampB; // +ve if B has more samples cos started earlier
                    var deltaSamples = (int)((delta / 1e+9) * sampleRate * channelCount);
                    var aheadBuffer = deltaSamples > 0 ? sampleBufferB : sampleBufferA;
                    aheadBuffer.RemoveRange(0, Math.Min(deltaSamples, aheadBuffer.Count));
                    initialTimestampA =
                    initialTimestampB = 0;
                }
                // Mix subsequently
                if (initialTimestampA == 0 && initialTimestampB == 0) {
                    if (sampleBufferA.Count < sampleBuffer.Length || sampleBufferB.Count < sampleBuffer.Length)
                        return;
                    var samplesA = sampleBufferA.GetRange(0, sampleBuffer.Length).ToArray();
                    var samplesB = sampleBufferB.GetRange(0, sampleBuffer.Length).ToArray();
                    sampleBufferA.RemoveRange(0, sampleBuffer.Length);
                    sampleBufferB.RemoveRange(0, sampleBuffer.Length);
                    Mix(samplesA, samplesB, samplesA);
                    processor.OnSampleBuffer(samplesA, sampleRate, channelCount, timestamp);
                }
            }
        }

        private void OnSampleBufferB (float[] sampleBuffer, int sampleRate, int channelCount, long timestamp) {
            lock ((sampleBufferB as ICollection).SyncRoot) {
                initialTimestampB = initialTimestampB == -1L ? timestamp : initialTimestampB;
                sampleBufferB.AddRange(sampleBuffer);
            }
        }

        private static void Mix (float[] srcA, float[] srcB, float[] dst) {
            for (int i = 0; i < srcA.Length; i++) {
                var sum = srcA[i] + srcB[i];
                var product = srcA[i] * srcB[i];
                var mult = product > 0 ? srcA[i] > 0 ? -1 : 1 : 0;
                dst[i] = sum + mult * product;
            }
        }

        private class PassThroughProcessor : IAudioProcessor {

            private readonly Action<float[], int, int, long> handler;

            public PassThroughProcessor (Action<float[], int, int, long> handler) {
                this.handler = handler;
            }

            void IAudioProcessor.OnSampleBuffer (float[] sampleBuffer, int sampleRate, int channelCount, long timestamp) {
                handler(sampleBuffer, sampleRate, channelCount, timestamp);
            }
        }
        #endregion
    }
}