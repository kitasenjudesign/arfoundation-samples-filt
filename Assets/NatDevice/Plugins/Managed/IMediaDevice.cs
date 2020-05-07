/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices {

    using UnityEngine;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Media device which provides media buffers.
    /// </summary>
    public interface IMediaDevice : IEquatable<IMediaDevice> {

        /// <summary>
        /// Device unique ID.
        /// </summary>
        string uniqueID { get; }

        /// <summary>
        /// Is the device running?
        /// </summary>
        bool running { get; }

        /// <summary>
        /// Stop running.
        /// </summary>
        void StopRunning ();
    }

    /// <summary>
    /// Audio device which provides sample buffers.
    /// </summary>
    public interface IAudioDevice : IMediaDevice {

        /// <summary>
        /// Audio sample rate.
        /// </summary>
        int sampleRate { get; }

        /// <summary>
        /// Audio channel count.
        /// </summary>
        int channelCount { get; }

        /// <summary>
        /// Start running.
        /// </summary>
        /// <param name="@delegate">Delegate to receive sample buffers.</param>
        void StartRunning (SampleBufferDelegate @delegate);
    }

    /// <summary>
    /// Camera device which provides pixel buffers.
    /// </summary>
    public interface ICameraDevice : IMediaDevice {

        /// <summary>
        /// Is the camera front facing?
        /// </summary>
        bool frontFacing { get; }

        /// <summary>
        /// Get or set the current preview resolution of the camera.
        /// </summary>
        (int width, int height) previewResolution { get; set; }

        /// <summary>
        /// Get or set the current framerate of the camera.
        /// </summary>
        int frameRate { get; set; }

        /// <summary>
        /// Start running.
        /// </summary>
        Task<Texture2D> StartRunning ();
    }

    /// <summary>
    /// Delegate invoked when audio device reports a new sample buffer.
    /// </summary>
    /// <param name="sampleBuffer">PCM sample buffer interleaved by channel.</param>
    /// <param name="timestamp">Sample buffer timestamp in nanoseconds.</param>
    public delegate void SampleBufferDelegate (float[] sampleBuffer, long timestamp);
}