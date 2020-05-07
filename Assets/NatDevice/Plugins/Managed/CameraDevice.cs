/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices {

    using UnityEngine;
    using System.Threading.Tasks;
    using Internal;
    
    /// <summary>
    /// Abstraction for a hardware camera device.
    /// </summary>
    [Doc(@"CameraDevice")]
    public abstract class CameraDevice : ICameraDevice {

        #region --Properties--
        /// <summary>
        /// Device unique ID.
        /// </summary>
        [Doc(@"UniqueID")]
        public abstract string uniqueID { get; }

        /// <summary>
        /// Is this camera front facing?
        /// </summary>
        [Doc(@"FrontFacing")]
        public abstract bool frontFacing { get; }

        /// <summary>
        /// Is flash supported for photo capture?
        /// </summary>
        [Doc(@"FlashSupported")]
        public abstract bool flashSupported { get; }

        /// <summary>
        /// Is torch supported?
        /// </summary>
        [Doc(@"TorchSupported")]
        public abstract bool torchSupported { get; }

        /// <summary>
        /// Is exposure lock supported?
        /// </summary>
        [Doc(@"ExposureLockSupported")]
        public abstract bool exposureLockSupported { get; }

        /// <summary>
        /// Is focus lock supported
        /// </summary>
        [Doc(@"FocusLockSupported")]
        public abstract bool focusLockSupported { get; }

        /// <summary>
        /// Is white balance lock supported?
        /// </summary>
        [Doc(@"WhiteBalanceLockSupported")]
        public abstract bool whiteBalanceLockSupported { get; }

        /// <summary>
        /// Field of view in degrees.
        /// </summary>
        [Doc(@"FieldOfView")]
        public abstract (float width, float height) fieldOfView { get; }

        /// <summary>
        /// Exposure bias range.
        /// </summary>
        [Doc(@"ExposureRange")]
        public abstract (float min, float max) exposureRange { get; }

        /// <summary>
        /// Zoom ratio range.
        /// </summary>
        [Doc(@"ZoomRange")]
        public abstract (float min, float max) zoomRange { get; }

        /// <summary>
        /// Get or set the preview resolution.
        /// </summary>
        [Doc(@"PreviewResolution")]
        public abstract (int width, int height) previewResolution { get; set; }

        /// <summary>
        /// Get or set the photo resolution.
        /// </summary>
        [Doc(@"PhotoResolution")]
        public abstract (int width, int height) photoResolution { get; set; }

        /// <summary>
        /// Get or set the preview framerate.
        /// </summary>
        [Doc(@"Framerate")]
        public abstract int frameRate { get; set; }

        /// <summary>
        /// Get or set the exposure bias.
        /// This value must be in the range returned by `exposureRange`.
        /// </summary>
        [Doc(@"ExposureBias", @"ExposureBiasDiscussion")]
        public abstract float exposureBias { get; set; }

        /// <summary>
        /// Get or set the exposure lock.
        /// </summary>
        [Doc(@"ExposureLock")]
        public abstract bool exposureLock { get; set; }

        /// <summary>
        /// Set the exposure point of interest.
        /// </summary>
        //[Doc(@"ExposurePoint", @"ExposurePointDiscussion")]
        public abstract (float x, float y) exposurePoint { set; }

        /// <summary>
        /// Get or set the photo flash mode.
        /// </summary>
        [Doc(@"PhotoFlashMode")]
        public abstract FlashMode flashMode { get; set; }

        /// <summary>
        /// Get or set the focus lock.
        /// </summary>
        [Doc(@"FocusLock")]
        public abstract bool focusLock { get; set; }

        /// <summary>
        /// Set the focus point of interest.
        /// </summary>
        //[Doc(@"FocusPoint", @"FocusPointDiscussion")]
        public abstract (float x, float y) focusPoint { set; }

        /// <summary>
        /// Get or set the torch mode.
        /// </summary>
        [Doc(@"TorchEnabled")]
        public abstract bool torchEnabled { get; set; }

        /// <summary>
        /// Get or set the white balance lock.
        /// </summary>
        [Doc(@"WhiteBalanceLock")]
        public abstract bool whiteBalanceLock { get; set; }

        /// <summary>
        /// Get or set the zoom ratio.
        /// This value must be in the range returned by `zoomRange`.
        /// </summary>
        [Doc(@"ZoomRatio")]
        public abstract float zoomRatio { get; set; }

        /// <summary>
        /// Set the preview orientation.
        /// Defaults to the screen orientation.
        /// </summary>
        //[Doc(@"Orientation")]
        public abstract FrameOrientation orientation { set; }
        #endregion


        #region --Operations--
        /// <summary>
        /// Is the device running?
        /// </summary>
        [Doc(@"Running")]
        public abstract bool running { get; }

        /// <summary>
        /// Start running.
        /// </summary>
        /// <param name="@delegate">Delegate to receive frame textures.</param>
        [Doc(@"StartPreview")]
        public abstract Task<Texture2D> StartRunning ();

        /// <summary>
        /// Stop running.
        /// </summary>
        [Doc(@"StopRunning")]
        public abstract void StopRunning ();

        /// <summary>
        /// Capture a photo.
        /// </summary>
        [Doc(@"CapturePhoto", @"CapturePhotoDiscussion")]
        public abstract Task<Texture2D> CapturePhoto ();
        #endregion


        #region --Operations--

        public bool Equals (IMediaDevice other) => other != null && other is CameraDevice && other.uniqueID == uniqueID;

        public override string ToString () => $"camera:{uniqueID}";
        #endregion
    }
}