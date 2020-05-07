/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices.Internal {

    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class Bridge {

        private const string Assembly =
        #if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        @"__Internal";
        #else
        @"NatDevice";
        #endif


        #region --Delegates--
        public delegate void FrameDelegate (IntPtr context, IntPtr pixelBuffer, int width, int height, long timestamp);
        public delegate void SampleBufferDelegate (IntPtr context, IntPtr sampleBuffer, int sampleCount, long timestamp);
        #endregion
        
    
        #region --IMediaDevice--
        [DllImport(Assembly, EntryPoint = @"NDDispose")]
        public static extern void Dispose (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDUniqueID")]
        public static extern void UniqueID (this IntPtr device, [MarshalAs(UnmanagedType.LPStr)] StringBuilder dest);
        [DllImport(Assembly, EntryPoint = @"NDRunning")]
        public static extern bool Running (this IntPtr camera);
        [DllImport(Assembly, EntryPoint = @"NDAudioDeviceStartRunning")]
        public static extern bool StartRunning (this IntPtr device, SampleBufferDelegate callback, IntPtr context);
        [DllImport(Assembly, EntryPoint = @"NDStopRunning")]
        public static extern void StopRunning (this IntPtr device);
        #endregion


        #region --AudioDevice--
        [DllImport(Assembly, EntryPoint = @"NDAudioDevices")]
        public static extern void AudioDevices (out IntPtr outDevicesArray, out int outDevicesArrayCount);
        [DllImport(Assembly, EntryPoint = @"NDName")]
        public static extern void Name (this IntPtr device, [MarshalAs(UnmanagedType.LPStr)] StringBuilder dest);
        [DllImport(Assembly, EntryPoint = @"NDEchoCancellation")]
        public static extern bool EchoCancellation (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSampleRate")]
        public static extern int SampleRate (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetSampleRate")]
        public static extern void SampleRate (this IntPtr device, int sampleRate);
        [DllImport(Assembly, EntryPoint = @"NDChannelCount")]
        public static extern int ChannelCount (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetChannelCount")]
        public static extern void ChannelCount (this IntPtr device, int sampleRate);
        #endregion


        #region --CameraDevice--
        [DllImport(Assembly, EntryPoint = @"NDCameraDevices")]
        public static extern void CameraDevices (out IntPtr outDevicesArray, out int outDevicesArrayCount);
        [DllImport(Assembly, EntryPoint = @"NDFrontFacing")]
        public static extern bool FrontFacing (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDFlashSupported")]
        public static extern bool FlashSupported (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDTorchSupported")]
        public static extern bool TorchSupported (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDExposureLockSupported")]
        public static extern bool ExposureLockSupported (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDFocusLockSupported")]
        public static extern bool FocusLockSupported (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDWhiteBalanceLockSupported")]
        public static extern bool WhiteBalanceLockSupported (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDFieldOfView")]
        public static extern void FieldOfView (this IntPtr device, out float x, out float y);
        [DllImport(Assembly, EntryPoint = @"NDExposureRange")]
        public static extern void ExposureRange (this IntPtr device, out float min, out float max);
        [DllImport(Assembly, EntryPoint = @"NDZoomRange")]
        public static extern void ZoomRange (this IntPtr device, out float min, out float max);
        [DllImport(Assembly, EntryPoint = @"NDPreviewResolution")]
        public static extern void PreviewResolution (this IntPtr device, out int width, out int height);
        [DllImport(Assembly, EntryPoint = @"NDSetPreviewResolution")]
        public static extern void PreviewResolution (this IntPtr device, int width, int height);
        [DllImport(Assembly, EntryPoint = @"NDPhotoResolution")]
        public static extern void PhotoResolution (this IntPtr device, out int width, out int height);
        [DllImport(Assembly, EntryPoint = @"NDSetPhotoResolution")]
        public static extern void PhotoResolution (this IntPtr device, int width, int height);
        [DllImport(Assembly, EntryPoint = @"NDFramerate")]
        public static extern int Framerate (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetFramerate")]
        public static extern void Framerate (this IntPtr device, int framerate);
        [DllImport(Assembly, EntryPoint = @"NDExposureBias")]
        public static extern float ExposureBias (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetExposureBias")]
        public static extern void ExposureBias (this IntPtr device, float bias);
        [DllImport(Assembly, EntryPoint = @"NDSetExposurePoint")]
        public static extern void ExposurePoint (this IntPtr device, float x, float y);
        [DllImport(Assembly, EntryPoint = @"NDExposureLock")]
        public static extern bool ExposureLock (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetExposureLock")]
        public static extern void ExposureLock (this IntPtr device, bool locked);
        [DllImport(Assembly, EntryPoint = @"NDFlashMode")]
        public static extern FlashMode FlashMode (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetFlashMode")]
        public static extern void FlashMode (this IntPtr device, FlashMode state);
        [DllImport(Assembly, EntryPoint = @"NDFocusLock")]
        public static extern bool FocusLock (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetFocusLock")]
        public static extern void FocusLock (this IntPtr device, bool locked);
        [DllImport(Assembly, EntryPoint = @"NDSetFocusPoint")]
        public static extern void FocusPoint (this IntPtr device, float x, float y);
        [DllImport(Assembly, EntryPoint = @"NDTorchEnabled")]
        public static extern bool TorchEnabled (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetTorchEnabled")]
        public static extern void TorchEnabled (this IntPtr device, bool enabled);
        [DllImport(Assembly, EntryPoint = @"NDWhiteBalanceLock")]
        public static extern bool WhiteBalanceLock (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetWhiteBalanceLock")]
        public static extern void WhiteBalanceLock (this IntPtr device, bool locked);
        [DllImport(Assembly, EntryPoint = @"NDZoomRatio")]
        public static extern float ZoomRatio (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NDSetZoomRatio")]
        public static extern void ZoomRatio (this IntPtr device, float ratio);
        [DllImport(Assembly, EntryPoint = @"NDSetOrientation")]
        public static extern void Orientation (this IntPtr device, FrameOrientation orentation);
        [DllImport(Assembly, EntryPoint = @"NDCameraDeviceStartRunning")]
        public static extern void StartRunning (this IntPtr device, FrameDelegate handler, IntPtr context);
        [DllImport(Assembly, EntryPoint = @"NDCapturePhoto")]
        public static extern void CapturePhoto (this IntPtr device, FrameDelegate handler, IntPtr context);
        #endregion
    }
}