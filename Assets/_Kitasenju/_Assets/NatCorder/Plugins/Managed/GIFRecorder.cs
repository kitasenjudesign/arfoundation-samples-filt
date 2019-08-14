/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder {

    using UnityEngine;
    using System;
    using System.IO;
    using Internal;

    /// <summary>
    /// Animated GIF image recorder.
    /// Recorded GIF will loop forever.
    /// </summary>
    [Doc(@"GIFRecorder")]
    public sealed class GIFRecorder : IMediaRecorder, IAbstractRecorder {

        #region --Client API--
        /// <summary>
        /// Video width
        /// </summary>
        [Doc(@"PixelWidth")]
        public int pixelWidth {
            get { return recorder.pixelWidth; }
        }
        
        /// <summary>
        /// Video height
        /// </summary>
        [Doc(@"PixelHeight")]
        public int pixelHeight {
            get { return recorder.pixelHeight; }
        }

        /// <summary>
        /// Create a GIF recorder
        /// </summary>
        /// <param name="imageWidth">Image width</param>
        /// <param name="imageHeight">Image height</param>
        /// <param name="frameDuration">Frame duration in seconds</param>
        /// <param name="recordingCallback">Recording callback</param>
        [Doc(@"GIFRecorderCtor")]
        public GIFRecorder (int imageWidth, int imageHeight, float frameDuration, Action<string> recordingCallback) {
            imageWidth = imageWidth >> 1 << 1;
            imageHeight = imageHeight >> 1 << 1;
            var recordingDirectory = Application.persistentDataPath;
            var recordingFilename = string.Format("recording_{0}.gif", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
            switch (Application.platform) {
                case RuntimePlatform.OSXEditor:
                    recordingDirectory = Directory.GetCurrentDirectory();
                    goto case RuntimePlatform.OSXPlayer;
                case RuntimePlatform.WindowsEditor:
                    recordingDirectory = Directory.GetCurrentDirectory();
                    goto case RuntimePlatform.WindowsPlayer;
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.IPhonePlayer: {
                    var recordingPath = Path.Combine(recordingDirectory, recordingFilename);
                    var nativeRecorder = MediaRecorderBridge.CreateGIFRecorder(imageWidth, imageHeight, frameDuration);
                    this.recorder = new MediaRecorderiOS(nativeRecorder, imageWidth, imageHeight, recordingPath, recordingCallback);
                    break;
                }
                case RuntimePlatform.Android: {
                    var recordingPath = Path.Combine(recordingDirectory, recordingFilename);
                    var nativeRecorder = new AndroidJavaObject(@"com.yusufolokoba.natcorder.GIFRecorder", imageWidth, imageHeight, frameDuration);
                    this.recorder = new MediaRecorderAndroid(nativeRecorder, imageWidth, imageHeight, recordingPath, recordingCallback);
                    break;
                }
                default:
                    Debug.LogError("NatCorder Error: GIFRecorder is not supported on this platform");
                    this.recorder = null; // Self-destruct >:D
                    break;
            }
        }

        /// <summary>
        /// Stop recording and dispose the recorder.
        /// The recording callback is expected to be invoked soon after calling this method.
        /// </summary>
        [Doc(@"Dispose", @"DisposeDiscussion")]
        public void Dispose () {
            recorder.Dispose();
        }

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST in the RGBA32 format.
        /// </summary>
        /// <param name="pixelBuffer">Pixel buffer containing video frame to commit</param>
        /// <param name="timestamp">Sample buffer timestamp in nanoseconds</param>
        [Doc(@"CommitFrame", @"CommitFrameDiscussion"), Code(@"RecordWebCam")]
        public void CommitFrame<T> (T[] pixelBuffer, long timestamp) where T : struct {
            recorder.CommitFrame(pixelBuffer, timestamp);
        }

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST in the RGBA32 format.
        /// </summary>
        /// <param name="nativeBuffer">Pixel buffer in native memory to commit</param>
        /// <param name="timestamp">Sample buffer timestamp in nanoseconds</param>
        [Doc(@"CommitFrame", @"CommitFrameDiscussion")]
        public void CommitFrame (IntPtr nativeBuffer, long timestamp) {
            recorder.CommitFrame(nativeBuffer, timestamp);
        }

        /// <summary>
        /// Not used on GIF recorders.
        /// </summary>
        [Doc(@"CommitSamplesGIF")]
        public void CommitSamples (float[] sampleBuffer, long timestamp) { }
        #endregion

        private readonly IMediaRecorder recorder;
        IMediaRecorder IAbstractRecorder.recorder {
            get { return recorder; }
        }
    }
}