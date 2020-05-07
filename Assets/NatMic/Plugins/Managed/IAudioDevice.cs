/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic {

    /// <summary>
    /// An audio input device
    /// </summary>
    public interface IAudioDevice {
        /// <summary>
        /// Is the device currently recording?
        /// </summary>
        bool IsRecording { get; }
        /// <summary>
        /// Start recording
        /// </summary>
        /// <param name="requestedSampleRate">Requested sample rate</param>
        /// <param name="requestedChannelCount">Requested channel count</param>
        /// <param name="processor">Delegate to receive audio sample buffers</param>
        void StartRecording (int requestedSampleRate, int requestedChannelCount, IAudioProcessor processor);
        /// <summary>
        /// Stop recording
        /// </summary>
        void StopRecording ();
    }

    /// <summary>
    /// An audio data receiver supplied by an audio device
    /// </summary>
    public interface IAudioProcessor {
        /// <summary>
        /// Delegate invoked when microphone reports a new sample buffer
        /// </summary>
        /// <param name="sampleBuffer">Audio sample buffer interleaved by channel</param>
        /// <param name="sampleRate">Audio sample rate</param>
        /// <param name="channelCount">Audio channel count</param>
        /// <param name="timestamp">Buffer timestamp</param>
        void OnSampleBuffer (float[] sampleBuffer, int sampleRate, int channelCount, long timestamp);
    }
}