/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.Internal {

    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class AudioDeviceBridge {

        private const string Assembly =
        #if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        @"__Internal";
        #else
        @"NatMic";
        #endif

        private const UnmanagedType StringType =
        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        UnmanagedType.LPWStr;
        #else
        UnmanagedType.LPStr;
        #endif

        public delegate void SampleBufferCallback (IntPtr context, IntPtr sampleBuffer, int sampleCount, int sampleRate, int channelCount, long timestamp);

        #if UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        [DllImport(Assembly, EntryPoint = @"NMDevices")]
        public static extern void Devices (out IntPtr outDevicesArray, out int outDevicesArrayCount);
        [DllImport(Assembly, EntryPoint = @"NMFreeDevice")]
        public static extern void FreeDevice (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NMDeviceName")]
        public static extern void DeviceName (this IntPtr device, [MarshalAs(StringType)] StringBuilder dest);
        [DllImport(Assembly, EntryPoint = @"NMDeviceUniqueID")]
        public static extern void DeviceUniqueID (this IntPtr device, [MarshalAs(StringType)] StringBuilder dest);
        [DllImport(Assembly, EntryPoint = @"NMDeviceEchoCancellation")]
        public static extern bool DeviceEchoCancellation (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NMIsRecording")]
        public static extern bool IsRecording (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NMStartRecording")]
        public static extern void StartRecording (this IntPtr device, int sampleRate, int channelCount, SampleBufferCallback callback, IntPtr context);
        [DllImport(Assembly, EntryPoint = @"NMStopRecording")]
        public static extern void StopRecording (this IntPtr device);
        [DllImport(Assembly, EntryPoint = @"NMCurrentTimestamp")]
        public static extern long CurrentTimestamp ();
        #else
        public static void Devices (out IntPtr outDevicesArray, out int outDevicesArrayCount) { outDevicesArray = IntPtr.Zero; outDevicesArrayCount = 0; }
        public static void FreeDevice (this IntPtr device) {}
        public static void DeviceName (this IntPtr device, StringBuilder dest) { }
        public static void DeviceUniqueID (this IntPtr device, StringBuilder dest) { }
        public static bool DeviceEchoCancellation (this IntPtr device) { return false; }
        public static bool IsRecording (this IntPtr device) { return false; }
        public static void StartRecording (this IntPtr device, int sampleRate, int channelCount, SampleBufferCallback callback, IntPtr context) {}
        public static void StopRecording (this IntPtr device) {}
        public static long CurrentTimestamp () { return 0L; }
        #endif
    }
}