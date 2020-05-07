/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices.Internal {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public class NativeCameraDevice : CameraDevice {

        #region --Properties--
        
        public override string uniqueID {
            get {
                var result = new StringBuilder(1024);
                device.UniqueID(result);
                return result.ToString();
            }
        }

        public override bool frontFacing => device.FrontFacing();

        public override bool flashSupported => device.FlashSupported();

        public override bool torchSupported => device.TorchSupported();

        public override bool exposureLockSupported => device.ExposureLockSupported();

        public override bool focusLockSupported => device.FocusLockSupported();

        public override bool whiteBalanceLockSupported => device.WhiteBalanceLockSupported();

        public override (float width, float height) fieldOfView {
            get {
                device.FieldOfView(out var width, out var height);
                return (width, height);
            }
        }

        public override (float min, float max) exposureRange {
            get {
                device.ExposureRange(out var min, out var max);
                return (min, max);
            }
        }

        public override (float min, float max) zoomRange {
            get {
                device.ZoomRange(out var min, out var max);
                return (min, max);
            }
        }

        public override (int width, int height) previewResolution {
            get { device.PreviewResolution(out var width, out var height); return (width, height); }
            set => device.PreviewResolution(value.width, value.height);
        }

        public override (int width, int height) photoResolution {
            get { device.PhotoResolution(out var width, out var height); return (width, height); }
            set => device.PhotoResolution(value.width, value.height);
        }
        
        public override int frameRate {
            get => device.Framerate();
            set => device.Framerate(value);
        }

        public override float exposureBias {
            get => device.ExposureBias();
            set => device.ExposureBias(value);
        }

        public override bool exposureLock {
            get => device.ExposureLock();
            set => device.ExposureLock(value);
        }

        public override (float x, float y) exposurePoint {
            set => device.ExposurePoint(value.x, value.y);
        }

        public override FlashMode flashMode {
            get => device.FlashMode();
            set => device.FlashMode(value);
        }

        public override bool focusLock {
            get => device.FocusLock();
            set => device.FocusLock(value);
        }

        public override (float x, float y) focusPoint {
            set => device.FocusPoint(value.x, value.y);
        }

        public override bool torchEnabled {
            get => device.TorchEnabled();
            set => device.TorchEnabled(value);
        }

        public override bool whiteBalanceLock {
            get => device.WhiteBalanceLock();
            set => device.WhiteBalanceLock(value);
        }

        public override float zoomRatio {
            get => device.ZoomRatio();
            set => device.ZoomRatio(value);
        }

        public override FrameOrientation orientation {
            set => device.Orientation(value);
        }
        #endregion


        #region --Preview--

        public override bool running => device.Running();

        public override Task<Texture2D> StartRunning () {
            var startTask = new TaskCompletionSource<Texture2D>();
            Action<IntPtr, int, int, long> handler = (pixelBuffer, width, height, timestamp) => {
                // Update preview texture
                var firstFrame = !previewTexture;
                previewTexture = previewTexture ?? new Texture2D(width, height, TextureFormat.RGBA32, false, false);
                previewTexture.LoadRawTextureData(pixelBuffer, width * height * 4);
                previewTexture.Apply();
                // Complete task
                if (firstFrame)
                    startTask.SetResult(previewTexture);
            };
            device.StartRunning(OnFrame, (IntPtr)GCHandle.Alloc(handler, GCHandleType.Normal));
            return startTask.Task;
        }

        public override void StopRunning () {
            device.StopRunning();
            Texture2D.Destroy(previewTexture);
            previewTexture = null;
        }

        public override Task<Texture2D> CapturePhoto () {
            var captureTask = new TaskCompletionSource<Texture2D>();
            GCHandle handle;
            Action<IntPtr, int, int, long> handler = (pixelBuffer, width, height, timestamp) => {
                handle.Free();
                // Create photo texture
                var photoTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                photoTexture.LoadRawTextureData(pixelBuffer, width * height * 4);
                photoTexture.Apply();
                // Complete task
                captureTask.SetResult(photoTexture);
            };
            handle = GCHandle.Alloc(handler, GCHandleType.Normal);
            device.CapturePhoto(OnFrame, (IntPtr)handle);
            return captureTask.Task;
        }
        #endregion


        #region --Operations--

        private readonly IntPtr device;
        private Texture2D previewTexture;

        public NativeCameraDevice (IntPtr device) {
            this.device = device;
            // Set orientation
            switch (Screen.orientation) {
                case ScreenOrientation.LandscapeLeft: orientation = FrameOrientation.LandscapeLeft; break;
                case ScreenOrientation.Portrait: orientation = FrameOrientation.Portrait; break;
                case ScreenOrientation.LandscapeRight: orientation = FrameOrientation.LandscapeRight; break;
                case ScreenOrientation.PortraitUpsideDown: orientation = FrameOrientation.PortraitUpsideDown; break;
            }
        }

        ~NativeCameraDevice () => device.Dispose();

        [MonoPInvokeCallback(typeof(Bridge.FrameDelegate))]
        private static void OnFrame (IntPtr context, IntPtr pixelBuffer, int width, int height, long timestamp) => (((GCHandle)context).Target as Action<IntPtr, int, int, long>)(pixelBuffer, width, height, timestamp);
        #endregion
    }
}