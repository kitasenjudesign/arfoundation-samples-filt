/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices {

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Internal;

    /// <summary>
    /// Query that can be used to access available media devices.
    /// </summary>
    [Doc(@"MediaDeviceQuery")]
    public sealed partial class MediaDeviceQuery {

        #region --Client API--
        /// <summary>
        /// All devices that meet the provided criteria.
        /// </summary>
        [Doc(@"MediaDeviceQueryDevices")]
        public readonly IMediaDevice[] devices;

        /// <summary>
        /// Current device that meets the provided criteria.
        /// </summary>
        [Doc(@"MediaDeviceQueryCurrentDevice")]
        public IMediaDevice currentDevice => index < count ? devices[index] : null;

        /// <summary>
        /// Number of devices that meet the provided criteria.
        /// </summary>
        [Doc(@"MediaDeviceQueryCount")]
        public int count => devices.Length;

        /// <summary>
        /// Create a media device query.
        /// </summary>
        /// <param name="criteria">Criteria that devices should meet.</param>
        [Doc(@"MediaDeviceQueryCtor")]
        public MediaDeviceQuery (params Criterion[] criteria) {
            // Get media devices
            var devices = new List<IMediaDevice>();
            switch (Application.platform) {
                case RuntimePlatform.Android: goto case RuntimePlatform.IPhonePlayer;
                case RuntimePlatform.IPhonePlayer: devices.AddRange(AudioDevices()); devices.AddRange(CameraDevices()); break;
                case RuntimePlatform.OSXEditor: goto case RuntimePlatform.WindowsPlayer;
                case RuntimePlatform.OSXPlayer: goto case RuntimePlatform.WindowsPlayer;
                case RuntimePlatform.WindowsEditor: goto case RuntimePlatform.WindowsPlayer;
                case RuntimePlatform.WindowsPlayer: devices.AddRange(AudioDevices()); devices.AddRange(WebCamDevices()); break;
                default: devices.AddRange(WebCamDevices()); break;
            }
            // Filter by provided criteria
            this.devices = devices.Where(device => criteria.All(criterion => criterion(device))).ToArray();
        }

        /// <summary>
        /// Advance the next available device that meets the provided criteria.
        /// </summary>
        [Doc(@"MediaDeviceQueryAdvance"), MethodImpl(MethodImplOptions.Synchronized)]
        public void Advance () => index = (index + 1) % devices.Length;
        #endregion


        #region --Operations--

        private int index;

        private static IEnumerable<AudioDevice> AudioDevices () {
            Bridge.AudioDevices(out var deviceArray, out var deviceCount);
            var devices = new AudioDevice[deviceCount];
            for (int i = 0; i < devices.Length; i++)
                devices[i] = new NativeAudioDevice(Marshal.ReadIntPtr(deviceArray, i * Marshal.SizeOf(typeof(IntPtr))));
            Marshal.FreeCoTaskMem(deviceArray);
            return devices;
        }

        private static IEnumerable<CameraDevice> CameraDevices () {
            Bridge.CameraDevices(out var deviceArray, out var deviceCount);
            var devices = new CameraDevice[deviceCount];
            for (var i = 0; i < devices.Length; i++)
                devices[i] = new NativeCameraDevice(Marshal.ReadIntPtr(deviceArray, i * Marshal.SizeOf(typeof(IntPtr))));
            Marshal.FreeCoTaskMem(deviceArray);
            return devices;
        }

        private static IEnumerable<WebCameraDevice> WebCamDevices () => WebCamTexture.devices.Select(device => new WebCameraDevice(device));
        #endregion
    }
}